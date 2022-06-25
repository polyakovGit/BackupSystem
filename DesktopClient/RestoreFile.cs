
namespace DesktopClient
{
    public partial class RestoreFile : Form
    {
        public RestoreFile()
        {
            InitializeComponent();
        }

        private void buttonSelectFile_Click(object sender, EventArgs e)
        {
            var dialog = new SaveFileDialog();
            dialog.InitialDirectory = !string.IsNullOrEmpty(textBoxFilename.Text)
                ? Path.GetDirectoryName(textBoxFilename.Text)
                : Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            if (dialog.ShowDialog() != DialogResult.OK)
                return;

            textBoxFilename.Text = dialog.FileName;
        }
    }
}
