
using Abbybot_III.Core.Twitter.Queue.sql;
using Abbybot_III.Core.Twitter.Queue.types;

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

using TweetSharp;

namespace Abbybot_III.Apis.Twitter.Core
{
    class TweetSender
    {
        static int id;
        static int trycount;

        static void p(object st) 
        {
            Console.WriteLine($"[Tweet Sender Core]: {st}");
        }
        static bool sendingtweet = false;
        public static async Task SendTweet(Tweet tweet)
        {
            if (sendingtweet)
                return;
            sendingtweet = true;
            if (tweet == null)
                return;
            if (tweet.id != id)
            {
                trycount = 0;
            }
            trycount++;

            if (trycount > 2)
            {
                await TweetQueueSql.Remove(tweet); 
                return;
            }

            var tempfilepath = await ImageDownloader.DownloadImage(tweet.url);

            if (tempfilepath == null)
                return;
            
            FileStream file = null;

            do
            {
                try
                {
                    file = new FileStream(tempfilepath, FileMode.Open);
                    await Task.Delay(0);
                }
                catch { }
            } while (file == null);

            MediaFile m = new MediaFile { Content = file };

            TwitterAsyncResult<TwitterUploadedMedia> media = await Twitter.ts.UploadMediaAsync(new UploadMediaOptions { Media = m });
            //Console.WriteLine(media.Response.Response);

            if (media.Response.Response.ToLower().Contains("unrecognized")|| media.Response.Response.ToLower().Contains("bad request"))
            {
                await TweetQueueSql.Remove(tweet);
                //await SendTweet(tweet);
                return;
            }

            TwitterUploadedMedia me = media.Value;
            if (media == null)
                return;
            string[] s = new string[] { me.Media_Id };
            Task<TwitterAsyncResult<TwitterStatus>> o = null;
            if (!tweet.sourceurl.Contains("twitter"))
            {
                o = Twitter.ts.SendTweetAsync(new SendTweetOptions { Status = $"{tweet.message}\n\n{tweet.sourceurl} #abigailwilliams #abbybot @AbbyKaioken", MediaIds = s });
            }
            else
            {
                o = Twitter.ts.SendTweetAsync(new SendTweetOptions { Status = $"{tweet.message}\n\n{tweet.sourceurl} #abigailwilliams #abbybot @AbbyKaioken" });
            }

            TwitterAsyncResult<TwitterStatus> tweeto = await o;
            TwitterStatus tweetvalue = tweeto.Value;

            if (tweetvalue == null)
            {
            }
            else
            {
                SaveTweet(tweetvalue, tweet);
                await TweetQueueSql.Remove(tweet);
            }
            file.Dispose();
            sendingtweet = false;
        }

        private static void SaveTweet(TwitterStatus tweetvalue, Tweet id)
        {
            string dir = @".\Tweets\";
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            string e = tweetvalue.CreatedDate.ToString().Replace(' ', '_').Replace('/', '-').Replace(':', '-');

            try {
            File.WriteAllText($"{dir}Tweet-{e}.json", tweetvalue.RawSource);
            } catch
            {

            }
            }
    }
}
