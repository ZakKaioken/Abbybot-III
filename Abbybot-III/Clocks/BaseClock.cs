using Abbybot_III.Core.Heart;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Abbybot_III.Clocks
{
    class BaseClock
    {
        public string name = "base clock";
        public async Task Init()
        {

            await OnInit(DateTime.Now);
            AbbyHeart.heartBeat += async (time) => await Tick(time);
        }
        public virtual async Task OnInit(DateTime time)
        {

        }
        public async Task Tick(DateTime time)
        {
            await OnTick(time);
        }
        public virtual async Task OnTick(DateTime time)
        {
            await OnWork(time);
        }

        public virtual async Task OnWork(DateTime time)
        {

        }
    }
}
