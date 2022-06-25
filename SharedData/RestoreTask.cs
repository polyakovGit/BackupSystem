using System.Runtime.Serialization.Formatters.Binary;

namespace SharedData
{
    [Serializable]
    public class RestoreTask
    {
        public int Id = -1;
        public string Path = "";

        public RestoreTask(int id, string path)
        {
            Id = id;
            Path = path;
        }

        public byte[] ToArray()
        {
            using (var ms = new MemoryStream())
            {
                var binFormatter = new BinaryFormatter();
                binFormatter.Serialize(ms, this);
                return ms.ToArray();
            }
        }
        public static RestoreTask FromArray(byte[] array)
        {
            using (var ms = new MemoryStream())
            {
                var binFormatter = new BinaryFormatter();
                ms.Write(array, 0, array.Length);
                ms.Position = 0;
                return binFormatter.Deserialize(ms) as RestoreTask;
            }
        }
    }
}
