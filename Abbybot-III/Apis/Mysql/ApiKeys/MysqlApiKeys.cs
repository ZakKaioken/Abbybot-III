using Newtonsoft.Json;
using System;
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
        public string jPath;

        public void Save()
        {
            string api = JsonConvert.SerializeObject(this);
            File.WriteAllText(jPath, api);
        }

        public static MysqlApiKeys Load(string path)
        {

            var dir = Path.GetDirectoryName(path);
            var fileName = Path.GetFileName(path);
            var e = Path.GetFullPath(path).Replace(fileName, "");
            if (!Directory.Exists(e)) Directory.CreateDirectory(e);
            if (!File.Exists(path))
            {
                var twikeys = new MysqlApiKeys()
                {
                    database = "",
                    password = "",
                    port = 3306,
                    server = "",
                    user = ""
                };
                var tex = JsonConvert.SerializeObject(twikeys);
                File.WriteAllText(path, tex);
            }

            var text = File.ReadAllText(path);
            if (text.Contains("\"\""))
            {
                Console.WriteLine($"Pls help me master... I forgot how to remember... Will you please check the {fileName} file in {dir} to make sure i have my connection info set?");
            }

            return JsonConvert.DeserializeObject<MysqlApiKeys>(File.ReadAllText(path));
        }
    }
}
