using AbbySql;
using AbbySql.Types;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Abbybot_III.Sql.Abbybot.Fun
{
    class SayBlacklistSql
    {
        public static async Task<List<string>> GetBlackListTags()
        {
            var abisb = new StringBuilder();
            abisb.Append("SELECT * FROM `sayblacklist`");
            List<string> tags = new List<string>();

            bool e = false;
            var table = await AbbysqlClient.FetchSQL(abisb.ToString());
            foreach (AbbyRow row in table)
            {
                tags.Add((row["Word"] is string favchan) ? favchan : "");
            }
            return tags;
        }
    }
}
