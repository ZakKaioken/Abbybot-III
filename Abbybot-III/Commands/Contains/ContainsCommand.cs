using Abbybot_III.Core.CommandHandler.extentions;
using Abbybot_III.Core.CommandHandler.Types;
using Abbybot_III.Core.Roles.sql;

using Capi.Interfaces;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Abbybot_III.Commands.Contains
{
    [Capi.Cmd("%!contains", 1,1)]
    public class ContainCommand : BaseCommand
    {
        public override async Task<bool> Evaluate(AbbybotCommandArgs aca)
        {
            bool v = (aca.Message.ToLower().Contains(Command.ToLower()));
            bool verification = v && await base.Evaluate(aca);
            return verification;
        }
        public override async Task DoWork(AbbybotCommandArgs aca)
        {
            var s = new StringBuilder(Command);
            s.Append(" was called with the message: ");
            s.Append(aca.Message.Replace(Command, ""));
            await Task.CompletedTask;
        }

        public override async Task<string> toHelpString(AbbybotCommandArgs aca)
        {
            await Task.CompletedTask;
            return $"{Command}: a contains command.";
        }
    }
}
