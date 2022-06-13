using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SharedData;


namespace DesktopClient
{
    public partial class TaskFileEdit : Form
    {
        public TaskFileEdit()
        {
            InitializeComponent();
            comboBox1.SelectedIndex = 0;
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
                NextBackupTime = dateTimePicker1.Value,
                TypeTimeBackup = comboBox1.SelectedIndex,
                MaxCount = maxCount
            };

            return task;
        }

        public void SetTask(FileBackupTask task)
        {
            textBoxFilename.Text = task.FileName;
            dateTimePicker1.Value = task.NextBackupTime;
            comboBox1.SelectedIndex = task.TypeTimeBackup;
            textBoxCount.Text = task.MaxCount.ToString();
        }
    }
}
