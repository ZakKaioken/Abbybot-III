using Abbybot_III.Commands.Contains;
using Abbybot_III.Core.CommandHandler.extentions;
using Abbybot_III.Core.CommandHandler.Types;

using Abyplay;

using Discord.WebSocket;

using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abbybot_III.Commands.Normal
{
    [Capi.Cmd("abbybot jointime", 1, 1)]
    class jointime : ContainCommand
    {
        public override async Task DoWork(AbbybotCommandArgs a)
        {
            if (!(a.originalMessage.Author is SocketGuildUser sgu))
            {
                var usu = a.GetUser(a.user.Id);
                StringBuilder sb = new StringBuilder();
                foreach (var g in usu.MutualGuilds.ToList())
                {
                    var zkz = g.GetUser(a.user.Id);
                    if (!zkz.JoinedAt.HasValue)
                    {
                        sb.AppendLine($"you didn't have a join time in {g.Name}... somehow...");
                        continue;
                    }
                    var ms = (TimeSpan)(DateTime.Now - zkz.JoinedAt.Value);
                    var ts = TimeStringGenerator.MilistoTimeString((decimal)ms.TotalMilliseconds);
                    sb.AppendLine($"you joined {g.Name} exactly {ts} ago.");
                }
                await a.Send(sb.ToString());
            }
            else
            {
                var po = a.originalMessage.Author is SocketGuildUser sgk;
                var zoz = a.GetGuildUser(sgu.Guild.Id, sgu.Id);
                foreach (var ax in a.getMentionedDiscordGuildUsers())
                {
                    if (ax is not SocketGuildUser sgum) continue;
                    if (!ax.JoinedAt.HasValue)
                    {
                        await a.Send($"{ax.Username} didn't have a join time... somehow...");
                        continue;
                    }
                    var ms = (TimeSpan)(DateTime.Now - ax.JoinedAt.Value);
                    var ts = TimeStringGenerator.MilistoTimeString((decimal)ms.TotalMilliseconds);
                    await a.Send($"{ax.Username} joined {a.server.Name} exactly {ts} ago.");
                }
                if (!a.isMentioning)
                {
                    if (!zoz.JoinedAt.HasValue)
                    {
                        await a.Send($"you didn't have a join time... somehow...");
                        return;
                    }
                    var ms = (TimeSpan)(DateTime.Now - zoz.JoinedAt.Value);
                    var ts = TimeStringGenerator.MilistoTimeString((decimal)ms.TotalMilliseconds);
                    await a.Send($"You joined {a.guild.Name} exactly {ts} ago.");
                }
            }
        }

        public override async Task<string> toHelpString(AbbybotCommandArgs aca)
        {
            if (aca.originalMessage.Author is SocketGuildUser sgk)
            {
                var zoz = aca.GetGuildUser(sgk.Guild.Id,sgk.Id);
                var ms = (TimeSpan)(DateTime.Now - zoz.JoinedAt.Value);
                var ts = TimeStringGenerator.MilistoTimeString((decimal)ms.TotalMilliseconds);

                return $"you joined {sgk.Guild.Name} {ts} ago. Use ab!jointime in a different server to find out how long ago you joined it.";
            }
            return "check how long ago you or someone else joined.";
        }
    }
}