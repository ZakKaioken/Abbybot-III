using Abbybot_III.Apis.Twitter.ApiKeys;
using Abbybot_III.Apis.Twitter.Core;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using TweetSharp;

namespace Abbybot_III.Apis.Twitter
{
    class Twitter
    {
        public static TwitterService ts;

        public static async Task init()
        {
            var tk = TwitterApiKeys.Load(@"ApiKeys\Twitter.json");
            ts = new TwitterService(tk.ConsumerKey, tk.ConsumerSecret, tk.AccessToken, tk.AcessTokenSecret);
            TweetReciever.init();
        }
    }
}
