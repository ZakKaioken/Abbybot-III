using Abbybot_III.Core.CommandHandler.Types;
using Abbybot_III.Sql.Abbybot.User;

using Capi;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Abbybot_III.Commands.Custom.PassiveUsage
{
    //[Cmd("PassiveCommand", 1, 1)]
    class PassiveCommand : BaseCommand
    {
        public override async Task<bool> ShowHelp(AbbybotCommandArgs aca)
        {
            return false;
        }
        public override async Task<bool> Evaluate(AbbybotCommandArgs aca)
        {
            return true;
        }
    }
}
