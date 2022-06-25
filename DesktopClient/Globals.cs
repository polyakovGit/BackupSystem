using Network;
using System.Text;
using ClientConfig;
using SharedData;



namespace DesktopClient;

public static class Globals
{
    public static Config Config = new Config();
    private static string _configFilename = Path.Combine(Environment.CurrentDirectory, "Config.json");
    public static TasksInfo Tasks;
    private static TcpConnection? _connection;
    public static Main? MainWindow;
    public static string IpAddress = "";
    public static int Port = 0;

    public static Login Login = null;
    public static bool connected = false;

    public static bool Init()
    {
        return Connect();
    }

    public static bool Connect()
    {
        ConnectionResult result = ConnectionResult.TCPConnectionNotAlive;
        _connection = ConnectionFactory.CreateTcpConnection(Config.ServerIp, Config.ServerPort, out result);
        if (result == ConnectionResult.Connected)
        {
            connected = true;
            _connection.RegisterStaticPacketHandler<SharedRequest>(RecvHandler);
            _connection.TIMEOUT = 600000;
            IpAddress = _connection.IPLocalEndPoint.Address.MapToIPv4().ToString();
            Port = _connection.IPLocalEndPoint.Port;
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
            case "Login":
                {
                    bool isLogged = Convert.ToBoolean(Encoding.UTF8.GetString(packet.Data));

                    try
                    {
                        if (isLogged)
                        {
                            Login.Invoke(new Action(() =>
                            {
                                Login.isLogin = true;
                                Login.Close();
                            }));
                        }
                        else
                        {
                            Login.Invoke(new Action(() =>
                            {
                                MessageBox.Show("Login of password invalid!", "Login Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                Login.Clear();
                            }));
                        }
                    }
                    catch { }
                    break;
                }
            default:
                result = "Unknown command";
                break;
        }

        connection.Send(new SharedResponse(result, packet));
    }
    public static async void SendLogin(string username, string password)
    {
        try
        {
            await _connection.SendAsync<SharedResponse>(new SharedRequest()
            {
                Command = "Login",
                Data = Encoding.UTF8.GetBytes($"{username} &*&*& {password}")
            });
        }
        catch { }
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

    public static async void SendRestore(RestoreTask restoreTask)
    {
        if (_connection == null)
            return;

        await _connection.SendAsync<SharedResponse>(new SharedRequest()
        {
            Command = "restore",
            Data = restoreTask.ToArray()
        });
    }

    public static void LoadConfig()
    {
        Config = Config.LoadFromFile(_configFilename);
    }

    public static void SaveConfig()
    {
        Config.SaveToFile(_configFilename);
    }
}