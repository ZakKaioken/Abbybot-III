
using Abbybot_III.Core.CommandHandler.extentions;
using Abbybot_III.Core.CommandHandler.Types;

using Discord.WebSocket;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Abbybot_III.Commands.Normal
{
    [Capi.Cmd("%say", 1, 1)]
    class Say : NormalCommand
    {
        public override async Task DoWork(AbbybotCommandArgs a)
        {
            StringBuilder FavoriteCharacter = new StringBuilder(a.Message).Replace(Command, "").Replace("--debugmode", "");
            while (FavoriteCharacter[0] == ' ')
                FavoriteCharacter.Remove(0, 1);

            if (!(a.channel is SocketDMChannel))
                await a.Delete();
            await a.Send(FavoriteCharacter);
        }
    }
}
