using AbbySql;
using AbbySql.Types;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Abbybot_III.Core.Users.sql
{
    class UserBlacklistSql
    {
        public static async Task<bool> BlackListTag(ulong did, string item)
        {
            var abisb = new StringBuilder();
            abisb.Append($"SELECT * FROM `usergelblacklist` WHERE `userId` = '{did}' && `tag`= '{item}';");
            var table = await AbbysqlClient.FetchSQL(abisb.ToString());
            //Abbybot.print(table.Count);
            if (table.Count > 0)
                throw new Exception($"You already have {item} blacklisted...");

            abisb.Clear();
            //INSERT INTO `discord`.`usergelblacklist` (`userId`, `tag`) VALUES ('1', 'o');
            abisb.Append($"INSERT INTO `discord`.`usergelblacklist`(`userId`, `tag`) VALUES ('{did}', '{item}'); ");
            var e = await AbbysqlClient.RunSQL(abisb.ToString());
            return e > 0;
        }

        public static async Task<List<string>> GetBlackListTags(ulong id)
        {
            var abisb = new StringBuilder();
            abisb.Append("SELECT `tag` FROM `usergelblacklist` WHERE `userId` = '");
            abisb.Append(id);
            abisb.Append("';");
            List<string> tags = new List<string>();

            var table = await AbbysqlClient.FetchSQL(abisb.ToString());
            foreach (AbbyRow row in table)
            {
                tags.Add((row["tag"] is string favchan) ? favchan : "");
            }
            return tags;
        }
    }
}