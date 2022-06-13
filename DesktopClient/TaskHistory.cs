using SharedData;

namespace DesktopClient
{
    public partial class TaskHistory : Form
    {
        public TaskHistory()
        {
            InitializeComponent();
        }

        public void UpdateHistoty(BackupTask task)
        {
            listView.Items.Clear();
            foreach (var history in task.History)
            {
                ListViewItem lvi = new ListViewItem();
                lvi.Tag = history;
                lvi.Text = history.Date.ToString();
                lvi.Name = "History";
                string action = history.Action switch
                {
                    TaskAction.Created => "Задание создано",
                    TaskAction.Backup => "Выполнено резервирование",
                    TaskAction.Restore => "Выполнено восстановление",
                    _ => "-"
                };
                ListViewItem.ListViewSubItem subItem = new ListViewItem.ListViewSubItem(lvi, action);
                lvi.SubItems.Add(subItem);
                listView.Items.Add(lvi);
            }
        }
    }
}
