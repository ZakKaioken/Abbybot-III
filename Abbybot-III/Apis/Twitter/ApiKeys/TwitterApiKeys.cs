using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;

namespace Abbybot_III.Apis.Twitter.ApiKeys
{
    class TwitterApiKeys
    {
        public string ConsumerKey;
        public string ConsumerSecret;
        public string AccessToken;
        public string AcessTokenSecret;

        [JsonIgnore]
        public string jPath;

        public void Save()
        {
            string api = JsonConvert.SerializeObject(this);
            File.WriteAllText(jPath, api);
        }

        public static TwitterApiKeys Load(string path)
        {


            var dir = Path.GetDirectoryName(path);
            var fileName = Path.GetFileName(path);
            var e = Path.GetFullPath(path).Replace(fileName, "");
            if (!Directory.Exists(e)) Directory.CreateDirectory(e);
            if (!File.Exists(path))
            {
                var twikeys = new TwitterApiKeys()
                {
                    AccessToken = "",
                    AcessTokenSecret = "",
                    ConsumerKey = "",
                    ConsumerSecret = ""
                };
                var tex = JsonConvert.SerializeObject(twikeys); 
                File.WriteAllText(path, tex);
            }

            var text = File.ReadAllText(path);
            if (text.Contains("\"\""))
            {
                Console.WriteLine($"uh master... I can't post on twitter without my api keys... Check {fileName} in {dir} for my api keys for me...");
                throw new Exception();
            }

            TwitterApiKeys api = JsonConvert.DeserializeObject<TwitterApiKeys>(File.ReadAllText(path));

            return api;
        }
    }
}
