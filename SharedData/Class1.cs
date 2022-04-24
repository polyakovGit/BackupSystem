using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using Network.Attributes;
using Network.Packets;

namespace SharedData;

[PacketRequest(typeof(SharedClass))]
public class SharedResponse : ResponsePacket
{
    public SharedResponse(SharedClass result, RequestPacket request)
        : base(request)
    {
        this.Result = result;
    }

    public SharedClass Result { get; set; }
}

[Serializable]
public class FilesInfo
{
    public FilesInfo() { Data = new List<FileStruct>(); }
    private FilesInfo(List<FileStruct> binFiles) { Data = binFiles; }

    [Serializable]
    public struct FileStruct
    {
        public string NameFile;
        public byte[] Bin;
    }

    public List<FileStruct> Data { get; private set; }

    public void Add(string nameFile, byte[] bin) => Data.Add(new FileStruct() { NameFile = nameFile, Bin = bin });

    public byte[] ToArray()
    {
        var binFormatter = new BinaryFormatter();
        var mStream = new MemoryStream();
        binFormatter.Serialize(mStream, Data);
        return mStream.ToArray();
    }

    public static FilesInfo FromBin(byte[] bin)
    {
        var mStream = new MemoryStream();
        var binFormatter = new BinaryFormatter();
        mStream.Write(bin, 0, bin.Length);
        mStream.Position = 0;
        return new FilesInfo(binFormatter.Deserialize(mStream) as List<FileStruct>);
    }
}

public class SharedClass : RequestPacket
{
    public SharedClass()
    {
        Command = Value = "null";
        Files = new byte[10];
    }

    public string Command { get; set; }
    public string Value { get; set; }
    public byte[] Files { get; set; }
}