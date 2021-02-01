
using Abbybot_III.Core.CommandHandler.extentions;
using Abbybot_III.Core.CommandHandler.Types;

using Discord;
using Discord.WebSocket;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Abbybot_III.Commands.Normal
{
    [Capi.Cmd("%dm", 1, 1)]
    class dm : NormalCommand
    {
        public override async Task DoWork(AbbybotCommandArgs a)
        {
            StringBuilder FavoriteCharacter = new StringBuilder(a.Message).Replace(Command, "").Replace("--debugmode", "");
            while (FavoriteCharacter[0] == ' ')
                FavoriteCharacter.Remove(0, 1);

            var mu = a.mentionedUserIds;
            StringBuilder sb = new StringBuilder();
            foreach (var muz in mu)
            {
                await muz.SendMessageAsync(FavoriteCharacter.ToString());
                await Task.Delay(100);
            }
            sb.Append("Sent a dm to ");
            sb.AppendJoin(", ", mu);

            if (!(a.channel is SocketDMChannel))
            await a.Delete();
            await a.Send(sb);
        }
    }
}
