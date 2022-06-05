using System;
using System.CodeDom;
using System.ComponentModel;
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

            await Task.Delay(1000);
        }
    }
}
#pragma warning restore CA1416