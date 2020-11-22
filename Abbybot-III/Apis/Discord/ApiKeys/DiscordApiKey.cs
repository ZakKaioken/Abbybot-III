using Newtonsoft.Json;

using System;
using System.IO;

namespace Abbybot_III.Apis.Discord.ApiKeys
{
    class DiscordApiKey
    {
        public string ApiKey;

        [JsonIgnore]
        public string jPath;

        public void Save()
        {
            string api = JsonConvert.SerializeObject(this);
            File.WriteAllText(jPath, api);
        }

        public static DiscordApiKey Load(string path)
        {
            DiscordApiKey api = null;


            var dir = Path.GetDirectoryName(path);
            var fileName = Path.GetFileName(path);
            var e = Path.GetFullPath(path).Replace(fileName, "");
            if (!Directory.Exists(e)) Directory.CreateDirectory(e);
            if (!File.Exists(path))
            {
                var twikeys = new DiscordApiKey()
                {
                    ApiKey = ""
                };
                var tex = JsonConvert.SerializeObject(twikeys);
                File.WriteAllText(path, tex);
            }

            var text = File.ReadAllText(path);
            if (text.Contains("\"\""))
            {
                Console.WriteLine($"Master I can't talk to my friends without my discord token... It's the {fileName} file in {dir}!!!");
            }


            api = JsonConvert.DeserializeObject<DiscordApiKey>(File.ReadAllText(path));
            return api;
        }

    }
}
