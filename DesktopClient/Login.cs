using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DesktopClient
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void buttonConnection_Click(object sender, EventArgs e)
        {
            Globals.SERVER_IP = textBoxServerAddress.Text;
            Globals.SERVER_PORT = int.Parse(textBoxServerPort.Text);
            if (Globals.Init())
            {
                Globals.SendLogin(textBoxLogin.Text, textBoxPass.Text);
                //this.DialogResult = DialogResult.OK;
                //this.Close();
            }
            else if (Globals.connected)
            {
                Globals.SendLogin(textBoxLogin.Text, textBoxPass.Text);
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
