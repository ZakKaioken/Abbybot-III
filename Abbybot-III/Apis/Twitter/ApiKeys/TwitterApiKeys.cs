using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Abbybot_III.Apis.Twitter.ApiKeys
{
    class TwitterApiKeys
    {
        public string ConsumerKey;
        public string ConsumerSecret;
        public string AccessToken;
        public string AcessTokenSecret;

        [JsonIgnore]
        public string Path;

        public void Save()
        {
            string api = JsonConvert.SerializeObject(this);
            File.WriteAllText(Path, api);
        }

        public static TwitterApiKeys Load(string path)
        {
            TwitterApiKeys api = JsonConvert.DeserializeObject<TwitterApiKeys>(File.ReadAllText(path));

            return api;
        }
    }
}
