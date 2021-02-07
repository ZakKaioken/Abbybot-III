using Abbybot_III.Commands.Custom.PassiveUsage;
using Abbybot_III.Core.CommandHandler.extentions;
using Abbybot_III.Core.CommandHandler.Types;
using Abbybot_III.Sql.Abbybot.User;

using Abyplay;

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

        public string helpString { get; set; }

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

        bool selfrun = false;

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
            if (Command.ToLower() == "abby") return false;
            StringBuilder sb = new StringBuilder($"running the base evaluate on: {Command.Replace("abbybot ", "")}\n");
            sb.AppendLine($"Type: {(CommandType)this.Type}");
            sb.AppendLine($"Rating: {(CommandRatings)this.Rating}");
            sb.AppendLine($"multithreaded: {this.Multithreaded}");
            sb.AppendLine($"HelpLine: {(await this.toHelpString(aca))}");
            bool hasperms = false;
            if (aca.abbybotUser.userPerms.Ratings != null)
                hasperms = aca.abbybotUser.userPerms.Ratings.Contains(Rating);
            sb.AppendLine($"has permissions: {hasperms}");
            
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
            var canRun = ((isGuild && istextchannel && guilduser && hasperms) || dmchannel);
            sb.AppendLine($"requirements met?: {canRun}");
            var isAbbybot = aca.abbybotUser.Id == Apis.Discord.Discord._client.CurrentUser.Id;
            var IsAbbybotRunnable = isAbbybot && SelfRun;

            sb.AppendLine($"isabbybotrunnable: {IsAbbybotRunnable}");
            sb.AppendLine($"isabbybotrunnable = {isAbbybot} && {SelfRun}");
            await Task.CompletedTask;


            var wecanrun = canRun && !isAbbybot || canRun && IsAbbybotRunnable;
            sb.AppendLine($"wecanrun: {wecanrun}");

            sb.AppendLine($"canrun = (({isGuild} && {istextchannel} && {guilduser} && {hasperms}) OR {dmchannel})");
            sb.AppendLine($"wecanrun = {canRun} && {!isAbbybot} OR {canRun} && {IsAbbybotRunnable} ");

            if (aca.Message.Contains("--debugmode") && !aca.Message.Contains("abbybot say") && !aca.Message.Contains("abbybot dm"))
                await aca.Send(sb);

            bool go = canRun && !isAbbybot || canRun && IsAbbybotRunnable;
            return go;
        }

        public virtual async Task<string> toHelpString(AbbybotCommandArgs aca)
        {
            await Task.CompletedTask;
            return $"{Command}: a contains command.";
        }

        public virtual async Task RegenHelpString()
        {

        }

        public virtual async Task<bool> ShowHelp(AbbybotCommandArgs aca)
        {
            await Task.CompletedTask;
            return await Evaluate(aca);
        }

        async Task iCommand.DoWork(iMsgData md)
        {
            var aca = (md as AbbybotCommandArgs);

            
            
            if (!(this is PassiveCommand))
            {
                bool inTimeOut = aca.abbybotUser.userTrust.inTimeOut;
                //sb.AppendLine($"in time out: {inTimeOut}");
                if (inTimeOut)
                {

                    DateTime time = aca.abbybotUser.userTrust.TimeOutEndDate;
                    string reason = aca.abbybotUser.userTrust.timeoutReason;
                    var tt = TimeStringGenerator.MilistoTimeString((decimal)(time - DateTime.Now).TotalMilliseconds);

                    await aca.Send($"You're in **timeout** for {tt}. You **{reason}** and I can't stand for that. Sorry.");
                    return;
                }

                ulong guildId = 0, channelId = 0;

                if (aca.abbybotGuild != null)
                {
                    guildId = aca.abbybotGuild.GuildId;
                    channelId = aca.channel.Id;
                }
                ulong abbybotId = Apis.Discord.Discord._client.CurrentUser.Id;
                await PassiveUserSql.IncreaseStat(abbybotId, guildId, channelId, aca.abbybotUser.Id, "CommandsSent");
                Console.WriteLine($"I was suposed to increase {"CommandsSent"}");
                await LastTimeSql.SetTimeSql(aca.abbybotUser.Id, "Command", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            }
            await DoWork(aca);
        }

        async Task<bool> iCommand.Evaluate(iMsgData message)
        {
            return await Evaluate(message as AbbybotCommandArgs);
        }

        async Task iCommand.RegenHelpString(iMsgData md)
        {
            helpString = await toHelpString(md as AbbybotCommandArgs);
        }

        async Task<bool> iCommand.ShowHelp(iMsgData md)
        {
            return await ShowHelp(md as AbbybotCommandArgs);
        }


    }
}
