using System.Data;
using Npgsql;
using SharedData;


namespace DesktopClient
{
    public partial class TaskPgSqlEdit : Form
    {
        public TaskPgSqlEdit()
        {
            InitializeComponent();
            comboBoxSchedule.SelectedIndex = 0;
        }

        private void buttonConnect_Click(object sender, EventArgs e)
        {
            comboBoxDatabases.Items.Clear();

            try
            {
                NpgsqlConnectionStringBuilder connStringBuilder = new NpgsqlConnectionStringBuilder();
                connStringBuilder.Host = textBoxHost.Text;
                int port = 0;
                int.TryParse(textBoxPort.Text, out port);
                connStringBuilder.Port = port;
                connStringBuilder.Username = textBoxUserId.Text;
                connStringBuilder.Password = textBoxPass.Text;
                using (NpgsqlConnection connection = new NpgsqlConnection(connStringBuilder.ConnectionString))
                {
                 
                    connection.Open();

                    NpgsqlDataAdapter adapter = new NpgsqlDataAdapter("SELECT datname FROM pg_database WHERE datistemplate = false;", connection);
                    DataSet dataSet = new DataSet();
                    adapter.Fill(dataSet);
                    foreach(DataRow row in dataSet.Tables[0].Rows)
                        comboBoxDatabases.Items.Add(row[0].ToString());

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

        public PgSqlBackupTask GetTask()
        {
            int port = 0;
            int.TryParse(textBoxPort.Text, out port);
            int maxCount = 1;
            int.TryParse(textBoxCount.Text, out maxCount);
            if (maxCount <= 0)
                maxCount = 1;
            var task = new PgSqlBackupTask()
            {
                Host = textBoxHost.Text,
                Port = port,
                UserId = textBoxUserId.Text,
                Password = textBoxPass.Text,
                DbName = comboBoxDatabases.Text,
                NextBackupTime = dateTimePicker1.Value,
                TypeTimeBackup = comboBoxSchedule.SelectedIndex,
                MaxCount = maxCount
            };

            return task;
        }

        public void SetTask(PgSqlBackupTask task)
        {
            textBoxHost.Text = task.Host;
            textBoxPort.Text = task.Port.ToString();
            textBoxUserId.Text = task.UserId;
            textBoxPass.Text = task.Password;
            comboBoxDatabases.Text = task.DbName;
            dateTimePicker1.Value = task.NextBackupTime;
            comboBoxSchedule.SelectedIndex = task.TypeTimeBackup;
            textBoxCount.Text = task.MaxCount.ToString();
        }
    }
}
