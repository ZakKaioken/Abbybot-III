using System;
using System.Threading.Tasks;

namespace Abbybot_III.Clocks
{
    class ConsoleCleanerClock : RepeatingClock
    {
        public override async Task OnInit(DateTime time)
        {
            delay = TimeSpan.FromMinutes(5);
            await base.OnInit(time);
        }

        public override async Task OnWork(DateTime time)
        {
            Console.Clear();
        }
    }
}