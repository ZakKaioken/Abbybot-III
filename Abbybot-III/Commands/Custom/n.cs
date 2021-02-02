using Abbybot_III.Commands.Contains;
using Abbybot_III.Core.CommandHandler.extentions;
using Abbybot_III.Core.CommandHandler.Types;

using Discord;
using Discord.WebSocket;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Abbybot_III.Commands.Custom
{
    [Capi.Cmd("n", 1, 1)]
    class n : ContainCommand
    {
        StringBuilder nhen = new StringBuilder();
        List<int> books = new List<int>();
        public override async Task DoWork(AbbybotCommandArgs message)
        {
            nhen.Clear();
            foreach (var b in books)
            {
                nhen.AppendLine($"https://nhentai.net/g/{b}");
            }
            await message.Send(nhen);
        }

        public override async Task<bool> Evaluate(AbbybotCommandArgs cea)
        {
            if (cea.abbybotGuild.AbbybotIsHere) return false;
            books.Clear();
            var msg = cea.Message.ToLower().Split(" ");
            var cmd = Command.ToLower();
            bool verification = false;
            foreach (var m in msg)
            {
                bool v = m.Contains(cmd.ToLower());
                if (v)
                {
                    int o = 0;
                    var book = m.Split(cmd);
                    if (int.TryParse(book[1], out o))
                    {
                        bool hen = await IsHentai(o);
                        if (hen)
                        {
                            books.Add(o);
                            verification = true;
                        }
                    }
                }
            }
            bool isnsfwchannel = false;
            if (cea.channel is SocketDMChannel sdc)
            {
                isnsfwchannel = true;
            }
            else if (cea.channel is ITextChannel itc)
            {
                isnsfwchannel = itc.IsNsfw;
            }

            if (!cea.abbybotUser.userFavoriteCharacter.IsLewd || !isnsfwchannel)
                verification = false;
            return verification;
        }

        public async Task<bool> IsHentai(int book)
        {

            StringBuilder sb = new StringBuilder();
            object o = null;
            try
            {
                o = await NHentaiSharp.Core.SearchClient.SearchByIdAsync(book);
            }
            catch { }

            bool isbook = false;
            if (o != null)
            {
                isbook = true;
            }
            return isbook;
        }

    }

}
