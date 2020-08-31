using Abbybot_III.Core.Guilds.DataType;

using Discord;
using Discord.WebSocket;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Abbybot_III.Core.Guilds.GuildMessageHandler.DataType.DiscordGuildMessage.User
{
    class JoinedMessage : GuildMessage
    {
        SocketGuildUser user;

        public override string BuildDescription(string msg)
        {
            return msg.Replace("[server]", user.Guild.Name).Replace("[user]", user.Username);
        }

        internal static async Task<JoinedMessage> CreateFromUser(SocketGuildUser user)
        {
            var e = await Get(user.Guild, "welcome");

            JoinedMessage jm = null;

            if (e != null)
                jm = new JoinedMessage()
                {
                    channelId = e.channelId,
                    guildId = e.guildId,
                    imgurl = e.imgurl,
                    type = e.type,
                    user = user,
                    message = e.message,
                    color = Color.Green
            };
            jm.guild = user.Guild;
            return jm;
        }
    }
}
