using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Abbybot_III.Clocks
{
    class RepeatingClock : BaseClock
    {
        DateTime tick = DateTime.Now;
        public TimeSpan delay = TimeSpan.Zero;

        public override async Task OnTick(DateTime time)
        {
            if (delay == TimeSpan.Zero)
            {
                Console.WriteLine($"A {this.GetType()} has a delay of 0 which is bad pls fix");
                return;
            }

            if (time > tick)
            {
                tick = time.Add(delay);
                await OnWork(time);
            }
        }
    }
}
