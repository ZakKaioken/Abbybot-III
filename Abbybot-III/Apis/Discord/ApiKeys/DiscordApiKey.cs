using Newtonsoft.Json;

using System;
using System.IO;

namespace Abbybot_III.Apis.Discord.ApiKeys
{
    class DiscordApiKey
    {
        public string ApiKey;

        [JsonIgnore]
        public string Path;

        public void Save()
        {
            string api = JsonConvert.SerializeObject(this);
            File.WriteAllText(Path, api);
        }

        public static DiscordApiKey Load(string path)
        {
            DiscordApiKey api = null;
            try
            {
                api = JsonConvert.DeserializeObject<DiscordApiKey>(File.ReadAllText(path));
            }
            catch
            {
                Console.WriteLine("Master!! Hey!! You forgot to give me my discord api keys!!!");
            }
            return api;
        }

    }
}
