using Network;
using SharedData;
using System.Collections.ObjectModel;
using System.Text;
using TTST;
using Newtonsoft.Json;

namespace ClientService;

public class Server
{
    private const int SERVER_PORT = 1708;
    private const string TASKS_FILENAME = "Tasks.json";
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
                    result = "OK";
                    break;
                }
            case "backup":
                {
                    Console.WriteLine("-> Get files for backup");

                    var files = FilesInfo.FromBin(packet.Data);
                    foreach (var file in files.Data)
                        await File.WriteAllBytesAsync($@"BackupFiles\{Path.GetFileName(file.NameFile)}", file.Bin);

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
                Console.WriteLine($"-> Command not found! ({packet.Command})");
                break;
        }

        connection.Send(new SharedResponse(result, packet));
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