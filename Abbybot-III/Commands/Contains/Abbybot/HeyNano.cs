using Abbybot_III.Clocks;
using Abbybot_III.Commands.Custom.PassiveUsage;
using Abbybot_III.Core.CommandHandler.extentions;
using Abbybot_III.Core.CommandHandler.Types;
using Abbybot_III.Sql.Abbybot.Abbybot;

using System.Threading.Tasks;

namespace Abbybot_III.Commands.Nano.Nano
{
    [Capi.Cmd("Good morning cute sister abbybot", 1, 1)]
    class GmNano : NanoCommand
    {
        public override async Task DoWork(AbbybotCommandArgs aca)
        {
            await aca.Send("thank you for waking me up cutie nano!!");
            PingAbbybotClock.o = 1;
        }
    }
}