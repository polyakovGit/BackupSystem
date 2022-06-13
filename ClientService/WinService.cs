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
                        if (task is FileBackupTask)
                        {
                            await Task.Run(() => Directory.CreateDirectory(Path.GetDirectoryName(file.NameFile)));
                            await File.WriteAllBytesAsync(file.NameFile, file.Bin);
                            result = "OK";
                        }
                        else if (task is (DbBackupTask))
                        {
                            var dbTask = task as DbBackupTask;
                            string fullPath = Path.Combine(Path.GetTempPath(), file.NameFile);
                            await File.WriteAllBytesAsync(fullPath, file.Bin);

                            try
                            {
                                SqlConnectionStringBuilder connStringBuilder = new SqlConnectionStringBuilder();
                                connStringBuilder.DataSource = dbTask.Server;
                                connStringBuilder.UserID = dbTask.Login;
                                connStringBuilder.Password = dbTask.Password;
                                using (SqlConnection conn = new SqlConnection(connStringBuilder.ConnectionString))
                                {
                                    await conn.OpenAsync();
                                    string query = @"RESTORE DATABASE @databaseName 
                                        FROM DISK = @localDatabasePath 
                                        WITH REPLACE";
                                    var sqlCommand = new SqlCommand(query, conn);
                                    sqlCommand.Parameters.AddWithValue("@databaseName", dbTask.DbName);
                                    sqlCommand.Parameters.AddWithValue("@localDatabasePath", fullPath);
                                    await sqlCommand.ExecuteNonQueryAsync();
                                }
                                result = "OK";
                            }
                            catch (Exception ex)
                            {
                                await File.AppendAllTextAsync(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log.txt"), ex.Message + "\n");
                            }   
                            
                            FileInfo fi = new FileInfo(fullPath);
                            await fi.DeleteAsync();
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
                        && task.Status != SharedData.TaskStatus.Disabled))
                    {
                        if (task == null)
                            continue;

                        if (task is FileBackupTask)
                        {
                            FileBackupTask fileTask = task as FileBackupTask;
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
                        }
                        else //DbBackupTask
                        {
                            DbBackupTask dbTask = task as DbBackupTask;
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
                                    DbBackupTask updatedTask = dbTask;
                                    updatedTask.Status = SharedData.TaskStatus.Working;
                                    updatedTask.AddAction(TaskAction.Backup);
                                    updatedTask.UpdateNextBackupTime();
                                    updatedTask.BackupTimes.Add(backupTime);
                                    updatedTasks.Add(updatedTask);
                                }
                                else
                                {
                                    DbBackupTask updatedTask = dbTask;
                                    updatedTask.Status = SharedData.TaskStatus.Error_Quota;
                                    updatedTask.UpdateNextBackupTime();
                                    updatedTasks.Add(updatedTask);
                                }
                            }
                            catch (Exception ex)
                            {
                                await File.AppendAllTextAsync(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log.txt"), ex.Message + "\n");
                                DbBackupTask updatedTask = dbTask;
                                updatedTask.Status = SharedData.TaskStatus.Error_DbConnect;
                                updatedTask.UpdateNextBackupTime();
                                updatedTasks.Add(updatedTask);
                            }
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