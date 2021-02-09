using Abbybot_III.Core.Data.User;
using Abbybot_III.Core.Guilds;
using Abbybot_III.Core.Guilds.sql;

using Discord.WebSocket;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Abbybot_III.Core.CommandHandler.Types
{
    public class AbbybotTwitterCommandArgs : AbbybotCommandArgs
    {
        public new string Message
        {
            get
            {
                return msg;
            }
            set
            {
                msg = value.Replace("pussy", "usb c").Replace("Pussy", "Usb c").Replace("PUSSY", "USB C");
            }
        }

        public new SocketUser author;
        public new ISocketMessageChannel channel;
        public new SocketMessage originalMessage;
        public new List<SocketUser> mentionedUserIds;
        public new AbbybotGuild abbybotGuild;
        public new AbbybotUser abbybotUser;
        public new AbbybotUser abbybotSudoUser;

        string msg;

        public static new async Task<AbbybotCommandArgs> MakeArgsFromMessage(SocketMessage sm)
        {
            var aca = new AbbybotCommandArgs
            {
                Message = sm.Content,
                author = sm.Author,
                abbybotUser = await AbbybotUser.GetUserFromSocketUser(sm.Author),
                channel = sm.Channel,
                originalMessage = sm
            };

            List<ulong> menids = new List<ulong>();
            foreach (var e in sm.MentionedUsers)
            {
                if (e.Id == Apis.Discord.Discord._client.CurrentUser.Id)
                    aca.abbybotSudoUser = await AbbybotUser.GetUserFromSocketUser(e);
                menids.Add(e.Id);
            }
            aca.mentionedUserIds = sm.MentionedUsers.ToList();

            if (sm.Author is SocketGuildUser sgux)
            {
                aca.abbybotGuild = new AbbybotGuild { GuildId = sgux.Guild.Id, Name = sgux.Guild.Name };
                await GuildSql.GetGuild(aca.abbybotGuild);
            }
            return aca;
        }
    }
}