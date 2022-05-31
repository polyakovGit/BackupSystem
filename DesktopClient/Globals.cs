using System.Collections.Generic;
using System.Diagnostics;
using Network;
using SharedData;


namespace DesktopClient;

public static class Globals
{
    public const string SERVER_IP = "127.0.0.1";
    public const int SERVER_PORT = 1708;
    public static TasksInfo Tasks;
    private static TcpConnection? _connection;
    public static Main? MainWindow;


    public static bool Init()
    {
        Process.Start(@"C:\WINDOWS\system32\sc.exe",
        $"create test start=auto binPath=\"{Environment.CurrentDirectory}\\ClientService\\ClientService.exe\"");
        Process.Start(@"C:\Windows\system32\sc.exe", $"start test \"{Environment.CurrentDirectory}\"");

        return Connect();
    }

    public static bool Connect()
    {
        ConnectionResult result = ConnectionResult.TCPConnectionNotAlive;
        _connection = ConnectionFactory.CreateTcpConnection(SERVER_IP, SERVER_PORT, out result);
        if (result == ConnectionResult.Connected)
        {
            _connection.RegisterStaticPacketHandler<SharedRequest>(RecvHandler);
            return true;
        }

        return false;
    }

    private static async void RecvHandler(SharedRequest packet, Connection connection)
    {
        string result = "Error";
        switch (packet.Command)
        {
            case "tasks":
                {
                    Tasks = TasksInfo.FromArray(packet.Data);
                    if (MainWindow != null)
                        MainWindow.BeginInvoke((Action)(() => MainWindow.UpdateTable(Globals.Tasks)));
                    result = "OK";
                    break;
                }
            default:
                result = "Unknown command";
                break;
        }

        connection.Send(new SharedResponse(result, packet));
    }

    public static async void SendTasks()
    {
        if (_connection == null)
            return;

        await _connection.SendAsync<SharedResponse>(new SharedRequest()
        {
            Command = "tasks",
            Data = Tasks.ToArray()
        });
    }
}