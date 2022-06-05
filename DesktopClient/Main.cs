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

namespace DesktopClient;

public partial class Main : Form
{

    public Main()
    {
        InitializeComponent();
    }

    public void UpdateTable(TasksInfo tasks)
    {
        listView1.Items.Clear();
        foreach (var task in tasks.Data.Values)
        {
            ListViewItem lvi = new ListViewItem();
            lvi.Tag = task;
            lvi.Text = task.Id.ToString();
            lvi.Name = "Task";
            string taskType;
            if (task is FileBackupTask)
                taskType = "Файл";
            else if (task is DbBackupTask)
                taskType = "База данных";
            else
                taskType = "Неизвестно";
            ListViewItem.ListViewSubItem subItem = new ListViewItem.ListViewSubItem(lvi, taskType);
            lvi.SubItems.Add(subItem);
            subItem = new ListViewItem.ListViewSubItem(lvi, Globals.SERVER_IP);
            lvi.SubItems.Add(subItem);
            subItem = new ListViewItem.ListViewSubItem(lvi, task.GetStatusString());
            lvi.SubItems.Add(subItem);
            subItem = new ListViewItem.ListViewSubItem(lvi, task.GetScheduleString());
            lvi.SubItems.Add(subItem);
            listView1.Items.Add(lvi);
        }
    }

    private void buttonAddFile_Click(object sender, EventArgs e)
    {
        var taskEditDlg = new TaskFileEdit();
        if (taskEditDlg.ShowDialog() != DialogResult.OK)
            return;
        var newTask = taskEditDlg.GetTask();
        if (string.IsNullOrEmpty(newTask.FileName))
            return;
        newTask.Id = Globals.Tasks.GetNextId();
        Globals.Tasks.Data[newTask.Id] = newTask;
        Globals.SendTasks();
        UpdateTable(Globals.Tasks);
    }
    private void buttonAddFile_Click_1(object sender, EventArgs e)
    {
        var taskEditDlg = new TaskFileEdit();
        if (taskEditDlg.ShowDialog() != DialogResult.OK)
            return;
        var newTask = taskEditDlg.GetTask();
        if (string.IsNullOrEmpty(newTask.FileName))
            return;
        newTask.Id = Globals.Tasks.GetNextId();
        Globals.Tasks.Data[newTask.Id] = newTask;
        Globals.SendTasks();
        UpdateTable(Globals.Tasks);
    }
    private void buttonEdit_Click(object sender, EventArgs e)
    {
        var taskEditDlg = new TaskFileEdit();
        if (listView1.SelectedItems.Count <= 0)
            return;

        BackupTask task = (BackupTask)listView1.SelectedItems[0].Tag;
        if (task is FileBackupTask)
        {
            taskEditDlg.SetTask(task as FileBackupTask);
            if (taskEditDlg.ShowDialog() != DialogResult.OK)
                return;
            var newTask = taskEditDlg.GetTask();
            task.NextBackupTime = newTask.NextBackupTime;
            task.TypeTimeBackup = newTask.TypeTimeBackup;
            (task as FileBackupTask).FileName = newTask.FileName;
            Globals.Tasks.Data[task.Id] = task;
            Globals.SendTasks();
            UpdateTable(Globals.Tasks);
        }
        else if (task is DbBackupTask)
        {
            //TODO:
        }
        else
        {
            //error task
        }
    }
    private void buttonDelete_Click(object sender, EventArgs e)
    {
        if (listView1.SelectedItems.Count == 1)
        {
            BackupTask task = (BackupTask)listView1.SelectedItems[0].Tag;
            Globals.Tasks.Data.Remove(task.Id);
            Globals.SendTasks();
            UpdateTable(Globals.Tasks);
        }
    }

    private void listView1_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (listView1.SelectedIndices.Count == 1)
        {
            buttonEdit.Enabled = true;
            buttonDelete.Enabled = true;
            buttonRestore.Enabled = true;
        }
        else
        {
            buttonEdit.Enabled = false;
            buttonDelete.Enabled = false;
            buttonRestore.Enabled = false;
        }
    }

    private void Main_Load(object sender, EventArgs e)
    {
        Login s = new Login();
        s.ShowDialog();
    }

}