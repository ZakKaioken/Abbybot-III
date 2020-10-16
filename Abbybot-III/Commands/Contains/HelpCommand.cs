using Abbybot_III.Core.CommandHandler;
using Abbybot_III.Core.CommandHandler.extentions;
using Abbybot_III.Core.CommandHandler.Types;

using Discord;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abbybot_III.Commands.Contains
{
    [Capi.Cmd("%help", 1, 1)]
    class HelpCommand : ContainCommand
    {
        public override async Task DoWork(AbbybotCommandArgs abd)
        {
            var ratings = abd.abbybotUser.userPerms.Ratings;
            List<string> commands = await CommandHandler.capi.GetHelp(abd);

            EmbedBuilder eb = new EmbedBuilder
            {
                Title = "Commands",
                Description = "Almost every command i will listen to",
                Color = Color.Teal
            };

            if (abd.abbybotUser.userTrust.inTimeOut)
            {
                eb.Color = Color.Red;
                eb.Description = "Your timeout commands.";

                StringBuilder sb = new StringBuilder();
                sb.Append("%help\n%timeout");
                eb.AddField("\u200b", sb.ToString());
            }
            else if (commands.Count > 0)
            {
                var cancoman = commands.ToList().Split(4);
                StringBuilder sb = new StringBuilder();
                foreach (var cancommand in cancoman)
                {
                    sb.Clear();
                    foreach (var cmd in cancommand)
                    {
                        sb.Append(cmd + "\n");
                    }
                    eb.AddField("\u200b", sb.ToString());
                }
            }
            else
            {
                eb.AddField("I'mm sso dizzzy?! help me master!!", "!");
            }
            await abd.SendDM(eb);

            if (abd.abbybotUser.userTrust.inTimeOut)
                await abd.Send("I put it in our dms.");

            await abd.Send("hey master i hope you don't mind i put the help page in our dms. \nif you ask me out and youre underage you're a bad gorl");
        }

        public override async Task<bool> Evaluate(AbbybotCommandArgs aca)
        {
            return aca.Message.ToLower().Contains(Command.ToLower());
        }
        public override async Task<bool> ShowHelp(AbbybotCommandArgs aca)
        {
            return true;
        }

        public override async Task<string> toHelpString(AbbybotCommandArgs aca)
        {
            return $"{Command} displays a list of possible commands to your dms. if you're seeing this, you likely ran the help command!";
        }

    }
    static class LinqExtensions
    {
        public static IEnumerable<IEnumerable<T>> Split<T>(this IEnumerable<T> list, int parts)
        {
            int i = 0;
            var splits = from item in list
                         group item by i++ % parts into part
                         select part.AsEnumerable();
            return splits;
        }
    }

}
