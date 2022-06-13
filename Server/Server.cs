using Network;
using SharedData;
using System.Collections.ObjectModel;
using System.Text;
using Newtonsoft.Json;

namespace ClientService;

public class Server
{
    private const int SERVER_PORT = 1708;
    private const string TASKS_FILENAME = "Tasks.json";
    private const string BACKUP_FOLDER = "BackupFiles";
    private ServerConnectionContainer _server;
    private TasksInfo _tasks;
    public static ObservableCollection<UserStruct> userDB { get; set; } = new ObservableCollection<UserStruct>();

    public Server()
    {
        LoadUsers();
        _tasks = TasksInfo.LoadFromFile(TASKS_FILENAME);
        _server = ConnectionFactory.CreateServerConnectionContainer(1708, false);
        _server.AllowUDPConnections = false;
        _server.ConnectionEstablished += (conn, type) =>
        {
            Console.WriteLine($"-> New connection");
            conn.TIMEOUT = 600000;
            conn.RegisterPacketHandler<SharedRequest>(HandlerCommand, this);
            conn.SendAsync<SharedResponse>(new SharedRequest()
            {
                Command = "tasks",
                Data = _tasks.ToArray()
            });
        };
    }

    void LoadUsers()
    {
        if (File.Exists("users.json"))
        {
            userDB = JsonConvert.DeserializeObject<ObservableCollection<UserStruct>>(File.ReadAllText("users.json"));

            Console.WriteLine($"Loaded {userDB.Count.ToString()} users");
        }
        else
        {
            UserStruct userStruct = new UserStruct() { Username = "root", Password = new Random().Next(99999, 999999).ToString() };
            userDB.Add(userStruct);
            Console.WriteLine($"No find user! Creating main user: {userStruct.Username}:{userStruct.Password}");
            File.WriteAllText("users.json", JsonConvert.SerializeObject(userDB));
        }
    }

    public async Task Listen()
    {
        await _server.Start();
    }

    private async void HandlerCommand(SharedRequest packet, Connection connection)
    {
        Console.WriteLine(packet.Command);
        var result = "Error";
        switch (packet.Command)
        {
            case "tasks":
                {
                    Console.WriteLine("-> Tasks updated");
                    _tasks = TasksInfo.FromArray(packet.Data);
                    await _tasks.SaveToFileAsync(TASKS_FILENAME);

                    var request = new SharedRequest()
                    {
                        Command = "tasks",
                        Data = _tasks.ToArray()
                    };
                    foreach (TcpConnection tcpConnection in _server.TCP_Connections)
                    {
                        if (tcpConnection != connection)
                        {
                            await tcpConnection.SendAsync<SharedResponse>(request);
                        }
                    }
                    Task.Run(() => CheckBackups());
                    result = "OK";
                    break;
                }
            case "backup":
                {
                    Console.WriteLine("-> Get files for backup");

                    var files = FilesInfo.FromBin(packet.Data);
                    foreach (var file in files.Data)
                    {
                        var dir = Path.Combine(BACKUP_FOLDER, file.Id.ToString(), file.Date.ToString("yyMMdd_HHmmss"));
                        await Task.Run(() => Directory.CreateDirectory(dir));
                        await File.WriteAllBytesAsync(Path.Combine(dir,Path.GetFileName(file.NameFile)), file.Bin);
                    }

                    result = "OK";
                    break;
                }

            case "Login":
                {
                    string[] logData = Encoding.UTF8.GetString(packet.Data).Split(new string[] { " &*&*& " }, StringSplitOptions.None);

                    string username = logData[0];
                    string password = logData[1];

                    lock (userDB)
                    {
                        var user = userDB.FirstOrDefault(x => x.Username == username && x.Password == password);

                        if (user != null)
                            SendLoginState(true, connection);
                        else
                            SendLoginState(false, connection);
                    }

                    break;
                }
            case "restore":
                {
                    int id = BitConverter.ToInt32(packet.Data, 0);
                    if (_tasks.Data.ContainsKey(id))
                    {
                        var task = _tasks.Data[id];
                        if (task.BackupTimes.Count != 0)
                        {
                            string filename = "";
                            if (task is FileBackupTask)
                            {
                                filename = (task as FileBackupTask).FileName;
                            }
                            else if (task is DbBackupTask)
                            {
                                filename = (task as DbBackupTask).DbName + ".bak";
                            }
                            else
                            {
                                break;
                            }

                            var fullPath = Path.Combine(BACKUP_FOLDER, 
                                id.ToString(), 
                                task.BackupTimes.Max().ToString("yyMMdd_HHmmss"), 
                                Path.GetFileName(filename));
                            if (File.Exists(fullPath))
                            {
                                Console.WriteLine($"->Restore task {id}\n");
                                var restoreFile = new FilesInfo();
                                restoreFile.Add(id, DateTime.MinValue, filename, await File.ReadAllBytesAsync(fullPath));
                                foreach (TcpConnection tcpConnection in _server.TCP_Connections)
                                {
                                    if (tcpConnection != connection)
                                    {
                                        await tcpConnection.SendAsync<SharedResponse>(new SharedRequest()
                                        {
                                            Command = "restore",
                                            Data = restoreFile.ToArray()
                                        });
                                    }
                                }

                                result = "OK";
                            }
                        }
                    }

                    
                    break;
                }
            default:
                Console.WriteLine($"-> Command not found! ({packet.Command})");
                break;
        }

        connection.Send(new SharedResponse(result, packet));
    }

    void CheckBackups()
    {
        bool tasksChanged = false;
        var dirInfo = new DirectoryInfo(BACKUP_FOLDER);
        foreach (var dir in dirInfo.GetDirectories())
        {
            int numDir = 0;
            if (!Int32.TryParse(dir.Name, out numDir) || !_tasks.Data.ContainsKey(numDir))
            {
                tasksChanged = true;
                Directory.Delete(dir.FullName, true);
            }
        }

        List<BackupTask> backupTasks = new List<BackupTask>(_tasks.Data.Values);
        foreach (var task in backupTasks)
        {
            if (task.BackupTimes.Count > task.MaxCount)
            {
                while (task.BackupTimes.Count > task.MaxCount)
                {
                    var olderDateTime = task.BackupTimes.Min();
                    var dir = Path.Combine(BACKUP_FOLDER, task.Id.ToString(), olderDateTime.ToString("yyMMdd_HHmmss"));
                    Directory.Delete(dir, true);
                    task.BackupTimes.Remove(olderDateTime);
                    tasksChanged = true;
                }

                _tasks.Data[task.Id] = task;
            }
        }
        if (tasksChanged)
        {
            _tasks.UsedQuota = GetFolderSize(dirInfo);
            _tasks.SaveToFile(TASKS_FILENAME);
            foreach (TcpConnection tcpConnection in _server.TCP_Connections)
            {
                tcpConnection.Send(new SharedRequest()
                {
                    Command = "tasks",
                    Data = _tasks.ToArray()
                });
            }
        }
    }

    long GetFolderSize(DirectoryInfo di)
    {
        long size = 0;
        // Добавляем размер файлов
        FileInfo[] files = di.GetFiles();
        foreach (FileInfo fi in files)
        {
            size += fi.Length;
        }
        // Добавляем размер подкаталогов
        DirectoryInfo[] dirs = di.GetDirectories();
        foreach (DirectoryInfo dir in dirs)
        {
            size += GetFolderSize(dir);
        }
        return size;
    }

    void SendLoginState(bool logged, Connection connection)
    {
        Console.WriteLine($"Login state...{logged.ToString()}");
        connection.SendAsync<SharedResponse>(new SharedRequest()
        {
            Command = "Login",
            Data = Encoding.UTF8.GetBytes(logged.ToString())
        });
    }

    protected internal void Disconnect() => _server.Stop();
}