using Abbybot_III.Core.Guilds.DataType;

using Discord;
using Discord.WebSocket;

using System.Threading.Tasks;

namespace Abbybot_III.Core.Guilds.GuildMessageHandler.DataType.DiscordGuildMessage.User
{
    class LeftMessage : GuildMessage
    {
        SocketGuildUser user;

        public override string BuildDescription(string msg)
        {
            return msg.Replace("[server]", user.Guild.Name).Replace("[user]", user.Username);
        }

        public static async Task<LeftMessage> CreateFromUser(SocketGuildUser user)
        {
            var e = await Get(user.Guild, "bye");

            LeftMessage jm = null;

            if (e != null)
            {
                jm = new LeftMessage()
                {
                    channelId = e.channelId,
                    guildId = e.guildId,
                    imgurl = e.imgurl,
                    type = e.type,
                    user = user,
                    message = e.message,
                    color = Color.Red
                };
                jm.guild = user.Guild;
            }
            return jm;
        }
    }
}