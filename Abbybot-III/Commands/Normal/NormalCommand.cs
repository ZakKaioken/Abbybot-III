using Abbybot_III.Core.CommandHandler.extentions;
using Abbybot_III.Core.CommandHandler.Types;

using Capi.Interfaces;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Abbybot_III.Commands.Normal
{
    [Capi.Cmd("%!normal", 1, 1)]
    class NormalCommand : iCommand
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


        public virtual async Task DoWork(AbbybotCommandArgs e)
        {
            List<string> args = new List<string>(e.Message.Replace(Command, "").Split(' '));
            if (args[0] == " ")
                args.Remove(" ");

            StringBuilder sb = new StringBuilder();
            sb.Append($"normal command {Command} was called. got back these args:");
            foreach (string item in args)
            {
                sb.Append($" {item}");
            }

            await e.Send(sb);
        }

        public virtual async Task<bool> Evaluate(AbbybotCommandArgs aca)
        {
            var e = aca.Message.Split();
            bool er = e[0] == Command && aca.abbybotUser.userPerms.Ratings.Contains(Rating);

            if (aca.abbybotUser.userTrust.inTimeOut && er)
            {
                await aca.Send($"You're in timeout for a little while. You did a mean thing and I can't stand for that. Check your time and details with %timeout. Sorry.");
                er = false;
            }
            await Task.CompletedTask;
            return er;
        }
        public virtual async Task<string> toHelpString(AbbybotCommandArgs aca)
        {
            return $"{Command}: a contains command.";
        }
        public virtual bool ShowHelp(AbbybotCommandArgs aca)
        {
            return aca.abbybotUser.userPerms.Ratings.Contains(Rating);
        }
        async Task iCommand.DoWork(iMsgData md)
        {
            await DoWork(md as AbbybotCommandArgs);
        }

        async Task<bool> iCommand.Evaluate(iMsgData message)
        {
            return await Evaluate(message as AbbybotCommandArgs);
        }

        async Task<string> iCommand.toHelpString(iMsgData md)
        {
            return await toHelpString(md as AbbybotCommandArgs);
        }

        bool iCommand.ShowHelp(iMsgData md)
        {
            return ShowHelp(md as AbbybotCommandArgs);
        }

    }
}
