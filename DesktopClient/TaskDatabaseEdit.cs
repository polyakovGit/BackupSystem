using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using SharedData;


namespace DesktopClient
{
    public partial class TaskDatabaseEdit : Form
    {
        public TaskDatabaseEdit()
        {
            InitializeComponent();
            comboBoxSchedule.SelectedIndex = 0;
        }

        private void buttonConnect_Click(object sender, EventArgs e)
        {
            comboBoxDatabases.Items.Clear();

            try
            {
                SqlConnectionStringBuilder connStringBuilder = new SqlConnectionStringBuilder();
                connStringBuilder.DataSource = textBoxServerName.Text;
                connStringBuilder.UserID = textBoxNameUser.Text;
                connStringBuilder.Password = textBoxPass.Text;
                using (SqlConnection connection = new SqlConnection(connStringBuilder.ConnectionString))
                {
                    connection.Open();
                    DataTable dtDatabases = connection.GetSchema("databases");

                    foreach (DataRow item in dtDatabases.Rows)
                        comboBoxDatabases.Items.Add(item["database_name"]);

                    labelConnectionStatus.Text = "Соединение установлено";
                }
            }
            catch (Exception ex)
            {
                labelConnectionStatus.Text = "Соединение не установлено";
            }

            if (comboBoxDatabases.Items.Count > 0)
                comboBoxDatabases.SelectedIndex = 0;
        }

        public DbBackupTask GetTask()
        {
            int maxCount = 1;
            int.TryParse(textBoxCount.Text, out maxCount);
            if (maxCount <= 0)
                maxCount = 1;
            var task = new DbBackupTask()
            {
                Server = textBoxServerName.Text,
                Login = textBoxNameUser.Text,
                Password = textBoxPass.Text,
                DbName = comboBoxDatabases.Text,
                NextBackupTime = dateTimePicker1.Value,
                TypeTimeBackup = comboBoxSchedule.SelectedIndex,
                MaxCount = maxCount
            };

            return task;
        }

        public void SetTask(DbBackupTask task)
        {
            textBoxServerName.Text = task.Server;
            textBoxNameUser.Text = task.Login;
            textBoxPass.Text = task.Password;
            comboBoxDatabases.Text = task.DbName;
            dateTimePicker1.Value = task.NextBackupTime;
            comboBoxSchedule.SelectedIndex = task.TypeTimeBackup;
            textBoxCount.Text = task.MaxCount.ToString();
        }
    }
}
