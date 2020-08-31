using Abbybot_III.Core.Guilds.DataType;

using Discord;
using Discord.WebSocket;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Abbybot_III.Core.Guilds.GuildMessageHandler.DataType.DiscordGuildMessage.User
{
    class BannedMessage : GuildMessage
    {
        SocketUser user;

        public override string BuildDescription(string msg)
        {
            return msg.Replace("[user]", user.Username);
        }

        internal static async Task<BannedMessage> CreateFromUser(SocketUser user, SocketGuild guild)
        {
            var e = await Get(guild, "banned");

            BannedMessage jm = null;

            if (e != null)
                jm = new BannedMessage()
                {
                    channelId = e.channelId,
                    guildId = e.guildId,
                    imgurl = e.imgurl,
                    type = e.type,
                    user = user,
                    message = e.message,
                    color = Color.Red
            };
            jm.guild = guild;
            return jm;
        }
    }
}
