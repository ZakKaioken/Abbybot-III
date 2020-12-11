using Abbybot_III.Core.CommandHandler;
using Abbybot_III.Core.CommandHandler.extentions;
using Abbybot_III.Core.CommandHandler.Types;

using Capi.Interfaces;

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
        
        public HelpCommand()
        {
            Multithreaded = true;
        }
        public override bool SelfRun { get => false; set => base.SelfRun = value; }
        public override async Task DoWork(AbbybotCommandArgs abd)
        {

            await abd.Send("Hey cutie master! I'm gathering my list of commands for you now! I'll dm it to you when it's ready! 😁");

            var ratings = abd.abbybotUser.userPerms.Ratings;
            List<iCommand> commands = CommandHandler.capi.commands;

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
                List<iCommand> usableCommands = new List<iCommand>();
                foreach (var i in commands.ToList())
                    if (await i.ShowHelp(abd))
                        usableCommands.Add(i);
                var commandTypeGroups = usableCommands.GroupBy(x => x.GetType());

                StringBuilder sb = new StringBuilder();

                foreach (var tg in commandTypeGroups.Split(4))
                {
                    foreach (var typegroup in tg)
                    {
                        if (typegroup.Count() > 1)
                        {
                            var typzcommands = typegroup.ToList();
                            var bse = typzcommands[0];
                            sb.Append(await bse.toHelpString(abd)).Append(":\n");
                            sb.AppendJoin(", ", typzcommands.Select(x => $"**{x.Command}**"));
                        }
                        else
                        {
                            foreach (var cmd in typegroup.ToList())
                            {
                                sb.Append((await cmd.toHelpString(abd)).Replace(cmd.Command, $"**{cmd.Command}**") + "\n");
                            }
                        }
                    }
                    eb.AddField("\u200b", sb.ToString());
                    sb.Clear();
                }
            }
            else
            {
                eb.AddField("I'mm sso dizzzy?! help me master!!", "!");
            }
            await abd.SendDM(eb);

            if (abd.abbybotUser.userTrust.inTimeOut)
                await abd.Send("I put it in our dms.");
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
