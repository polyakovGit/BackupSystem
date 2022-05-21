
using ServerService;

if (!Directory.Exists("BackupFiles"))
    Directory.CreateDirectory("BackupFiles");
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
//var server = new Server();
//await server.Listen();