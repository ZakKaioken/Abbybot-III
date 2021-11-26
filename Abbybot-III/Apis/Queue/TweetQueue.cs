using Abbybot_III.Apis.Twitter.Core;
using Abbybot_III.Core.Heart;

using System;
using System.Threading.Tasks;

namespace Abbybot_III.Core.Twitter.Queue
{
    class TweetQueue
    {
        DateTime TweetQueueBeat;
        TimeSpan tweetQueueMilis = TimeSpan.FromHours(5);

        public void init()
        {
            var now = DateTime.Now;
            var twtqueueinitialstart = tweetQueueMilis.TotalMilliseconds - ((now - DateTime.Today).TotalMilliseconds % (tweetQueueMilis.TotalMilliseconds));
            TweetQueueBeat = now.AddMilliseconds(twtqueueinitialstart);

            AbbyHeart.heartBeat += (time) => beat(time).GetAwaiter().GetResult();
        }

        async Task beat(DateTime time)
		{
			if (TweetQueueBeat < time)
			{
				Abbybot_III.Abbybot.print("Sending Tweet");
				TweetQueueBeat = TweetQueueBeat.AddMilliseconds(tweetQueueMilis.TotalMilliseconds);
				int tries = 0;
                do
                {
                    await TweetSender.SendTweet(
                        onFail: fail =>
						{
							tries++;
                            Console.WriteLine($"I failed to send a tweet:\n{fail}\ngonna try again...");
						}, 
                        onSucceed: () => tries = 10
                    );
                } while (tries<3);
			}
		}
	}
}