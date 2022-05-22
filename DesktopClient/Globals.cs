using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Runtime.InteropServices;

namespace DesktopClient;

public static class Globals
{
    private const string NameSetupFile = "Setup.json";
    public static Setup? Setup { get; private set; } = null;

    public static void Init()
    {
        Setup = File.Exists(NameSetupFile)
            ? JsonConvert.DeserializeObject<Setup>(File.ReadAllText(NameSetupFile))
            : new Setup();

        if (!File.Exists(NameSetupFile))
            SaveSetup();
        Process.Start(@"C:\WINDOWS\system32\sc.exe",
        $"create test start=auto binPath=\"{Environment.CurrentDirectory}\\ClientService\\ClientService.exe\"");
        Process.Start(@"C:\Windows\system32\sc.exe", $"start test \"{Environment.CurrentDirectory}\"");
    }

    public static void SaveSetup()
    {
        File.WriteAllText(NameSetupFile, JsonConvert.SerializeObject(Setup));
    }
}

public class Setup
{
    public Dictionary<string, DataFile> PathsToFiles = new();
}

public struct DataFile
{
    public int TypeTimeBackup;
    public DateTime TimeBackup;
}