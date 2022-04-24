using System;
using System.CodeDom;
using System.ComponentModel;
using System.Diagnostics;
using System.ServiceProcess;
using System.Threading;
using Newtonsoft.Json;
using SharedData;

namespace ClientService;

#pragma warning disable CA1416
public class WinService : ServiceBase
{
    private const string Servicename = "ClientService";
    private string _homePath;
    private bool _isWork = false;
    private Dictionary<string, DateTime> _nextBackup;

    public WinService()
    {
        this.ServiceName = Servicename;
        this.CanStop = true;
        this.CanPauseAndContinue = false;
        this.AutoLog = false;

        _homePath = string.Empty;
        _nextBackup = new Dictionary<string, DateTime>();
    }

    protected override async void OnStart(string[] args)
    {
        if (args.Length == 0)
            Stop();
        
        await Client.Connect("127.0.0.1");
        _homePath = args[0];
        _isWork = true; 
        await Task.Run(Handler);
    }
    
    protected override void OnStop()
    {
        base.OnStop();
        _isWork = false;
    }

    private async Task Handler()
    {
        while (_isWork)
        {
            var data = JsonConvert.DeserializeObject<Setup>(await File.ReadAllTextAsync($@"{_homePath}\Setup.json"));
            var filesForBackup = new FilesInfo();

            foreach (var file in data!.PathsToFiles.Where(file => !_nextBackup.ContainsKey(file.Key)))
                _nextBackup.Add(file.Key, DateTime.Now);

            var tmpDictForRemove = (from keyValuePair in _nextBackup where !data.PathsToFiles.ContainsKey(keyValuePair.Key) select keyValuePair.Key).ToList();

            foreach (var dictForRemove in tmpDictForRemove)
                _nextBackup.Remove(dictForRemove);

            foreach (var file in _nextBackup.Where(file => file.Value <= DateTime.Now && File.Exists(file.Key)))
            {
                var dateTime = DateTime.Now;
                var timeSpan = new TimeSpan(data.PathsToFiles[file.Key].TimeBackup.Hour, data.PathsToFiles[file.Key].TimeBackup.Minute, data.PathsToFiles[file.Key].TimeBackup.Second);

                dateTime = data.PathsToFiles[file.Key].TypeTimeBackup switch
                {
                    0 => dateTime.AddDays(1),
                    1 => dateTime.AddDays(7),
                    2 => dateTime.AddMonths(1),
                    _ => dateTime
                };

                _nextBackup[file.Key] = dateTime.Date + timeSpan;
                filesForBackup.Add(new FileInfo(file.Key).Name, await File.ReadAllBytesAsync(file.Key));
            }

            if (filesForBackup.Data.Count != 0)
                await Client.Send(new SharedClass()
                {
                    Command = "backup",
                    Files = filesForBackup.ToArray()
                });
            
            await Task.Delay(1_500);
        }
    }
}
#pragma warning restore CA1416


public class Setup
{
    public Dictionary<string, DataFile> PathsToFiles = new();
}

public struct DataFile
{
    public int TypeTimeBackup;
    public DateTime TimeBackup;
}