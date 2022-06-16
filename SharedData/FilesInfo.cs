using System.Runtime.Serialization.Formatters.Binary;


namespace SharedData
{
    [Serializable]
    public class FilesInfo
    {
        public FilesInfo() { Data = new List<FileStruct>(); }
        private FilesInfo(List<FileStruct> binFiles) { Data = binFiles; }

        [Serializable]
        public struct FileStruct
        {
            public int Id;
            public DateTime Date;
            public string NameFile;
            public byte[] Bin;
        }

        public List<FileStruct> Data { get; private set; }

        public void Add(int id, DateTime date, string nameFile, byte[] bin) => 
            Data.Add(new FileStruct() { Id = id, Date = date, NameFile = nameFile, Bin = bin });

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
}
