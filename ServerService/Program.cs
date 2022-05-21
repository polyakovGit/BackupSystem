
using ServerService;

if (!Directory.Exists("BackupFiles"))
    Directory.CreateDirectory("BackupFiles");
IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<Server>();
    })
    .Build();

await host.RunAsync();
var server = new Server();
await server.Listen();