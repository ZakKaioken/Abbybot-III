using Abbybot_III.Core.Guilds.DataType;

using AbbySql;
using AbbySql.Types;


using System.Threading.Tasks;

namespace Abbybot_III.Core.Guilds.sql
{
    class GuildMessageSql
    {
        public static async Task<GuildMessage> GetGuildMessage(ulong id, string type)
        {
            GuildMessage g = null;
            
            var table2 = await AbbysqlClient.FetchSQL($"SELECT * FROM servermessages WHERE guildId ='{id}' && type = '{type}';");
            foreach (AbbyRow row in table2)
            {
                g = new GuildMessage()
                {
                    guildId = (ulong)row["guildId"],
                    type = (row["type"] is string s) ? s : "",
                    message = (row["message"] is string msg) ? msg : "",
                    imgurl = (row["imagelink"] is string imlk) ? imlk : "",
                    channelId = (ulong)row["channel"]
                };
            }
            return g;
        }
    }
}