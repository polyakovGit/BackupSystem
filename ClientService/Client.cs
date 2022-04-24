using Network;
using SharedData;

namespace ClientService;

public static class Client
{
    private static TcpConnection? _client;

    public static async Task Connect(string ip)
    {
        _client = (await ConnectionFactory.CreateTcpConnectionAsync(ip, 1708)).Item1;
        //client.RegisterStaticPacketHandler<SharedClass>((packet, connection) =>
    }

    public static async Task<SharedClass> Send(SharedClass shared)
    {
        if (_client is null)
            throw new Exception("connection is not init");
        
        var response = await _client.SendAsync<SharedResponse>(shared);
        return response.Result;
    }
}