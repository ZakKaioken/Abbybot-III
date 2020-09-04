using Abbybot_III.Apis.Twitter.Core;
using Abbybot_III.Core.Heart;
using Abbybot_III.Core.Twitter.Queue.sql;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Abbybot_III.Core.Twitter.Queue
{
    class TweetQueue
    {
    static DateTime TweetQueueBeat;
    static int tweetQueueMilis = 7200000;

    public static void init()
        {
            var now = DateTime.Now;
            var twtqueueinitialstart = tweetQueueMilis - ((now - DateTime.Today).TotalMilliseconds % (tweetQueueMilis));
            TweetQueueBeat = now.AddMilliseconds(twtqueueinitialstart);

            //AbbyHeart.heartBeat += async (time) => await beat(time);
        }
        
        private static async Task beat(DateTime time)
        {
            if (TweetQueueBeat < time)
            {
                TweetQueueBeat.AddMilliseconds(tweetQueueMilis);
                
                var tweet = await TweetQueueSql.Peek();
                await TweetSender.SendTweet(tweet);

            }

        }



    }
}
