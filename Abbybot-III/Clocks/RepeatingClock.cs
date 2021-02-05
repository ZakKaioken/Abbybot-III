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
        bool firstrun = false;
        public override async Task OnTick(DateTime time)
        {
            if (delay == TimeSpan.Zero)
            {
                Abbybot.print($"A {this.GetType()} has a delay of 0 which is bad pls fix");
                return;
            }

            if (time > tick)
            {
                tick = time.Add(delay);
                if (firstrun) { firstrun = false; return; }
                
                await OnWork(time);
            }
        }
    }
}
