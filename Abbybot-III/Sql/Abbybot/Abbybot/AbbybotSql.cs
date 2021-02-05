using AbbySql;
using AbbySql.Types;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Abbybot_III.Sql.Abbybot.Abbybot
{
    class AbbybotSql
    {
        public static async Task<List<ulong>> GetAbbybotIdAsync(string name)
        {

            List<ulong> abbybots = new List<ulong>();

            var n =AbbysqlClient.EscapeString(name);
            AbbyTable table = await AbbysqlClient.FetchSQL($"select * from `discord`.`abbybots` where `Name` = '{n}'");
            foreach(AbbyRow row in table)
            {
                abbybots.Add(table.Count >= 1 && (row["Id"] is ulong u) ? u : 0);
            }
            return abbybots;
        }
    }
}
