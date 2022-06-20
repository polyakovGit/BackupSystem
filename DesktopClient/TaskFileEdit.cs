using SharedData;


namespace DesktopClient
{
    public partial class TaskFileEdit : Form
    {
        public TaskFileEdit()
        {
            InitializeComponent();
        }

        private void buttonSelect_Click(object sender, EventArgs e)
        {
            var dialog = new OpenFileDialog();
            if (!string.IsNullOrEmpty(textBoxFilename.Text))
            {
                dialog.InitialDirectory = !string.IsNullOrEmpty(textBoxFilename.Text) 
                    ? Path.GetDirectoryName(textBoxFilename.Text) 
                    : Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            }
            if (dialog.ShowDialog() != DialogResult.OK)
                return;

            textBoxFilename.Text = dialog.FileName;
        }

        public FileBackupTask GetTask()
        {
            int maxCount = 1;
            int.TryParse(textBoxCount.Text, out maxCount);
            if (maxCount <= 0)
                maxCount = 1;
            var task = new FileBackupTask()
            {
                FileName = textBoxFilename.Text,
                Schedule = scheduleControl.GetSchedule(),
                MaxCount = maxCount
            };

            task.NextBackupTime = task.Schedule.GetFirstDateTime();

            return task;
        }

        public void SetTask(FileBackupTask task)
        {
            textBoxFilename.Text = task.FileName;
            scheduleControl.SetSchedule(task.Schedule);
            textBoxCount.Text = task.MaxCount.ToString();
        }
    }
}
