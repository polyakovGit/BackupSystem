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
            lvi.Text = "Резервирование";
            lvi.Name = "Task";
            ListViewItem.ListViewSubItem subItem = new ListViewItem.ListViewSubItem(lvi, Globals.SERVER_IP);
            lvi.SubItems.Add(subItem);
            subItem = new ListViewItem.ListViewSubItem(lvi, task.GetStatusString());
            lvi.SubItems.Add(subItem);
            subItem = new ListViewItem.ListViewSubItem(lvi, task.GetScheduleString());
            lvi.SubItems.Add(subItem);
            listView1.Items.Add(lvi);
        }
    }

    private void buttonAdd_Click(object sender, EventArgs e)
    {
        var taskEditDlg = new TaskEdit();
        if (taskEditDlg.ShowDialog() != DialogResult.OK)
            return;
        var newTask = taskEditDlg.GetTask();
        if (string.IsNullOrEmpty(newTask.FileName))
            return;
        Globals.Tasks.Data[newTask.FileName] = newTask;
        Globals.SendTasks();
        UpdateTable(Globals.Tasks);
    }

    private void buttonEdit_Click(object sender, EventArgs e)
    {
        var taskEditDlg = new TaskEdit();
        if (listView1.SelectedItems.Count <= 0)
            return;

        BackupTask task = (BackupTask)listView1.SelectedItems[0].Tag;
        taskEditDlg.SetTask(task);
        if (taskEditDlg.ShowDialog() != DialogResult.OK)
            return;
        var newTask = taskEditDlg.GetTask();
        if (string.IsNullOrEmpty(newTask.FileName))
            Globals.Tasks.Data.Remove(task.FileName);
        if (newTask.FileName != task.FileName)
            Globals.Tasks.Data.Remove(task.FileName);
        task.NextBackupTime = newTask.NextBackupTime;
        task.TypeTimeBackup = newTask.TypeTimeBackup;
        Globals.Tasks.Data[task.FileName] = task;
        Globals.SendTasks();
        UpdateTable(Globals.Tasks);
    }
    private void buttonDelete_Click(object sender, EventArgs e)
    {
        if (listView1.SelectedItems.Count == 1)
        {
            BackupTask task = (BackupTask)listView1.SelectedItems[0].Tag;
            Globals.Tasks.Data.Remove(task.FileName);
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
        }
        else
        {
            buttonEdit.Enabled = false;
            buttonDelete.Enabled = false;
        }
    }

    private void buttonSettings_Click(object sender, EventArgs e)
    {
        var settingsEditDlg = new Settings();
        if (settingsEditDlg.ShowDialog() != DialogResult.OK)
            return;
    }
}