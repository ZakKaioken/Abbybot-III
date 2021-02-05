using AbbySql;

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Abbybot_III.Sql.Abbybot.User
{
    class PassiveUserSql
    {
        public static async Task SetUsernameSql(ulong userId, string username, string nickname="")
        {
            nickname = (nickname != null) ? nickname : "";
            
            string u = (username.Length > 1) ? AbbysqlClient.EscapeString(username) : "";
            string n = (nickname.Length >1)? AbbysqlClient.EscapeString(nickname):"";
            string ss = $"UPDATE `discord`.`users` SET `Username`='{u}'";
            if (n.Length > 1) ss += $", `Name`= '{n}'";
                ss += $" WHERE  `Id`= {userId}; ";
            await AbbysqlClient.RunSQL(ss);

        }

        public static async Task IncStat(ulong userId, string item)
        {
            if (item.Length == 0)
                return;
            var it = AbbysqlClient.EscapeString(item);
            var a = await AbbysqlClient.FetchSQL($"select * from `discord`.`users` where `Id`='{userId}'");
            if (a.Count == 0) return;
            int num = (int)a[0][item];
            
            int n = ++num;
            string s = $"UPDATE `discord`.`users` SET `{it}`= '{n}' WHERE `Id`= {userId};";
            
            await AbbysqlClient.RunSQL(s);
        }
    }
}
