using Abbybot_III.Core.Data.User;
using Abbybot_III.Core.Guilds;
using Abbybot_III.Core.Guilds.sql;

using Discord.WebSocket;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abbybot_III.Core.CommandHandler.Types
{
    public class AbbybotCommandArgs : Capi.Interfaces.iMsgData
    {
        public string Message
        {
            get
            {
                return msg;
            }
            set
            {
                msg = value.Replace("pussy", "usb c").Replace("Pussy", "Usb c").Replace("PUSSY", "USB C").Replace("%", "abbybot ");
            }
            
        }
        public SocketUser author;
        public ISocketMessageChannel channel;
        public SocketMessage originalMessage;
        public List<SocketUser> mentionedUserIds;
        public AbbybotGuild abbybotGuild;
        public AbbybotUser abbybotUser;
        public AbbybotUser abbybotSudoUser;

        string msg;

        public static async Task<AbbybotCommandArgs> MakeArgsFromMessage(SocketMessage sm)
        {
            var aca = new AbbybotCommandArgs();
            aca.Message = sm.Content;
            aca.author = sm.Author;
            aca.abbybotUser = await AbbybotUser.GetUserFromSocketUser(sm.Author);
            aca.channel = sm.Channel;
            aca.originalMessage = sm;

            List<ulong> menids = new List<ulong>();
            foreach (var e in sm.MentionedUsers)
            {
                if (e.Id == Apis.Discord.Discord._client.CurrentUser.Id)
                    aca.abbybotSudoUser = await AbbybotUser.GetUserFromSocketUser(e);
                menids.Add(e.Id);
            }
            aca.mentionedUserIds = sm.MentionedUsers.ToList();


            if (sm.Author is SocketGuildUser sgux) {
                aca.abbybotGuild = new AbbybotGuild { GuildId = sgux.Guild.Id, Name = sgux.Guild.Name };
                await GuildSql.GetGuild(aca.abbybotGuild);
            }
            return aca;
        }

    }
}
