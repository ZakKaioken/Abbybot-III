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
    [Capi.Cmd("cons", 1,1)]
    public class ContainCommand : iCommand
    {
        public string Command
        {
            get; set;
        }
        public bool Multithreaded
        {
            get; set;
        }
        public CommandRatings Rating
        {
            get; set;
        }
        public CommandType Type
        {
            get; set;
        }
        public virtual async Task<bool> Evaluate(AbbybotCommandArgs aca)
        {
            bool e = false;
            if (aca.abbybotUser.userPerms.Ratings != null)
                e = aca.abbybotUser.userPerms.Ratings.Contains(Rating);

            bool v = (aca.Message.ToLower().Contains(Command.ToLower()));
            bool verification = v && e;
            //if (v)
            //await aca.Send($"{Command} tested {aca.AbbybotUser.PreferedName} sent, has permissions {e}, can run {verification}");
            return verification;
        }

        public virtual Task<string> toHelpString()
        {
            return Task.FromResult($"%{Command}: a contains command.");
        }

        public virtual async Task DoWork(AbbybotCommandArgs aca)
        {
            var s = new StringBuilder(Command);

            
            s.Append(" was called with the message: ");
            s.Append(aca.Message.Replace(Command, ""));
            await Task.CompletedTask;
        }

        async Task<bool> iCommand.Evaluate(iMsgData message)
        {
            return await Evaluate(message as AbbybotCommandArgs);
        }

        async Task iCommand.DoWork(iMsgData md)
        {
            await DoWork(md as AbbybotCommandArgs);
        }
    }
}
