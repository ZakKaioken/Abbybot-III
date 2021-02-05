using Abbybot_III.Core.Abbybot;

using AbbySql;
using AbbySql.Types;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Abbybot_III.Sql.Abbybot.Abbybot
{
    class FunAbbybotFactsSql
    {
        internal static async Task<List<FunAbbybotFact>> GetLatestMentionIdsAsync()
        {
            List<FunAbbybotFact> faf = new List<FunAbbybotFact>();

            AbbyTable table = await AbbysqlClient.FetchSQL($"SELECT * FROM funabbybotfacts;");
            if (table.Count >0)
            foreach (AbbyRow row in table)
            {
                faf.Add(new FunAbbybotFact
                {
                    id = (ulong)row["Id"],
                    fact = (row["Fact"] is string i) ? i : ""
                });
            }
            return faf;
        }
    }
}
