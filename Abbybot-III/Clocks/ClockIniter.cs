using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Abbybot_III.Clocks
{
    
        
    class ClockIniter
    {
        static BaseClock[] clocks = new BaseClock[]
        {
            //new TwitterMentionClock()
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
