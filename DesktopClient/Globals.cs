using System.Collections.Generic;
using System.Diagnostics;
using Network;
using SharedData;
using System.Text;


namespace DesktopClient;

public static class Globals
{
    public static string SERVER_IP;// = "127.0.0.1";
    public static int SERVER_PORT;// = 1708;
    public static TasksInfo Tasks;
    private static TcpConnection? _connection;
    public static Main? MainWindow;

    public static Login Login = null;
    public static bool connected = false;

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
            connected = true;
            _connection.RegisterStaticPacketHandler<SharedRequest>(RecvHandler);
            _connection.TIMEOUT = 60000;
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
                                MessageBox.Show("Имя пользователя или пароль неверен", "Ошибка входа", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

    public static async void SendRestore(int id)
    {
        if (_connection == null)
            return;

        await _connection.SendAsync<SharedResponse>(new SharedRequest()
        {
            Command = "restore",
            Data = BitConverter.GetBytes(id)
        });
    }
}