﻿using System.Security.Principal;

namespace DesktopClient;

internal static class Program
{
    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    private static void Main()
    {
        var identity = WindowsIdentity.GetCurrent();
        var principal = new WindowsPrincipal(identity);
        if (!principal.IsInRole(WindowsBuiltInRole.Administrator))
        {
            MessageBox.Show("Запустите с правами администратора");
            return;
        }
        
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        
        Globals.LoadConfig();
        Globals.MainWindow = new Main();
        Application.Run(Globals.MainWindow);
    }
}