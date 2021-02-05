using Abbybot_III.Clocks;
using Abbybot_III.Core.CommandHandler.extentions;
using Abbybot_III.Core.CommandHandler.Types;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Abbybot_III.Commands.Contains.Abbybot
{
    [Capi.Cmd("hey nano", 1,1)]
    class HeyNano :ContainCommand
    {
        public override async Task<bool> Evaluate(AbbybotCommandArgs aca)
        {
            var e = (aca.channel.Id == 806556997234327563);
            var u = (aca.abbybotUser.Id == 595308053448884294);
            return e&&u&&await base.Evaluate(aca);
        }
        public override async Task DoWork(AbbybotCommandArgs aca)
        {
            PingAbbybotClock.o = 1;
        }
        public override async Task<bool> ShowHelp(AbbybotCommandArgs aca)
        {
            return false;
        }
    }
}
