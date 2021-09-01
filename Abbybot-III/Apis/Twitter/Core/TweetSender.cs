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
        static void P(object st)
        {
            Abbybot.print($"[Tweet Sender Core]: {st}");
        }

        static readonly Random r = new Random();

        public static async Task SendTweet()
        {
            Abbybot.print("ayo");
            var abbybotchannels = await AbbybotSql.GetAbbybotChannelIdAsync();

            bool tellnano = abbybotchannels.Count > 0;
            ulong guildId = 0, channelId = 0;
            var er = r.Next(0, abbybotchannels.Count);
            if (tellnano) {
                var (g, ch) = abbybotchannels[er];
            guildId = g; channelId = ch;
            }
			var c = Discord.__client?.GetGuild(guildId)?.GetTextChannel(channelId);
            if (abbybotchannels.Count > 0) { 
                await c.SendMessageAsync("nano gonna work");
            }
            PingAbbybotClock.o = 6; //6 is the working state
            Tweet tweet = null;

            Task<Tweet>[] tweets = new Task<Tweet>[] {
                TweetQueueSql.Peek(),
                TweetArchiveSql.Peek()
            };
            string location = "";
            int i = 0, olo = 0;
            do {
                    tweet = await tweets[i];
                    await AbbyBooru.GetPictureById(tweet.GelId, post =>
                    {
                        tweet.url = post.FileUrl.ToString();
                        tweet.sourceurl = post.Source;
                        location = post.FileUrl.ToString();
                    });
                if (olo++ > 3)
                {
                    i++;
                    olo = 0;
                }
            } while (location == "" && i < tweets.Length);
            Console.Write($"location found? {location!=""}, i was bigger than or equal to dbs? {i>=tweets.Length}");
            if (i >= tweets.Length) return;

            if (tweet == null)
            {
                P("no tweet was found");
                if (tellnano)
                {
                    await c.SendMessageAsync("I'm done working nano");
                    PingAbbybotClock.o = 1;
                }
                return;
            }

            var tempfilepath = await ImageDownloader.DownloadImage(tweet.url);
            Console.WriteLine(tempfilepath);
            if (tempfilepath == null)
            {
                P("no image found.");
                if (tellnano)
                {
                    await c.SendMessageAsync("I'm done working nano");
                    PingAbbybotClock.o = 1;
                }
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
                    if (tellnano)
                    {
                        await c.SendMessageAsync("I'm done working nano");
                        PingAbbybotClock.o = 1;
                    }
                    return;
                }
                file.Dispose();
                Abbybot.print(media.Response.Response);

                if (media.Response.Response.ToLower().Contains("unrecognized"))
                {
                    Abbybot.print("media type unregognized");
                    await TweetQueueSql.Remove(tweet);
					await SendTweet();
                    if (tellnano)
                    {
                        await c.SendMessageAsync("I'm done working nano");
                        PingAbbybotClock.o = 1;
                    }
                    return;
                }
                me = media.Value;
            } while ((me == null) && (tsrs < 3));

            if (me == null)
            {
                await TweetArchiveSql.Add(tweet, true);
                await TweetQueueSql.Remove(tweet);
                await SendTweet();
                if (tellnano)
                {
                    await c.SendMessageAsync("I'm done working nano");
                    PingAbbybotClock.o = 1;
                }
                return;
            }

            string[] s = new string[] { me.Media_Id };
            int tries = 0;
            TwitterStatus tweetvalue;
            do
            {
                tries++;
                Console.Write("I");
                Task<TwitterAsyncResult<TwitterStatus>> o;
                if (!tweet.sourceurl.Contains("twitter"))
                {
                    o = Twitter.ts.SendTweetAsync(new SendTweetOptions { Status = $"{tweet.message}\n\n{tweet.sourceurl} #abigailwilliams #abbybot @AbbyKaioken", MediaIds = s });
                }
                else
                {
                    o = Twitter.ts.SendTweetAsync(new SendTweetOptions { Status = $"{tweet.message}\n\n{tweet.sourceurl} #abigailwilliams #abbybot @AbbyKaioken" });
                }

                TwitterAsyncResult<TwitterStatus> tweeto = await o;


                string a = tweeto?.Response?.Error?.Message switch {
                    "Status is a duplicate." => await Archive(),
                    _ => null
                };
                if (a == "archived")
                {
                    return;
                }
                if (a!=null)
                    Console.WriteLine(a);
                tweetvalue = tweeto.Value;
            } while ((tweetvalue == null) && (tries <= 3));

            if ((tweetvalue == null) || (tries >= 3))
                return;

            if (tweetvalue != null)
            {
                Abbybot.print("sent tweet");
                SaveTweet(tweetvalue, tweet);
            }

            await Archive();

            if (tweetvalue == null)
            {
                await SendTweet();
                if (tellnano)
                {
                    await c.SendMessageAsync("I'm done working nano");
                    PingAbbybotClock.o = 1;
                }
                return;
            }
            if (tellnano)
            {
                await c.SendMessageAsync("I'm done working nano");
                PingAbbybotClock.o = 1; //6 is the working state
            }
            async Task<string> Archive() {
                try {
                    await TweetArchiveSql.Add(tweet, true);
                    await TweetQueueSql.Remove(tweet);
                    return "archived";
                } catch (Exception e) {
                    
                    return $"Failed to archive {e}";
                }
                }
        }

        static void SaveTweet(TwitterStatus tweetvalue, Tweet id)
        {
            return;
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