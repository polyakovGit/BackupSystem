using Network;
using SharedData;


namespace ClientService;

public class Server
{
    private ServerConnectionContainer _server;

    public async Task Listen()
    {
        _server = ConnectionFactory.CreateServerConnectionContainer(1708, false);
        _server.AllowUDPConnections = false;
        _server.ConnectionEstablished += (conn, type) =>
        {
            conn.RegisterStaticPacketHandler<SharedClass>(HandlerCommand);
        };

        await _server.Start();
    }

    private static async void HandlerCommand(SharedClass packet, Connection connection)
    {
        var shared = new SharedClass();
        switch (packet.Command)
        {
            case "backup":
            {
                shared.Command = "result";
                
                Console.WriteLine("-> Get files for backup");
                
                var files = FilesInfo.FromBin(packet.Files);
                foreach (var file in files.Data)
                    await File.WriteAllBytesAsync($@"BackupFiles\{file.NameFile}", file.Bin);

                shared.Value = "OK";
                break;
            }
            default:
                Console.WriteLine($"-> Command not found! ({packet.Command})");
                break;
        }

        connection.Send(new SharedResponse(shared, packet));
    }

    protected internal void Disconnect() => _server.Stop();
}