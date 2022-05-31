using Network;
using SharedData;
using System.IO;
namespace ServerService
{
    public class Server : BackgroundService
    {
        private ServerConnectionContainer _server;
        private const string TASKS_FILENAME = "Tasks.json";
        private TasksInfo _tasks;

        public async Task Listen()
        {
            _server = ConnectionFactory.CreateServerConnectionContainer(1708, false);
            _server.AllowUDPConnections = false;
            _server.ConnectionEstablished += (conn, type) =>
            {
                conn.RegisterPacketHandler<SharedRequest>(HandlerCommand, this);
                conn.SendAsync<SharedResponse>(new SharedRequest()
                {
                    Command = "tasks",
                    Data = _tasks.ToArray()
                });
            };

            await _server.Start();
        }
        private async void HandlerCommand(SharedRequest packet, Connection connection)
        {
            var exePath = AppDomain.CurrentDomain.BaseDirectory;
            var result = "Error";
            switch (packet.Command)
            {
                case "tasks":
                    {
                        await File.AppendAllTextAsync(Path.Combine(exePath, "log.txt"), "->Tasks updated\n");
                        _tasks = TasksInfo.FromArray(packet.Data);
                        _tasks.SaveToFile(TASKS_FILENAME);
                        //TODO: send to other and locks
                        var request = new SharedRequest()
                        {
                            Command = "tasks",
                            Data = _tasks.ToArray()
                        };
                        foreach (TcpConnection tcpConnection in _server.TCP_Connections)
                        {
                            if (tcpConnection != connection)
                            {
                                await tcpConnection.SendAsync<SharedResponse>(request);
                            }
                        }
                        result = "OK";
                        break;
                    }
                case "backup":
                    {                      
                        await File.AppendAllTextAsync(Path.Combine(exePath,"log.txt"), "->Get files for backup\n");

                        var files = FilesInfo.FromBin(packet.Data);
                        foreach (var file in files.Data)
                            await File.WriteAllBytesAsync(Path.Combine(exePath,$@"BackupFiles\{Path.GetFileName(file.NameFile)}"), file.Bin);

                        result = "OK";
                        break;
                    }
                default:
                    await File.AppendAllTextAsync(Path.Combine(exePath, "log.txt"), $"-> Command not found! ({packet.Command})");
                    break;
            }

            connection.Send(new SharedResponse(result, packet));
        }
        protected internal void Disconnect() => _server.Stop();
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _tasks = TasksInfo.LoadFromFile(TASKS_FILENAME);
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await Listen();
                }
                catch (Exception ex)
                {
                    // обработка ошибки однократного неуспешного выполнения фоновой задачи
                }

                await Task.Delay(1000);
            }
        }
    }
}