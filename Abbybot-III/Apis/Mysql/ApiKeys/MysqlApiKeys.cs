using Newtonsoft.Json;

using System.IO;

namespace Abbybot_III.Apis.Mysql.ApiKeys
{
    class MysqlApiKeys
    {
        public string server;
        public string user;
        public string database;
        public int port;
        public string password;

        public override string ToString()
        {
            return $"server ={server}; user ={user}; database ={database}; port ={port}; password ={password}";
        }

        [JsonIgnore]
        public string Path;

        public void Save()
        {
            string api = JsonConvert.SerializeObject(this);
            File.WriteAllText(Path, api);
        }

        public static MysqlApiKeys Load(string path)
        {
            return JsonConvert.DeserializeObject<MysqlApiKeys>(File.ReadAllText(path));
        }
    }
}
