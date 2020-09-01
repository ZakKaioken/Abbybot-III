using Abbybot_III.Core.Heart;
using Abbybot_III.Core.Twitter.Queue;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Abbybot_III.Core.Twitter
{
    class AbbybotTwitter
    {
        public static void init()
        {
            TweetQueue.init();
            ImageQueue.init();
        }
    }
}
