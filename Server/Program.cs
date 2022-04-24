using ClientService;

if (!Directory.Exists("BackupFiles"))
    Directory.CreateDirectory("BackupFiles");

var server = new Server();
await server.Listen(); 