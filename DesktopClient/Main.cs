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
        foreach (var task in tasks.Data.Values.OrderBy(t => t.Id))
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
            subItem = new ListViewItem.ListViewSubItem(lvi, Globals.Config.ServerIp);
            lvi.SubItems.Add(subItem);
            subItem = new ListViewItem.ListViewSubItem(lvi, task.GetStatusString());
            lvi.SubItems.Add(subItem);
            subItem = new ListViewItem.ListViewSubItem(lvi, task.GetScheduleString());
            lvi.SubItems.Add(subItem);
            listView1.Items.Add(lvi);
        }

        progressBarQuota.Value = tasks.UsedQuota < tasks.MaxQuota 
            ? (int)(tasks.UsedQuota * 100 / tasks.MaxQuota)
            : 100;
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
        newTask.AddAction(TaskAction.Created);
        Globals.Tasks.Data[newTask.Id] = newTask;
        Globals.SendTasks();
        UpdateTable(Globals.Tasks);
    }

    private void buttonEdit_Click(object sender, EventArgs e)
    {
        if (listView1.SelectedItems.Count <= 0)
            return;

        BackupTask task = (BackupTask)listView1.SelectedItems[0].Tag;
        if (task == null)
            return;
        if (task is FileBackupTask)
        {
            var taskEditDlg = new TaskFileEdit();
            taskEditDlg.SetTask(task as FileBackupTask);
            if (taskEditDlg.ShowDialog() != DialogResult.OK)
                return;
            var newTask = taskEditDlg.GetTask();
            task.NextBackupTime = newTask.NextBackupTime;
            task.TypeTimeBackup = newTask.TypeTimeBackup;
            task.MaxCount = newTask.MaxCount;
            (task as FileBackupTask).FileName = newTask.FileName;
            Globals.Tasks.Data[task.Id] = task;
            Globals.SendTasks();
            UpdateTable(Globals.Tasks);
        }
        else if (task is DbBackupTask)
        {
            var dbTask = task as DbBackupTask;
            var taskEditDlg = new TaskDatabaseEdit();
            taskEditDlg.SetTask(dbTask);
            if (taskEditDlg.ShowDialog() != DialogResult.OK)
                return;
            var newTask = taskEditDlg.GetTask();
            dbTask.NextBackupTime = newTask.NextBackupTime;
            dbTask.TypeTimeBackup = newTask.TypeTimeBackup;
            dbTask.MaxCount = newTask.MaxCount;
            dbTask.Server = newTask.Server;
            dbTask.Login = newTask.Login;
            dbTask.Password = newTask.Password;
            dbTask.DbName = newTask.DbName;
            Globals.Tasks.Data[dbTask.Id] = dbTask;
            Globals.SendTasks();
            UpdateTable(Globals.Tasks);
        }
    }
    private void buttonDelete_Click(object sender, EventArgs e)
    {
        if (listView1.SelectedItems.Count != 1)
            return;
        BackupTask task = (BackupTask)listView1.SelectedItems[0].Tag;
        if (task == null)
            return;
        Globals.Tasks.Data.Remove(task.Id);
        Globals.SendTasks();
        UpdateTable(Globals.Tasks);
        listView1_SelectedIndexChanged(this, EventArgs.Empty);
    }

    private void listView1_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (listView1.SelectedIndices.Count == 1)
        {
            buttonEdit.Enabled = true;
            buttonDelete.Enabled = true;
            buttonRestore.Enabled = true;
            buttonDisable.Enabled = true;
            BackupTask task = (BackupTask)listView1.SelectedItems[0].Tag;
            if (task != null)
            {
                buttonDisable.Text = task.Status == SharedData.TaskStatus.Disabled ? "Включить" : "Отключить";
            }
            buttonHistory.Enabled = true;
        }
        else
        {
            buttonEdit.Enabled = false;
            buttonDelete.Enabled = false;
            buttonRestore.Enabled = false;
            buttonDisable.Enabled = false;
            buttonHistory.Enabled = false;
        }
    }

    private void Main_Load(object sender, EventArgs e)
    {
        Login s = new Login();
        s.ShowDialog();
    }

    private void buttonAddDb_Click(object sender, EventArgs e)
    {
        var taskEditDlg = new TaskDatabaseEdit();
        if (taskEditDlg.ShowDialog() != DialogResult.OK)
            return;
        var newTask = taskEditDlg.GetTask();
        newTask.Id = Globals.Tasks.GetNextId();
        newTask.AddAction(TaskAction.Created);
        Globals.Tasks.Data[newTask.Id] = newTask;
        Globals.SendTasks();
        UpdateTable(Globals.Tasks);
    }

    private void buttonRestore_Click(object sender, EventArgs e)
    {
        if (listView1.SelectedItems.Count == 1)
        {
            BackupTask task = (BackupTask)listView1.SelectedItems[0].Tag;
            if (task == null)
                return;
            Globals.SendRestore(task.Id);
        }
    }

    private void buttonDisable_Click(object sender, EventArgs e)
    {
        if (listView1.SelectedItems.Count != 1)
            return;
        BackupTask task = (BackupTask)listView1.SelectedItems[0].Tag;
        if (task == null)
            return;
        if (task.Status == SharedData.TaskStatus.Disabled)
        {
            task.Status = task.BackupTimes.Count == 0 ? SharedData.TaskStatus.New : SharedData.TaskStatus.Working;
        }
        else
        {
            task.Status = SharedData.TaskStatus.Disabled;
        }
        Globals.Tasks.Data[task.Id] = task;
        Globals.SendTasks();
        UpdateTable(Globals.Tasks);
        listView1_SelectedIndexChanged(this, EventArgs.Empty);
    }

    private void buttonQuota_Click(object sender, EventArgs e)
    {
        var quotaDlg = new Quota();
        if (quotaDlg.ShowDialog() != DialogResult.OK)
            return;

        long newQuota = 0;
        if (long.TryParse(quotaDlg.textBoxMax.Text, out newQuota))
        {
            Globals.Tasks.MaxQuota = newQuota * 1024;
            Globals.SendTasks();
            UpdateTable(Globals.Tasks);
        }
    }

    private void buttonHistory_Click(object sender, EventArgs e)
    {
        if (listView1.SelectedItems.Count <= 0)
            return;
        BackupTask task = (BackupTask)listView1.SelectedItems[0].Tag;
        if (task == null)
            return;

        var historyDlg = new TaskHistory();
        historyDlg.UpdateHistoty(task);
        historyDlg.ShowDialog();
    }
}