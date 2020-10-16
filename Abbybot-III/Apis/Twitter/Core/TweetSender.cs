
using Abbybot_III.Core.Twitter.Queue;
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
        public static async Task SendTweet()
        {
            Tweet tweet = null;
            try {
            tweet = await TweetQueueSql.Peek();
            } catch
            {
                try
                {
                    tweet = await TweetArchiveSql.Peek();
                } catch {}
            }

            if (tweet == null)
            {
                p("no tweet was found");
                return;
            }

            var tempfilepath = await ImageDownloader.DownloadImage(tweet.url);

            if (tempfilepath == null)
            {
               p("no image found.");
                return;
            }

            TwitterUploadedMedia me;
            int tsrs = 0;
            do
            {

                FileStream file = new FileStream(tempfilepath, FileMode.Open);

                MediaFile m = new MediaFile { Content = file };

                tsrs++;
                TwitterAsyncResult<TwitterUploadedMedia> media = await Twitter.ts.UploadMediaAsync(new UploadMediaOptions { Media = m });
                if (media == null)
                {
                    Console.WriteLine("failed to upload media");
                    return;
                }
                file.Dispose();
                Console.WriteLine(media.Response.Response);

                if (media.Response.Response.ToLower().Contains("unrecognized"))
                {
                    Console.WriteLine("media type unregognized");
                    await TweetArchiveSql.Add(tweet, false);
                    await TweetQueueSql.Remove(tweet);
                    await SendTweet();
                    return;
                }
                me = media.Value;
            } while ((me == null) && (tsrs < 3));

            if (me == null)
            {
                await TweetArchiveSql.Add(tweet, false);
                await TweetQueueSql.Remove(tweet);
                await SendTweet();
                return;
            }

            string[] s = new string[] { me.Media_Id };
            Task<TwitterAsyncResult<TwitterStatus>> o = null;
            int tries = 0;
            TwitterStatus tweetvalue;
            do
            {
                tries++;
                Console.Write("I");
                if (!tweet.sourceurl.Contains("twitter"))
                {
                    o = Twitter.ts.SendTweetAsync(new SendTweetOptions { Status = $"{tweet.message}\n\n{tweet.sourceurl} #abigailwilliams #abbybot @AbbyKaioken", MediaIds = s });
                }
                else
                {
                    o = Twitter.ts.SendTweetAsync(new SendTweetOptions { Status = $"{tweet.message}\n\n{tweet.sourceurl} #abigailwilliams #abbybot @AbbyKaioken" });
                }

                TwitterAsyncResult<TwitterStatus> tweeto = await o;
                tweetvalue = tweeto.Value;

            } while ((tweetvalue == null) && (tries <= 3));

            if (tweetvalue != null)
            {
                Console.WriteLine("sent tweet");
                SaveTweet(tweetvalue, tweet);
            }

            await TweetArchiveSql.Add(tweet, false);
            await TweetQueueSql.Remove(tweet);

            if (tweetvalue == null)
            {
                await SendTweet();
                return;
            }
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
