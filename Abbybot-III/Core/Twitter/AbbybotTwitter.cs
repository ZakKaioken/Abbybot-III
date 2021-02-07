using Abbybot_III.Core.Twitter.Queue;

namespace Abbybot_III.Core.Twitter
{
    class AbbybotTwitter
    {
        static TweetQueue tq = new TweetQueue();

        public static void init()
        {
            tq.init();
            ImageQueue.init();
        }
    }
}