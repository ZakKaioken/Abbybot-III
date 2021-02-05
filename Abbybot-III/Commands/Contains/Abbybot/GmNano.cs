using Abbybot_III.Clocks;
using Abbybot_III.Core.CommandHandler.extentions;
using Abbybot_III.Core.CommandHandler.Types;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Abbybot_III.Commands.Contains.Abbybot
{
    [Capi.Cmd("good morning nano", 1,1)]
    class GMNano :ContainCommand
    {
        public override async Task<bool> Evaluate(AbbybotCommandArgs aca)
        {
            var e = (aca.channel.Id == 806556997234327563);
            var u = (aca.abbybotUser.Id == 595308053448884294);
            return e&&u&&await base.Evaluate(aca);
        }
        public override async Task DoWork(AbbybotCommandArgs aca)
        {
            await aca.Send("I'm up!! Thanks alot cute sister abbybot!!");
            PingAbbybotClock.o = 1;
        }
        public override Task<bool> ShowHelp(AbbybotCommandArgs aca)
        {
            return Task.FromResult(false);
        }
    }
}
