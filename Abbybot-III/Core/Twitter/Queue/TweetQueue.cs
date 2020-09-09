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
        DateTime TweetQueueBeat;
        TimeSpan tweetQueueMilis = TimeSpan.FromHours(2);

    public void init()
        {
            var now = DateTime.Now;
            var twtqueueinitialstart = tweetQueueMilis.TotalMilliseconds - ((now - DateTime.Today).TotalMilliseconds % (tweetQueueMilis.TotalMilliseconds));
            TweetQueueBeat = now.AddMilliseconds(twtqueueinitialstart);

            AbbyHeart.heartBeat += async (time) => await beat(time);
        }
        
        private  async Task beat(DateTime time)
        {
            if (TweetQueueBeat < time)
            {
                TweetQueueBeat = TweetQueueBeat.AddMilliseconds(tweetQueueMilis.TotalMilliseconds);
                var tweet = await TweetQueueSql.Peek();
                if (tweet != null) {
                await TweetSender.SendTweet(tweet);
                }
            }

        }
    }
}
