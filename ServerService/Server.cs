using Network;
using SharedData;
using System.IO;
using System.Collections.ObjectModel;
using System.Text;
using Newtonsoft.Json;

namespace ServerService
{
    public class Server : BackgroundService
    {

        private ServerConnectionContainer _server;
        private const string TASKS_FILENAME = "Tasks.json";
        private const string BACKUP_FOLDER = "BackupFiles";
        private TasksInfo _tasks;

        string exePath = AppDomain.CurrentDomain.BaseDirectory;
        public static ObservableCollection<UserStruct> userDB { get; set; } = new ObservableCollection<UserStruct>();

        public async Task Listen()
        {
            LoadUsers();
            _server = ConnectionFactory.CreateServerConnectionContainer(1708, false);
            _server.AllowUDPConnections = false;
            _server.ConnectionEstablished += (conn, type) =>
            {
                File.AppendAllText(Path.Combine(exePath, "log.txt"), $"-> New connection\n");
                conn.RegisterPacketHandler<SharedRequest>(HandlerCommand, this);
                conn.SendAsync<SharedResponse>(new SharedRequest()
                {
                    Command = "tasks",
                    Data = _tasks.ToArray()
                });
            };

            await _server.Start();
        }
        async void LoadUsers()
        {
            if (File.Exists(Path.Combine(exePath, "users.json")))
            {
                userDB = JsonConvert.DeserializeObject<ObservableCollection<UserStruct>>(File.ReadAllText(Path.Combine(exePath, "users.json")));

                File.AppendAllText(Path.Combine(exePath, "log.txt"), $"Loaded {userDB.Count.ToString()} users\n");
            }
            else
            {
                UserStruct userStruct = new UserStruct() { Username = "root", Password = new Random().Next(99999, 999999).ToString() };
                userDB.Add(userStruct);
                File.AppendAllText(Path.Combine(exePath, "log.txt"), $"No find user! Creating main user: {userStruct.Username}:{userStruct.Password}\n");
                File.AppendAllText(Path.Combine(exePath, "users.json"), JsonConvert.SerializeObject(userDB));
            }
        }
        private async void HandlerCommand(SharedRequest packet, Connection connection)
        {

            var result = "Error";
            switch (packet.Command)
            {
                case "tasks":
                    {
                        await File.AppendAllTextAsync(Path.Combine(exePath, "log.txt"), "->Tasks updated\n");
                        _tasks = TasksInfo.FromArray(packet.Data);
                        _tasks.SaveToFile(TASKS_FILENAME);
                        //TODO: send to other and locks
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
                        await File.AppendAllTextAsync(Path.Combine(exePath, "log.txt"), "->Get files for backup\n");

                        var files = FilesInfo.FromBin(packet.Data);
                        foreach (var file in files.Data)
                        {
                            await Task.Run(() => Directory.CreateDirectory($@"{BACKUP_FOLDER}\{file.Id}\"));
                            await File.WriteAllBytesAsync(Path.Combine(exePath, $@"{BACKUP_FOLDER}\{file.Id}\{Path.GetFileName(file.NameFile)}"), file.Bin);
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
                default:
                    await File.AppendAllTextAsync(Path.Combine(exePath, "log.txt"), $"-> Command not found! ({packet.Command})\n");
                    break;
            }

            connection.Send(new SharedResponse(result, packet));
        }

        void CheckBackups()
        {
            foreach (var dir in Directory.EnumerateDirectories(BACKUP_FOLDER))
            {
                int numDir = 0;
                if (!Int32.TryParse(dir, out numDir) || !_tasks.Data.ContainsKey(numDir))
                {
                    Directory.Delete($@"{BACKUP_FOLDER}/{dir}", true);
                }
            }
        }

        void SendLoginState(bool logged, Connection connection)
        {
            File.AppendAllTextAsync(Path.Combine(exePath, "log.txt"), $"Login state...{logged.ToString()}\n");
            connection.SendAsync<SharedResponse>(new SharedRequest()
            {
                Command = "Login",
                Data = Encoding.UTF8.GetBytes(logged.ToString())
            });
        }
        protected internal void Disconnect() => _server.Stop();
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _tasks = TasksInfo.LoadFromFile(TASKS_FILENAME);
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await Listen();
                }
                catch (Exception ex)
                {
                    // обработка ошибки однократного неуспешного выполнения фоновой задачи
                }

                await Task.Delay(1000);
            }
        }
    }
}