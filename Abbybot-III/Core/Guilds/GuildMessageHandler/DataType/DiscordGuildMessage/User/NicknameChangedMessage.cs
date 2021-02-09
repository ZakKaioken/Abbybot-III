using Abbybot_III.Core.Guilds.DataType;

using Discord;
using Discord.WebSocket;

using System.Threading.Tasks;

namespace Abbybot_III.Core.Guilds.GuildMessageHandler.DataType.DiscordGuildMessage.User
{
    class NicknameChangedMessage : GuildMessage
    {
        string oldname;
        string newname;

        public override string BuildDescription(string msg)
        {
            return msg.Replace("[newname]", newname).Replace("[oldname]", oldname);
        }

        public static async Task<NicknameChangedMessage> CreateFromUser(string oldname, string newname, SocketGuild guild)
        {
            var e = await Get(guild, "NicknameChanged");

            NicknameChangedMessage jm = null;

            if (e != null)
                jm = new NicknameChangedMessage()
                {
                    channelId = e.channelId,
                    guildId = e.guildId,
                    imgurl = e.imgurl,
                    type = e.type,
                    oldname = oldname,
                    newname = newname,
                    message = e.message,
                    color = Color.Red
                };
            jm.guild = guild;
            return jm;
        }
    }
}