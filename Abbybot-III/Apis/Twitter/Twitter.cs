using Abbybot_III.Apis.Twitter.ApiKeys;
using Abbybot_III.Apis.Twitter.Core;

using System.Threading.Tasks;

using TweetSharp;

namespace Abbybot_III.Apis.Twitter
{
    class Twitter
    {
        public static TwitterService ts;
        public static bool tson = false;

        public static async Task init()
        {
            try
            {
                var tk = TwitterApiKeys.Load(@"ApiKeys\Twitter.json");
                ts = new TwitterService(tk.ConsumerKey, tk.ConsumerSecret, tk.AccessToken, tk.AcessTokenSecret);
                TweetReciever.init();
                tson = true;
            }
            catch
            {
            }
        }
    }
}