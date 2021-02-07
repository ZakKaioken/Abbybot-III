using Abbybot_III.Core.Heart;
using Abbybot_III.Core.Twitter.Queue.sql;
using Abbybot_III.Core.Twitter.Queue.types;

using System;
using System.Threading.Tasks;

namespace Abbybot_III.Core.Twitter.Queue
{
    class ImageQueue
    {
        static DateTime ImageQueueBeat;

        static int imgQueueMilis = 10000;

        public static void init()
        {
            var now = DateTime.Now;

            var imgqueueinitialstart = imgQueueMilis - ((now - DateTime.Today).TotalMilliseconds % (imgQueueMilis));
            ImageQueueBeat = now.AddMilliseconds(imgqueueinitialstart);

            AbbyHeart.heartBeat += async (time) => await beat(time);
        }

        static async Task beat(DateTime time)
        {
            if (ImageQueueBeat < time)
            {
                ImageQueueBeat.AddMilliseconds(imgQueueMilis);

                var count = await ImageQueueSql.Count();
                if (count < 50)
                {
                    var e = await Apis.Booru.AbbyBooru.Execute(new string[] { "abigail_williams*" });

                    Image img = new Image()
                    {
                        url = e.FileUrl.ToString(),
                        sourceurl = e.Source
                    };

                    await ImageQueueSql.Add(img);
                }
            }
        }
    }
}