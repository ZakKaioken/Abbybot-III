using Abbybot_III.Core.CommandHandler.extentions;
using Abbybot_III.Core.CommandHandler.Types;

using Capi.Interfaces;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Abbybot_III.Commands
{
    public class BaseCommand : iCommand
    {

        public string Command
        {
            get; set;
        }

        public virtual bool SelfRun
        {
            get
            {
                return selfrun;
            }
            set
            {
                selfrun = value;
            }
        }

        private bool selfrun = false;

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

        public virtual async Task DoWork(AbbybotCommandArgs aca)
        {
            await Task.CompletedTask;
            await aca.Send("a blank command with no type was sent...");
        }


        public virtual async Task<bool> Evaluate(AbbybotCommandArgs aca)
        {
            bool canRun = false;
            if (aca.abbybotUser.userPerms.Ratings != null)
                canRun = aca.abbybotUser.userPerms.Ratings.Contains(Rating);

            if(aca.abbybotUser.userTrust.inTimeOut && canRun)
            {
                await aca.Send($"You're in timeout for a little while. You did a mean thing and I can't stand for that. Check your time and details with %timeout. Sorry.");
                canRun = false;
            }

            var isAbbybot = aca.abbybotUser.Id != Apis.Discord.Discord._client.CurrentUser.Id;
            var IsAbbybotRunnable = isAbbybot && SelfRun;

            await Task.CompletedTask;

            return canRun && !isAbbybot || canRun && IsAbbybotRunnable;
            
        }

        public virtual async Task<string> toHelpString(AbbybotCommandArgs aca)
        {
            await Task.CompletedTask;
            return $"{Command}: a contains command.";
        }


        public virtual async Task<bool> ShowHelp(AbbybotCommandArgs aca)
        {
            await Task.CompletedTask;
            return await Evaluate(aca);
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

        async Task<bool> iCommand.ShowHelp(iMsgData md)
        {
            return await ShowHelp(md as AbbybotCommandArgs);
        }


    }
}
