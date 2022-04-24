using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DesktopClient;

public partial class Main : Form
{
    public Main()
    {
        InitializeComponent();
        if (Globals.Setup!.PathsToFiles.Count != 0)
            foreach (var file in Globals.Setup!.PathsToFiles)
                elements.Controls.Add(new BackupSettings(file.Key, file.Value));
    }

    private void AddBtnClick(object? sender, EventArgs e) => elements.Controls.Add(new BackupSettings());
}