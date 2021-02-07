using Abbybot_III.Core.Guilds.sql;

using Discord;
using Discord.WebSocket;

using System.Threading.Tasks;

namespace Abbybot_III.Core.Guilds.DataType
{
    class GuildMessage
    {
        public string type;
        public ulong guildId;
        public ulong channelId;

        public string message
        {
            get
            {
                return _msg;
            }
            set
            {
                _msg = BuildDescription(value);
            }
        }

        string _msg;
        public string imgurl;
        public SocketGuild guild;
        public Color color = Color.LightOrange;

        public virtual string BuildDescription(string msg)
        {
            return msg;
        }

        public static async Task<GuildMessage> Get(SocketGuild Guild, string type)
        {
            return await GuildMessageSql.GetGuildMessage(Guild.Id, type);
        }
    }
}