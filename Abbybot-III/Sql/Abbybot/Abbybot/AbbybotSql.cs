using AbbySql;
using AbbySql.Types;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace Abbybot_III.Sql.Abbybot.Abbybot
{
    class AbbybotSql
    {
        public static async Task<List<ulong>> GetAbbybotIdAsync()
        {
            List<ulong> abbybots = new List<ulong>();

            AbbyTable table = await AbbysqlClient.FetchSQL($"select * from `abbybot`.`discordbots`");
            foreach (AbbyRow row in table)
            {
                abbybots.Add(table.Count >= 1 && (row["Id"] is ulong u) ? u : 0);
            }
            abbybots.Remove(Apis.Discord.__client.CurrentUser.Id);
            return abbybots;
        }

        public static async Task<List<(ulong guildId, ulong channelId)>> GetAbbybotChannelIdAsync()
        {
            List<(ulong guildId, ulong channelId)> abbybots = new List<(ulong guildId, ulong channelId)>();

            AbbyTable table = await AbbysqlClient.FetchSQL($"select * from `abbybot`.`channels`");
            foreach (AbbyRow row in table)
            {
                var guild = table.Count >= 1 && (row["GuildId"] is ulong g) ? g : 0;
                var channel = table.Count >= 1 && (row["ChannelId"] is ulong c) ? c : 0;
                abbybots.Add((guild, channel));
            }
            return abbybots;
        }
    }
}