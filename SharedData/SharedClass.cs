using Network.Attributes;
using Network.Packets;

namespace SharedData;

[PacketRequest(typeof(SharedRequest))]
public class SharedResponse : ResponsePacket
{
    public SharedResponse(string result, SharedRequest request)
        : base(request)
    {
        this.Result = result;
    }
    public string Result { get; set; }
}

public class SharedRequest : RequestPacket
{
    public SharedRequest()
    {
        Command = "null";
        Data = new byte[10];
    }

    public string Command { get; set; }
    public byte[] Data { get; set; }
}