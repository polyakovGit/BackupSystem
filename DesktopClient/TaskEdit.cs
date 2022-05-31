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
    public partial class TaskEdit : Form
    {
        public TaskEdit()
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

        public BackupTask GetTask()
        {
            var task = new BackupTask()
            {
                FileName = textBoxFilename.Text,
                NextBackupTime = dateTimePicker1.Value,
                TypeTimeBackup = comboBox1.SelectedIndex
            };

            return task;
        }

        public void SetTask(BackupTask task)
        {
            textBoxFilename.Text = task.FileName;
            dateTimePicker1.Value = task.NextBackupTime;
            comboBox1.SelectedIndex = task.TypeTimeBackup;
        }
    }
}
