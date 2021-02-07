using Abbybot_III.Core.Guilds.DataType;

using Discord;
using Discord.WebSocket;

using System.Threading.Tasks;

namespace Abbybot_III.Core.Guilds.GuildMessageHandler.DataType.DiscordGuildMessage.User
{
    class UnbannedMessage : GuildMessage
    {
        SocketUser user;

        public override string BuildDescription(string msg)
        {
            return msg.Replace("[user]", user.Username);
        }

        public static async Task<UnbannedMessage> CreateFromUser(SocketUser user, SocketGuild guild)
        {
            var e = await Get(guild, "unbanned");

            UnbannedMessage jm = null;

            if (e != null)
                jm = new UnbannedMessage()
                {
                    channelId = e.channelId,
                    guildId = e.guildId,
                    imgurl = e.imgurl,
                    type = e.type,
                    user = user,
                    message = e.message,
                    color = Color.Green
                };
            jm.guild = guild;
            return jm;
        }
    }
}