
using ServerService;

var pathBackup =Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"BackupFiles");
if (!Directory.Exists(pathBackup))
    Directory.CreateDirectory(pathBackup);
IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<Server>();
    })
    .UseWindowsService(options =>
        {
            options.ServiceName = "MainService";
        })
    .Build();

await host.RunAsync();