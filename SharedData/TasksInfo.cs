using System;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using Newtonsoft.Json;


namespace SharedData
{
    [Serializable]
    public class TasksInfo
    {
        public TasksInfo() { Data = new Dictionary<int, BackupTask>(); UsedQuota = 0; MaxQuota = 10000000; }
        public Dictionary<int,BackupTask> Data { get; private set; }
        public long UsedQuota { get; set; }
        public long MaxQuota { get; set; }
        public byte[] ToArray()
        {
            using (var ms = new MemoryStream())
            {
                var binFormatter = new BinaryFormatter();
                binFormatter.Serialize(ms, this);
                return ms.ToArray();
            }
        }
        public static TasksInfo FromArray(byte[] array)
        {
            using (var ms = new MemoryStream())
            {
                var binFormatter = new BinaryFormatter();
                ms.Write(array, 0, array.Length);
                ms.Position = 0;
                return binFormatter.Deserialize(ms) as TasksInfo;
            }
        }

        public void SaveToFile(string filename)
        {
            File.WriteAllText(filename, JsonConvert.SerializeObject(this, 
                new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.Auto }));
        }

        public async Task SaveToFileAsync(string filename)
        {
           await File.WriteAllTextAsync(filename, JsonConvert.SerializeObject(this,
                new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.Auto }));
        }

        public static TasksInfo LoadFromFile(string filename)
        {
            if (File.Exists(filename))
            {
                TasksInfo taskInfo = JsonConvert.DeserializeObject<TasksInfo>(File.ReadAllText(filename), 
                    new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.Auto });
                if (taskInfo != null)
                {
                    return taskInfo;
                }
            }

            return new TasksInfo();
        }

        public int GetNextId()
        {
            if (Data.Count == 0)
                return 1;
            return Data.Keys.Max() + 1;
        }
    }
}
