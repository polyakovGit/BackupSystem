using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using Microsoft.WindowsAPICodePack.Dialogs;
using Newtonsoft.Json;

namespace DesktopClient;

public partial class BackupSettings : UserControl
{
    private string _path;
    private DateTime _time;
    private int _type;

    public BackupSettings()
    {
        InitializeComponent();

    }

    public BackupSettings(string path, DataFile data)
    {
        InitializeComponent();
        _path = path;
        _time = data.TimeBackup;
        _type = data.TypeTimeBackup;

        SelectTimeBackup.SelectedIndex = data.TypeTimeBackup;
        TimeBackup.Value = data.TimeBackup;
    }

    private void SelectData_Click(object sender, EventArgs e)
    {
        var dialog = new CommonOpenFileDialog();
        //dialog.IsFolderPicker = true;

        if (dialog.ShowDialog() != CommonFileDialogResult.Ok)
            return;

        Globals.Setup!.PathsToFiles[dialog.FileName] = new DataFile()
        {
            TimeBackup = TimeBackup.Value,
            TypeTimeBackup = SelectTimeBackup.SelectedIndex
        };

        Globals.SaveSetup();
    }
}