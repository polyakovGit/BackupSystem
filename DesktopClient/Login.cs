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
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else MessageBox.Show("Не удалось соединиться с сервером");
        }
    }
}
