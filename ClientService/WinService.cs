using System.Data;
using System.Data.SqlClient;
using System.ServiceProcess;
using Network;
using SharedData;
using ClientConfig;

namespace ClientService;

#pragma warning disable CA1416
public class WinService : ServiceBase
{
    private const string SERVICE_NAME = "ClientService";
    private Config _config = new Config();
    private bool _isWork = false;
    private TasksInfo _tasks;
    private TcpConnection? _client;
    private string _address = "";

    public WinService()
    {
        this.ServiceName = SERVICE_NAME;
        this.CanStop = true;
        this.CanPauseAndContinue = false;
        this.AutoLog = false;

        _tasks = new TasksInfo();
        _client = null;
    }

    protected override async void OnStart(string[] args)
    {
        var configFilename = Path.Combine(Environment.CurrentDirectory, "Config.json");
        _config = await Config.LoadFromFileAsync(configFilename);

        await Connect();
        
        _isWork = true; 
        await Task.Run(Handler);
    }
    
    protected override void OnStop()
    {
        base.OnStop();
        _isWork = false;
    }

    private async Task Connect()
    {
        var result = await ConnectionFactory.CreateTcpConnectionAsync(_config.ServerIp, _config.ServerPort);
        if (result.Item2 == ConnectionResult.Connected)
        {
            _client = result.Item1;
            _client.RegisterPacketHandler<SharedRequest>(RecvHandler, this);
            _client.TIMEOUT = 600000;
            _address = _client.IPLocalEndPoint.Address.MapToIPv4().ToString();
        }
    }

    private async void RecvHandler(SharedRequest packet, Connection connection)
    {
        string result = "Error";
        switch (packet.Command)
        {
            case "tasks":
                {
                    _tasks = TasksInfo.FromArray(packet.Data);
                    result = "OK";
                    break;
                }
            case "restore":
                {
                    var files = FilesInfo.FromBin(packet.Data);
                    foreach (var file in files.Data)
                    {
                        if (!_tasks.Data.ContainsKey(file.Id))
                            continue;
                        var task = _tasks.Data[file.Id];
                        if (task.Address != _address)
                            continue;
                        if (task is FileBackupTask)
                        {
                            await Task.Run(() => Directory.CreateDirectory(Path.GetDirectoryName(file.NameFile)));
                            await File.WriteAllBytesAsync(file.NameFile, file.Bin);
                            result = "OK";
                        }
                        else if (task is (SQLBackupTask))
                        {
                            var dbTask = task as SQLBackupTask;
                            string fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, file.NameFile + ".bak");
                            await File.WriteAllBytesAsync(fullPath, file.Bin);

                            try
                            {
                                SqlConnectionStringBuilder connStringBuilder = new SqlConnectionStringBuilder();
                                connStringBuilder.DataSource = dbTask.Server;
                                connStringBuilder.UserID = dbTask.Login;
                                connStringBuilder.Password = dbTask.Password;
                                using (SqlConnection conn = new SqlConnection(connStringBuilder.ConnectionString))
                                {
                                    string dataFile = $"C:\\{file.NameFile}.mdf";
                                    string logFile = $"C:\\{file.NameFile}_log.ldf";
                                    await conn.OpenAsync();
                                    var query = @"RESTORE FILELISTONLY FROM DISK = @localDatabasePath";
                                    var sqlCommand = new SqlCommand(query, conn);
                                    sqlCommand.Parameters.AddWithValue("@localDatabasePath", fullPath);
                                    var sqlDataReader = await sqlCommand.ExecuteReaderAsync();
                                    while (await sqlDataReader.ReadAsync())
                                    {
                                        if (sqlDataReader["Type"].ToString() == "D")
                                            dataFile = sqlDataReader["PhysicalName"].ToString().Replace(dbTask.DbName, file.NameFile);
                                        else if (sqlDataReader["Type"].ToString() == "L")
                                            logFile = sqlDataReader["PhysicalName"].ToString().Replace(dbTask.DbName, file.NameFile);
                                    }
                                    await sqlDataReader.CloseAsync();
                                    query = @"RESTORE DATABASE @databaseName 
                                        FROM DISK = @localDatabasePath 
                                        WITH REPLACE, 
                                            MOVE @TemplateDatabase TO @NewDatabaseData,
                                            MOVE @TemplateDatabaseLog TO @NewDatabaseLog;";
                                    sqlCommand = new SqlCommand(query, conn);
                                    sqlCommand.Parameters.AddWithValue("@databaseName", file.NameFile);
                                    sqlCommand.Parameters.AddWithValue("@localDatabasePath", fullPath);
                                    sqlCommand.Parameters.AddWithValue("@TemplateDatabase", dbTask.DbName);
                                    sqlCommand.Parameters.AddWithValue("@TemplateDatabaseLog", dbTask.DbName+"_log");
                                    sqlCommand.Parameters.AddWithValue("@NewDatabaseData", dataFile);
                                    sqlCommand.Parameters.AddWithValue("@NewDatabaseLog", logFile);
                                    await sqlCommand.ExecuteNonQueryAsync();
                                }
                                result = "OK";
                            }
                            catch (Exception ex)
                            {
                                await File.AppendAllTextAsync(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log.txt"), ex.Message + "\n");
                            }   
                            finally
                            {
                                FileInfo fi = new FileInfo(fullPath);
                                await fi.DeleteAsync();
                            }                            
                        }
                        else if (task is (PGBackupTask))
                        {
                            var pgTask = task as PGBackupTask;
                            string fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, file.NameFile + ".backup");
                            await File.WriteAllBytesAsync(fullPath, file.Bin);

                            try
                            {
                                PgStore.Config pgConfig = new PgStore.Config();
                                pgConfig.ServerName = pgTask.Host;
                                pgConfig.Port = pgTask.Port;
                                pgConfig.UserName = pgTask.UserId;
                                pgConfig.Password = pgTask.Password;
                                pgConfig.DataBase = file.NameFile;
                                PgStore.Control.CurrentConfig = pgConfig;
                                PgStore.Control.ResultChanged += Control_ResultChanged;
                                await Task.Factory.StartNew(() => PgStore.Control.Restaure(fullPath, true));

                                result = "OK";
                            }
                            catch (Exception ex)
                            {
                                await File.AppendAllTextAsync(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log.txt"), ex.Message + "\n");
                            }
                            finally
                            {
                                FileInfo fi = new FileInfo(fullPath);
                                await fi.DeleteAsync();
                            }                            
                        }

                        if (result == "OK")
                        {
                            task.AddAction(TaskAction.Restore);
                            _tasks.Data[task.Id] = task;
                            
                        }
                    }
                    break;
                }
            default:
                result = "Unknown command";
                break;
        }

        connection.Send(new SharedResponse(result, packet));
        if (result == "OK")
        {
            await connection.SendAsync<SharedResponse>(new SharedRequest()
            {
                Command = "tasks",
                Data = _tasks.ToArray()
            });
        }
    }

    private async Task<long> FileBackup(FileBackupTask fileTask, FilesInfo filesForBackup, 
        List<BackupTask> updatedTasks, List<string> filesForDelete, long quotaAddBytes)
    {
        if (File.Exists(fileTask.FileName))
        {
            FileInfo fi = new FileInfo(fileTask.FileName);
            if (fi.Length + quotaAddBytes + _tasks.UsedQuota <= _tasks.MaxQuota)
            {
                quotaAddBytes += fi.Length;
                var backupTime = DateTime.Now;
                filesForBackup.Add(fileTask.Id, backupTime, fileTask.FileName, await File.ReadAllBytesAsync(fileTask.FileName));
                FileBackupTask updatedTask = fileTask;
                updatedTask.Status = SharedData.TaskStatus.Working;
                updatedTask.AddAction(TaskAction.Backup);
                updatedTask.UpdateNextBackupTime();
                updatedTask.BackupTimes.Add(backupTime);
                updatedTasks.Add(updatedTask);
            }
            else
            {
                FileBackupTask updatedTask = fileTask;
                updatedTask.Status = SharedData.TaskStatus.Error_Quota;
                updatedTask.UpdateNextBackupTime();
                updatedTasks.Add(updatedTask);
            }
        }
        else
        {
            FileBackupTask updatedTask = fileTask;
            updatedTask.Status = SharedData.TaskStatus.Error_NoFile;
            updatedTask.UpdateNextBackupTime();
            updatedTasks.Add(updatedTask);
        }

        return quotaAddBytes;
    }

    private async Task<long> SqlServerBackup(SQLBackupTask dbTask, FilesInfo filesForBackup, 
        List<BackupTask> updatedTasks, List<string> filesForDelete, long quotaAddBytes)
    {
        string fileName = $"{dbTask.DbName}.bak";
        string fullPath = Path.Combine(Path.GetTempPath(), fileName);
        try
        {
            SqlConnectionStringBuilder connStringBuilder = new SqlConnectionStringBuilder();
            connStringBuilder.DataSource = dbTask.Server;
            connStringBuilder.UserID = dbTask.Login;
            connStringBuilder.Password = dbTask.Password;
            using (SqlConnection connection = new SqlConnection(connStringBuilder.ConnectionString))
            {
                await connection.OpenAsync();
                var formatMediaName = $"DatabaseToolkitBackup_{dbTask.DbName}";
                var formatName = $"Full Backup of {dbTask.DbName}";
                string query = "BACKUP DATABASE @databaseName TO DISK = @localDatabasePath WITH FORMAT, MEDIANAME = @formatMediaName, NAME = @formatName";
                var sqlCommand = new SqlCommand(query, connection);
                sqlCommand.Parameters.AddWithValue("@databaseName", dbTask.DbName);
                sqlCommand.Parameters.AddWithValue("@localDatabasePath", fullPath);
                sqlCommand.Parameters.AddWithValue("@formatMediaName", formatMediaName);
                sqlCommand.Parameters.AddWithValue("@formatName", formatName);
                await sqlCommand.ExecuteNonQueryAsync();
            }

            FileInfo fi = new FileInfo(fullPath);
            if (fi.Length + quotaAddBytes + _tasks.UsedQuota <= _tasks.MaxQuota)
            {
                quotaAddBytes += fi.Length;
                var backupTime = DateTime.Now;
                filesForBackup.Add(dbTask.Id, backupTime, fullPath, await File.ReadAllBytesAsync(fullPath));
                SQLBackupTask updatedTask = dbTask;
                updatedTask.Status = SharedData.TaskStatus.Working;
                updatedTask.AddAction(TaskAction.Backup);
                updatedTask.UpdateNextBackupTime();
                updatedTask.BackupTimes.Add(backupTime);
                updatedTasks.Add(updatedTask);
            }
            else
            {
                SQLBackupTask updatedTask = dbTask;
                updatedTask.Status = SharedData.TaskStatus.Error_Quota;
                updatedTask.UpdateNextBackupTime();
                updatedTasks.Add(updatedTask);
            }
        }
        catch (Exception ex)
        {
            await File.AppendAllTextAsync(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log.txt"), ex.Message + "\n");
            SQLBackupTask updatedTask = dbTask;
            updatedTask.Status = SharedData.TaskStatus.Error_DbConnect;
            updatedTask.UpdateNextBackupTime();
            updatedTasks.Add(updatedTask);
        }

        return quotaAddBytes;
    }

    void Control_ResultChanged(object sender, EventArgs e)
    {
        File.AppendAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log.txt"), sender.ToString() + "\n");
    }

    private async Task<long> PgSqlBackup(PGBackupTask pgTask, FilesInfo filesForBackup, 
        List<BackupTask> updatedTasks, List<string> filesForDelete, long quotaAddBytes)
    {
        string fileName = $"{pgTask.DbName}.backup";
        string fullPath = Path.Combine(Path.GetTempPath(), fileName);
        try
        {
            PgStore.Config pgConfig = new PgStore.Config();
            pgConfig.ServerName = pgTask.Host;
            pgConfig.Port = pgTask.Port;
            pgConfig.UserName = pgTask.UserId;
            pgConfig.Password = pgTask.Password;
            pgConfig.DataBase = pgTask.DbName;
            PgStore.Control.CurrentConfig = pgConfig;
            PgStore.Control.ResultChanged += Control_ResultChanged;
            await Task.Factory.StartNew(() => PgStore.Control.Backup(Path.GetTempPath(), fileName, true));

            FileInfo fi = new FileInfo(fullPath);
            if (fi.Length + quotaAddBytes + _tasks.UsedQuota <= _tasks.MaxQuota)
            {
                quotaAddBytes += fi.Length;
                var backupTime = DateTime.Now;
                filesForBackup.Add(pgTask.Id, backupTime, fullPath, await File.ReadAllBytesAsync(fullPath));
                PGBackupTask updatedTask = pgTask;
                updatedTask.Status = SharedData.TaskStatus.Working;
                updatedTask.AddAction(TaskAction.Backup);
                updatedTask.UpdateNextBackupTime();
                updatedTask.BackupTimes.Add(backupTime);
                updatedTasks.Add(updatedTask);
            }
            else
            {
                PGBackupTask updatedTask = pgTask;
                updatedTask.Status = SharedData.TaskStatus.Error_Quota;
                updatedTask.UpdateNextBackupTime();
                updatedTasks.Add(updatedTask);
            }
        }
        catch (Exception ex)
        {
            await File.AppendAllTextAsync(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log.txt"), ex.Message + "\n");
            PGBackupTask updatedTask = pgTask;
            updatedTask.Status = SharedData.TaskStatus.Error_DbConnect;
            updatedTask.UpdateNextBackupTime();
            updatedTasks.Add(updatedTask);
        }

        return quotaAddBytes;
    }

    private async Task Handler()
    {
        while (_isWork)
        {
            //Если нет подключения, то пытаемся подключиться к серверу
            if (_client == null || !_client.IsAlive)
            {
                await Connect();
            }
            else 
            {
                try
                {
                    long quotaAddBytes = 0;
                    var filesForBackup = new FilesInfo();
                    var updatedTasks = new List<BackupTask>();
                    var filesForDelete = new List<string>();
                    foreach (var task in _tasks.Data.Values.Where(task => 
                        task.NextBackupTime <= DateTime.Now 
                        && task.Status != SharedData.TaskStatus.Disabled
                        && task.Address == _address))
                    {
                        if (task == null)
                            continue;

                        if (task is FileBackupTask)
                        {
                            quotaAddBytes = await FileBackup(task as FileBackupTask, filesForBackup, updatedTasks, 
                                filesForDelete, quotaAddBytes);
                        }
                        else if (task is SQLBackupTask)
                        {
                            quotaAddBytes = await SqlServerBackup(task as SQLBackupTask, filesForBackup, updatedTasks,
                                filesForDelete, quotaAddBytes);
                        }
                        else if (task is PGBackupTask)
                        {
                            quotaAddBytes = await PgSqlBackup(task as PGBackupTask, filesForBackup, updatedTasks,
                                filesForDelete, quotaAddBytes);
                        }
                    }

                    if (filesForBackup.Data.Count > 0)
                    {
                        //send backup files
                        await _client.SendAsync<SharedResponse>(new SharedRequest()
                        {
                            Command = "backup",
                            Data = filesForBackup.ToArray()
                        });
                    }

                    if (updatedTasks.Count > 0)
                    {
                        _tasks.UsedQuota += quotaAddBytes;
                        foreach (var task in updatedTasks)
                        {
                            _tasks.Data[task.Id] = task;
                        }
                        //send tasks list
                        await _client.SendAsync<SharedResponse>(new SharedRequest()
                        {
                            Command = "tasks",
                            Data = _tasks.ToArray()
                        });
                    }

                    if (filesForDelete.Count > 0)
                    {
                        foreach (var file in filesForDelete)
                        {
                            FileInfo fi = new FileInfo(file);
                            await fi.DeleteAsync();
                        }
                    }
                }
                catch { }
            }

            await Task.Delay(1000);
        }
    }
}

public static class FileExtensions
{
    public static Task DeleteAsync(this FileInfo fi)
    {
        return Task.Factory.StartNew(() => fi.Delete());
    }
}
#pragma warning restore CA1416