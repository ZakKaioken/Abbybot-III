using Abbybot_III.Core.Abbybot;

using AbbySql;
using AbbySql.Types;

using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Abbybot_III.Sql.Abbybot.Abbybot
{
    class FunAbbybotFactsSql
    {
        public static async Task<List<FunAbbybotFact>> GetFactsList(bool v)
        {
            List<FunAbbybotFact> faf = new List<FunAbbybotFact>();
            StringBuilder sb = new StringBuilder($"SELECT * FROM funabbybotfacts");
            if (!v) sb.Append(" WHERE `Lewd` = '0'");
            sb.Append(";");
            AbbyTable table = await AbbysqlClient.FetchSQL(sb.ToString());
            if (table.Count > 0)
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