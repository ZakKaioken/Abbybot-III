using Abbybot_III.Core.Guilds.DataType;

using AbbySql;
using AbbySql.Types;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Abbybot_III.Core.Guilds.sql
{
    class GuildMessageSql
    {
        internal static async Task<GuildMessage> GetGuildMessage(ulong id, string type)
        {
            StringBuilder abisb = new StringBuilder("SELECT * FROM servermessages WHERE ");
            abisb.Append($"guildId ='{id}' && ");
                abisb.Append($"type = '{type}';");
            GuildMessage g = null;
            var table2 = await AbbysqlClient.FetchSQL(abisb.ToString());
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
