using Newtonsoft.Json;

namespace ClientConfig
{
    public class Config
    {
        public string ServerIp { get; set; }
        public int ServerPort { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }

        public Config()
        {
            ServerIp = "127.0.0.1";
            ServerPort = 1708;
            Login = "root";
            Password = "";
        }

        public void SaveToFile(string filename)
        {
            File.WriteAllText(filename, JsonConvert.SerializeObject(this));
        }

        public async Task SaveToFileAsync(string filename)
        {
           await File.WriteAllTextAsync(filename, JsonConvert.SerializeObject(this));
        }

        public static Config LoadFromFile(string filename)
        {
            if (File.Exists(filename))
            {
                Config clientConfig = JsonConvert.DeserializeObject<Config>(File.ReadAllText(filename));
                if (clientConfig != null)
                {
                    return clientConfig;
                }
            }

            return new Config();
        }

        public async static Task<Config> LoadFromFileAsync(string filename)
        {
            if (File.Exists(filename))
            {
                Config clientConfig = JsonConvert.DeserializeObject<Config>(await File.ReadAllTextAsync(filename));
                if (clientConfig != null)
                {
                    return clientConfig;
                }
            }

            return new Config();
        }
    }
}