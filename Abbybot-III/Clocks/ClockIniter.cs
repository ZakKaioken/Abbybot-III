using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Abbybot_III.Clocks
{
    
        
    class ClockIniter
    {
        public static BaseClock[] clocks = new BaseClock[]
        {
            //new TwitterMentionClock()
            new AutoFcDmClock(),
            new PingAbbybotClock()
        };

        public static async Task init()
        {
            foreach(var clock in clocks)
            {
                await clock.Init();
            }
        }
    }
}
