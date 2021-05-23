using Newtonsoft.Json;

using System.IO;

namespace Abbybot_III.Apis.ApiKeys
{
    class DiscordApiKey
    {
        public string ApiKey;

#pragma warning disable 649

        [JsonIgnore]
        public string jPath;

#pragma warning restore 649

        public void Save()
        {
            string api = JsonConvert.SerializeObject(this);
            File.WriteAllText(jPath, api);
        }

        public static DiscordApiKey Load(string path)
        {
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
                Abbybot.print($"Master I can't talk to my friends without my discord token... It's the {fileName} file in {dir}!!!");
            }

            DiscordApiKey api = JsonConvert.DeserializeObject<DiscordApiKey>(File.ReadAllText(path));
            return api;
        }
    }
}