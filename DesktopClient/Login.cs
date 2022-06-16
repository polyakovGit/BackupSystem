
namespace DesktopClient
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
            textBoxServerAddress.Text = Globals.Config.ServerIp;
            textBoxServerPort.Text = Globals.Config.ServerPort.ToString();
            textBoxLogin.Text = Globals.Config.Login;
            textBoxPass.Text = Globals.Config.Password;
        }

        private void buttonConnection_Click(object sender, EventArgs e)
        {
            Globals.Config.ServerIp = textBoxServerAddress.Text;
            int port = 0;
            int.TryParse(textBoxServerPort.Text, out port);
            Globals.Config.ServerPort = port;
            Globals.Config.Login = textBoxLogin.Text;
            Globals.Config.Password = textBoxPass.Text;
            Globals.SaveConfig();

            if (Globals.Init())
            {
                Globals.SendLogin(Globals.Config.Login, Globals.Config.Password);
            }
            else if (Globals.connected)
            {
                Globals.SendLogin(Globals.Config.Login, Globals.Config.Password);
            }
            else
            {
                MessageBox.Show("Не удалось соединиться с сервером");
            }
        }

        public bool isLogin = false;
        public void Clear()
        {
            textBoxLogin.Clear();
            textBoxPass.Clear();
        }
        private void Login_Load(object sender, EventArgs e)
        {
            Globals.Login = this;
        }

        private void Login_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!isLogin)
                Environment.Exit(0);
        }
    }
}
