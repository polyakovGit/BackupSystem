using Network;
using SharedData;
using System.IO;
using System.Collections.ObjectModel;
using System.Text;
using TTST;
using Newtonsoft.Json;
using RIS.Reflection.Mapping;
using RIS.Unions.Types;

namespace ServerService
{
    public class Server : BackgroundService
    {

        private ServerConnectionContainer _server;
        private const string TASKS_FILENAME = "Tasks.json";
        private TasksInfo _tasks;
        private readonly MethodMap<Server> _commandMap;

        string exePath = AppDomain.CurrentDomain.BaseDirectory;
        public static ObservableCollection<UserStruct> userDB { get; set; } = new ObservableCollection<UserStruct>();

        public Server()
        {
            _commandMap = new MethodMap<Server>(
                this,
                new[]
                {
                    typeof(SharedRequest),
                    typeof(Connection)
                },
                typeof(Task<CommandResult>));
        }

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
        void LoadUsers()
        {
            if (File.Exists("users.json"))
            {
                userDB = JsonConvert.DeserializeObject<ObservableCollection<UserStruct>>(File.ReadAllText("users.json"));

                File.AppendAllText(Path.Combine(exePath, "log.txt"), $"Loaded {userDB.Count.ToString()} users\n");
            }
            else
            {
                UserStruct userStruct = new UserStruct() { Username = "root", Password = new Random().Next(99999, 999999).ToString() };
                userDB.Add(userStruct);
                File.AppendAllTextAsync(Path.Combine(exePath, "log.txt"), $"No find user! Creating main user: {userStruct.Username}:{userStruct.Password}\n");
                File.AppendAllText(Path.Combine(exePath, "users.json"), JsonConvert.SerializeObject(userDB));
            }
        }
        private async void HandlerCommand(SharedRequest packet, Connection connection)
        {
            CommandResult result = new Error();

            if (_commandMap.Mappings.ContainsKey(packet.Command))
            {
                result = await _commandMap.Invoke<Task<CommandResult>>(
                    packet.Command,
                    packet, connection);
            }
            else
            {
                await File.AppendAllTextAsync(
                    Path.Combine(exePath, "log.txt"),
                    $"-> Command not found! ({packet.Command})\n");
            }

            connection.Send(new SharedResponse(
                result.Match(
                    _ => "OK",
                    _ => "Error"),
                packet));
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



        [MappedMethod("tasks")]
        public async Task<CommandResult> TasksCommand(SharedRequest packet, Connection connection)
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

            return new Success();
        }

        [MappedMethod("backup")]
        public async Task<CommandResult> BackupCommand(SharedRequest packet, Connection connection)
        {
            await File.AppendAllTextAsync(Path.Combine(exePath, "log.txt"), "->Get files for backup\n");

            var files = FilesInfo.FromBin(packet.Data);
            foreach (var file in files.Data)
                await File.WriteAllBytesAsync(Path.Combine(exePath, $@"BackupFiles\{Path.GetFileName(file.NameFile)}"), file.Bin);

            return new Success();
        }

        [MappedMethod("Login")]
        public Task<CommandResult> LoginCommand(SharedRequest packet, Connection connection)
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

            return Task.FromResult<CommandResult>(new Success());
        }
    }
}