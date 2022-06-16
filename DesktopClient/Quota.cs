namespace DesktopClient
{
    public partial class Quota : Form
    {
        public Quota()
        {
            InitializeComponent();
            labelUsed.Text = $"{Globals.Tasks.UsedQuota/1024} кБайт /";
            textBoxMax.Text = $"{Globals.Tasks.MaxQuota/1024}";
        }
    }
}
