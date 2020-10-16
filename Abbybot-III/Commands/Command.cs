using Abbybot_III.Core.CommandHandler.extentions;
using Abbybot_III.Core.CommandHandler.Types;

using Capi.Interfaces;

using Discord;
using Discord.WebSocket;

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
            StringBuilder sb = new StringBuilder("running the base evaluate\n");

            bool hasperms = false;
            if (aca.abbybotUser.userPerms.Ratings != null)
                hasperms = aca.abbybotUser.userPerms.Ratings.Contains(Rating);
            sb.AppendLine($"has permissions: {hasperms}");
            bool inTimeOut = aca.abbybotUser.userTrust.inTimeOut;
            sb.AppendLine($"in time out: {inTimeOut}");
            if (inTimeOut)
                await aca.Send($"You're in timeout for a little while. You did a mean thing and I can't stand for that. Check your time and details with %timeout. Sorry.");
                
            bool isGuild = aca.abbybotGuild != null;
            bool istextchannel = aca.channel is ITextChannel;
            bool guilduser = aca.author is SocketGuildUser;

            if (istextchannel)
            {
                var aaa = aca.channel as ITextChannel;
                sb.AppendLine($"channel is nsfw: {aaa.IsNsfw}");
            }

            bool guild = isGuild && istextchannel && guilduser;
            sb.AppendLine($"is in guild: {guild}");

            bool dmchannel = aca.channel is SocketDMChannel;
            sb.AppendLine($"is in dms: {dmchannel}");
            var canRun = ((isGuild && !inTimeOut && istextchannel && guilduser && hasperms) || dmchannel);
            sb.AppendLine($"requirements met?: {canRun}");
            var isAbbybot = aca.abbybotUser.Id == Apis.Discord.Discord._client.CurrentUser.Id;
            var IsAbbybotRunnable = isAbbybot && SelfRun;
            
            sb.AppendLine($"isabbybotrunnable: {IsAbbybotRunnable}");
            sb.AppendLine($"isabbybotrunnable = {isAbbybot} && {SelfRun}");
            await Task.CompletedTask;


            var wecanrun = canRun && !isAbbybot || canRun && IsAbbybotRunnable;
            sb.AppendLine($"wecanrun: {wecanrun}");

            sb.AppendLine($"canrun = (({isGuild} && {!inTimeOut} && {istextchannel} && {guilduser} && {hasperms}) OR {dmchannel})");
            sb.AppendLine($"wecanrun = {canRun} && {!isAbbybot} OR {canRun} && {IsAbbybotRunnable} ");

            if (aca.Message.Contains("--debugmode") && !aca.Message.Contains("%say") && !aca.Message.Contains("%dm"))
                await aca.Send(sb);

            
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
