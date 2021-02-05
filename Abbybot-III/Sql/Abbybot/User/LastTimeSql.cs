using AbbySql;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Abbybot_III.Sql.Abbybot.User
{
    class LastTimeSql
    {
        public static async Task SetTimeSql(ulong userId, string item, string time)
        {
            if (item.Length == 0)
                return;

            var it = AbbysqlClient.EscapeString(item);
            var a = await AbbysqlClient.FetchSQL($"select * from `discord`.`useractivity` where `UserId`='{userId}' and `Kind` = '{it}'");
            if (a.Count == 0)
            {
                await AbbysqlClient.RunSQL($"insert into `discord`.`useractivity` (`UserId`, `Kind`, `Time`) values ('{userId}','{it}','{time}');");
                return;
            } 
            string s = $"UPDATE `discord`.`useractivity` SET `Time`= '{time}' WHERE `UserId`='{userId}' and `Kind` = '{it}';";

            await AbbysqlClient.RunSQL(s);
        }

        public static async Task<string> GetLastTime(ulong userId, string item)
        {
            var it = AbbysqlClient.EscapeString(item);
            var a = await AbbysqlClient.FetchSQL($"select * from `discord`.`useractivity` where `UserId`='{userId}' and `Kind` = '{it}'");
            return (a.Count > 0 && a[0]["Time"] is string s) ? s : "";
        }
    }
}
