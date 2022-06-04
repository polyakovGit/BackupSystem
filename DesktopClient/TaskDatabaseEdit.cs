using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SharedData;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.SqlServer.Management.Common;



namespace DesktopClient
{
    public partial class TaskDatabaseEdit : Form
    {
        Database dtb = Database.getInstance();
        public TaskDatabaseEdit()
        {
            InitializeComponent();
        }

        private void buttonConnect_Click(object sender, EventArgs e)
        {
            try
            {
                string server = comboBoxNameServer.Text;
                dtb.serverName = server;
                string nameUser = textBoxNameUser.Text;
                string pass = textBoxPass.Text;
                dtb.srvConn = new ServerConnection(server, nameUser, pass);
                dtb.srv = new Server(dtb.srvConn);
            }
            catch
            {
                dtb.connectionStatus=false;
                labelConnectionStatus.Text = "Соединение не установлено";
            }
            dtb.connectionStatus = true;
            labelConnectionStatus.Text = "Соединение установлено";
        }

        private void Settings_Load(object sender, EventArgs e)
        {
            if(!dtb.connectionStatus)
                labelConnectionStatus.Text = "Соединение не установлено";
            else labelConnectionStatus.Text = "Соединение установлено";
        }

        private void buttonDisconnect_Click(object sender, EventArgs e)
        {
            dtb.srv.ConnectionContext.Disconnect();
            dtb.connectionStatus = false;
            labelConnectionStatus.Text = "Соединение не установлено";
        }
    }
}
