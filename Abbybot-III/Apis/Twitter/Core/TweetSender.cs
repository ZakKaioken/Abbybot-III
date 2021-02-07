using Abbybot_III.Clocks;
using Abbybot_III.Core.Twitter.Queue.sql;
using Abbybot_III.Core.Twitter.Queue.types;
using Abbybot_III.Sql.Abbybot.Abbybot;

using System;
using System.IO;
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
            Abbybot.print($"[Tweet Sender Core]: {st}");
        }

        static bool sendingtweet = false;
        static Random r = new Random();

        public static async Task SendTweet()
        {
            Abbybot.print("ayo");
            var abbybotchannels = await AbbybotSql.GetAbbybotChannelIdAsync();
            var er = r.Next(0, abbybotchannels.Count);
            var ch = abbybotchannels[er];
            var c = Discord.Discord._client.GetGuild(ch.guildId).GetTextChannel(ch.channelId);
            await c.SendMessageAsync("nano gonna work");
            PingAbbybotClock.o = 6; //6 is the working state
            Tweet tweet = null;
            try
            {
                tweet = await TweetQueueSql.Peek();
            }
            catch
            {
                try
                {
                    tweet = await TweetArchiveSql.Peek();
                }
                catch { }
            }

            if (tweet == null)
            {
                p("no tweet was found");
                await c.SendMessageAsync("I'm done working nano");
                PingAbbybotClock.o = 1;
                return;
            }

            var tempfilepath = await ImageDownloader.DownloadImage(tweet.url);

            if (tempfilepath == null)
            {
                p("no image found.");
                await c.SendMessageAsync("I'm done working nano");
                PingAbbybotClock.o = 1;
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
                    Abbybot.print("failed to upload media");
                    await c.SendMessageAsync("I'm done working nano");
                    PingAbbybotClock.o = 1;
                    return;
                }
                file.Dispose();
                Abbybot.print(media.Response.Response);

                if (media.Response.Response.ToLower().Contains("unrecognized"))
                {
                    Abbybot.print("media type unregognized");
                    await TweetQueueSql.Remove(tweet);
                    await SendTweet();
                    await c.SendMessageAsync("I'm done working nano");
                    PingAbbybotClock.o = 1;
                    return;
                }
                me = media.Value;
            } while ((me == null) && (tsrs < 3));

            if (me == null)
            {
                await TweetArchiveSql.Add(tweet, true);
                await TweetQueueSql.Remove(tweet);
                await SendTweet();
                await c.SendMessageAsync("I'm done working nano");
                PingAbbybotClock.o = 1;
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
                Abbybot.print("sent tweet");
                SaveTweet(tweetvalue, tweet);
            }

            await TweetArchiveSql.Add(tweet, true);
            await TweetQueueSql.Remove(tweet);

            if (tweetvalue == null)
            {
                await SendTweet();
                await c.SendMessageAsync("I'm done working nano");
                PingAbbybotClock.o = 1;
                return;
            }
            await c.SendMessageAsync("I'm done working nano");
            PingAbbybotClock.o = 1; //6 is the working state
        }

        static void SaveTweet(TwitterStatus tweetvalue, Tweet id)
        {
            string dir = @".\Tweets\";
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            string e = tweetvalue.CreatedDate.ToString().Replace(' ', '_').Replace('/', '-').Replace(':', '-');

            try
            {
                File.WriteAllText($"{dir}Tweet-{e}.json", tweetvalue.RawSource);
            }
            catch
            {
            }
        }
    }
}