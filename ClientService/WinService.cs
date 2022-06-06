﻿using System;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.ServiceProcess;
using System.Threading;
using Network;
using SharedData;

namespace ClientService;

#pragma warning disable CA1416
public class WinService : ServiceBase
{
    private const string SERVICE_NAME = "ClientService";
    private const string SERVER_IP = "127.0.0.1";
    private const int SERVER_PORT = 1708;
    private string _homePath;
    private bool _isWork = false;
    private TasksInfo _tasks;
    private TcpConnection? _client;

    public WinService()
    {
        this.ServiceName = SERVICE_NAME;
        this.CanStop = true;
        this.CanPauseAndContinue = false;
        this.AutoLog = false;

        _homePath = string.Empty;
        _tasks = new TasksInfo();
        _client = null;
    }

    protected override async void OnStart(string[] args)
    {
        if (args.Length == 0)
            Stop();

        await Connect();
        _homePath = args[0];
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
        var result = await ConnectionFactory.CreateTcpConnectionAsync(SERVER_IP, SERVER_PORT);
        if (result.Item2 == ConnectionResult.Connected)
        {
            _client = result.Item1;
            _client.RegisterPacketHandler<SharedRequest>(RecvHandler, this);
            _client.TIMEOUT = 60000;
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
                            string fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, file.NameFile);
                            await File.WriteAllBytesAsync(fullPath, file.Bin);

                            try
                            {
                                string connString = $"Data Source = {dbTask.Server}; User ID = {dbTask.Login}; Password = {dbTask.Password}";
                                using (SqlConnection conn = new SqlConnection(connString))
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
                            catch { }   
                            
                            FileInfo fi = new FileInfo(fullPath);
                            fi.DeleteAsync();
                        }
                    }
                    break;
                }
            default:
                result = "Unknown command";
                break;
        }

        connection.Send(new SharedResponse(result, packet));
    }

    private async Task Handler()
    {
        while (_isWork)
        {
            //Если нет подключения, то пытаемся подключиться к серверу
            if (_client == null)
            {
                await Connect();
            }
            else 
            {
                try
                {
                    var filesForBackup = new FilesInfo();
                    var updatedTasks = new List<BackupTask>();
                    foreach (var task in _tasks.Data.Values.Where(task => task.NextBackupTime <= DateTime.Now))
                    {
                        if (task is FileBackupTask)
                        {
                            FileBackupTask fileTask = task as FileBackupTask;
                            if (File.Exists(fileTask.FileName))
                            {
                                filesForBackup.Add(fileTask.Id, fileTask.FileName, await File.ReadAllBytesAsync(fileTask.FileName));
                                FileBackupTask updatedTask = fileTask;
                                updatedTask.Status = SharedData.TaskStatus.Working;
                                updatedTask.UpdateNextBackupTime();
                                updatedTask.LastBackupTime = DateTime.Now;
                                updatedTasks.Add(updatedTask);
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
                            string fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
                            try
                            {


                                string connString = $"Data Source = {dbTask.Server}; User ID = {dbTask.Login}; Password = {dbTask.Password}";
                                using (SqlConnection connection = new SqlConnection(connString))
                                {
                                    await connection.OpenAsync();
                                    var formatMediaName = $"DatabaseToolkitBackup_{dbTask.DbName}";
                                    var formatName = $"Full Backup of {dbTask.DbName}";
                                    string query = @"BACKUP DATABASE @databaseName TO DISK = @localDatabasePath WITH FORMAT, MEDIANAME = @formatMediaName, NAME = @formatName";
                                    var sqlCommand = new SqlCommand(query, connection);
                                    sqlCommand.Parameters.AddWithValue("@databaseName", dbTask.DbName);
                                    sqlCommand.Parameters.AddWithValue("@localDatabasePath", fullPath);
                                    sqlCommand.Parameters.AddWithValue("@formatMediaName", formatMediaName);
                                    sqlCommand.Parameters.AddWithValue("@formatName", formatName);
                                    await sqlCommand.ExecuteNonQueryAsync();
                                }

                                filesForBackup.Add(dbTask.Id, fullPath, await File.ReadAllBytesAsync(fullPath));
                                DbBackupTask updatedTask = dbTask;
                                updatedTask.Status = SharedData.TaskStatus.Working;
                                updatedTask.UpdateNextBackupTime();
                                updatedTask.LastBackupTime = DateTime.Now;
                                updatedTasks.Add(updatedTask);
                            }
                            catch (Exception ex)
                            {
                                File.AppendAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log.txt"), ex.Message);
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