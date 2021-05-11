using Abbybot_III.Clocks;
using Abbybot_III.Commands.Custom.PassiveUsage;
using Abbybot_III.Commands.Nano;
using Abbybot_III.Core.CommandHandler.Types;
using Abbybot_III.Sql.Abbybot.Abbybot;

using System.Threading.Tasks;

namespace Abbybot_III.Commands.Contains.Abbybot
{
    [Capi.Cmd("hey abbybot", 1, 1)]
    class heyNano : NanoCommand
    {
        public override async Task DoWork(AbbybotCommandArgs aca)
        {
            PingAbbybotClock.o = 1;
        }
    }
}