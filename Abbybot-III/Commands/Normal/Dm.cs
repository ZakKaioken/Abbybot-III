
using Abbybot_III.Core.CommandHandler.extentions;
using Abbybot_III.Core.CommandHandler.Types;

using Discord;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Abbybot_III.Commands.Normal
{
    [Capi.Cmd("%dm", 1, 1)]
    class Dm : NormalCommand
    {
        public override async Task DoWork(AbbybotCommandArgs a)
        {
            StringBuilder FavoriteCharacter = new StringBuilder(a.Message.Replace(Command, ""));
            while (FavoriteCharacter[0] == ' ')
                FavoriteCharacter.Remove(0, 1);


            var mu = a.mentionedUserIds;
            StringBuilder sb = new StringBuilder();
            await a.Send($"you are sending messages to: ");
            foreach (var muz in mu)
            {
                //await muz.SendMessageAsync(FavoriteCharacter.ToString());
                await Task.Delay(100);
            }
            sb.Append("Sent a dm to ");
            sb.AppendJoin(", ", mu);


            await a.Delete();
            await a.Send(sb);
        }
    }
}
