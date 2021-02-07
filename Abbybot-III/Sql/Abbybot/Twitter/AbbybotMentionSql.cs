using AbbySql;
using AbbySql.Types;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace Abbybot_III.Core.AbbyBooru.sql
{
    class AbbybotMentionSql
    {
        public static async Task<List<ulong>> GetLatestMentionIdsAsync(ulong id)
        {
            List<ulong> ids = new List<ulong>();

            AbbyTable table = await AbbysqlClient.FetchSQL($"SELECT * FROM heardtweets WHERE Id = {id};");
            foreach (AbbyRow row in table)
            {
                ids.Add((ulong)row["Id"]);
            }
            return ids;
        }

        public static async Task AddLatestMentionIdAsync(ulong id)
        {
            await AbbysqlClient.RunSQL($"INSERT INTO heardtweets (Id) VALUES ('{id}');");
        }
    }
}