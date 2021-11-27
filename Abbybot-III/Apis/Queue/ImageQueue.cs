using Abbybot_III.Core.Heart;
using Abbybot_III.Core.Twitter.Queue.sql;
using Abbybot_III.Core.Twitter.Queue.types;

using BooruSharp.Search.Post;

using System;
using System.Collections.Generic;
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
                    var ee = await Apis.AbbyBooru.ExecuteAsync(new string[] { "abigail_williams*" });

                    foreach (var e in ee) {
                        await ImageQueueSql.Add(new Image()
                        {
                            url = e.FileUrl.ToString(),
                            sourceurl = e.Source,
                            gelId = e.id,
                            md5 = e.md5
                        });
                    }
                }
            }
        }
    }
}