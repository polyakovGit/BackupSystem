using Network;
using SharedData;


namespace ClientService;

public class Server
{
    private const int SERVER_PORT = 1708;
    private const string TASKS_FILENAME = "Tasks.json";
    private ServerConnectionContainer _server;
    private TasksInfo _tasks;

    public Server()
    {
        _tasks = TasksInfo.LoadFromFile(TASKS_FILENAME);
        _server = ConnectionFactory.CreateServerConnectionContainer(1708, false);
        _server.AllowUDPConnections = false;
        _server.ConnectionEstablished += (conn, type) =>
        {
            Console.WriteLine($"-> New connection");
            conn.RegisterPacketHandler<SharedRequest>(HandlerCommand, this);
            conn.SendAsync<SharedResponse>( new SharedRequest()
                {
                    Command = "tasks",
                    Data = _tasks.ToArray()
                });
        };
    }

    public async Task Listen()
    {
        await _server.Start();
    }

    private async void HandlerCommand(SharedRequest packet, Connection connection)
    {
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
            default:
                Console.WriteLine($"-> Command not found! ({packet.Command})");
                break;
        }

        connection.Send(new SharedResponse(result, packet));
    }

    protected internal void Disconnect() => _server.Stop();
}