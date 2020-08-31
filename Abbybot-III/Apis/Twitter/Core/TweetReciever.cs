using Abbybot_III.Core.Heart;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Abbybot_III.Apis.Twitter.Core
{
    class TweetReciever
    {
        public static void init()
        {
            AbbyHeart.heartBeat += async (time) => await RequestBeat(time);
        }

        private static async Task RequestBeat(DateTime time)
        {
            //
        }
    }
}
