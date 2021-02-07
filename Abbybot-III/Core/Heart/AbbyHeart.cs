using System;
using System.Timers;

namespace Abbybot_III.Core.Heart
{
    class AbbyHeart
    {
        public static AbbyHeart abbyHeart = new AbbyHeart();

        public delegate void HeartBeat(DateTime time);

        public static event HeartBeat heartBeat;

        Timer timer = new Timer(1000);

        public AbbyHeart()
        {
            timer.Elapsed += (e, r) => heartBeat?.Invoke(DateTime.Now);
        }

        public static void Start()
        {
            abbyHeart.timer.Start();
        }
    }
}