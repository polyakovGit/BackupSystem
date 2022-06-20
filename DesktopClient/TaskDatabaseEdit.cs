using System.Data;
using System.Data.SqlClient;
using SharedData;


namespace DesktopClient
{
    public partial class TaskDatabaseEdit : Form
    {
        public TaskDatabaseEdit()
        {
            InitializeComponent();
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
                Schedule = scheduleControl.GetSchedule(),
                MaxCount = maxCount
            };

            task.NextBackupTime = task.Schedule.GetFirstDateTime();

            return task;
        }

        public void SetTask(DbBackupTask task)
        {
            textBoxServerName.Text = task.Server;
            textBoxNameUser.Text = task.Login;
            textBoxPass.Text = task.Password;
            comboBoxDatabases.Text = task.DbName;
            scheduleControl.SetSchedule(task.Schedule);
            textBoxCount.Text = task.MaxCount.ToString();
        }
    }
}
