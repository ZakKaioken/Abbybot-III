using AbbySql;
using AbbySql.Types;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace Abbybot_III.Core.Users.sql
{
    class AutoFcDmoSqls
    {
        public static async Task SetAutoFcDmAsync(ulong userId, bool FCMentions)
        {
            int fcm = FCMentions ? 1 : 0;

            AbbyTable table = await AbbysqlClient.FetchSQL($"select * from `user`.`autofcdms` where `UserId` = {userId}");
            if (table.Count < 1)
            {
                await AbbysqlClient.RunSQL($"insert into `user`.`autofcdms` (`UserId`, `On`) values ('{userId}','{fcm}');");
                return;
            }
            await AbbysqlClient.RunSQL($"UPDATE `user`.`autofcdms` SET `On`= '{fcm}' WHERE  `UserId`= {userId};");
        }

        public static async Task<bool> GetAutoFcDmAsync(ulong userId)
        {
            AbbyTable table = await AbbysqlClient.FetchSQL($"select * from `user`.`autofcdms` where `UserId` = {userId}");
            if (table.Count < 1) return false;

            return (table[0]["On"] is sbyte sba) && ((int)sba == 1);
        }

        public static async Task<List<ulong>> GetListAutoFcDmsAsync()
        {
            List<ulong> fcdms = new List<ulong>();
            AbbyTable table = await AbbysqlClient.FetchSQL($"select * from `user`.`autofcdms` where `On` = 1;");
            foreach (AbbyRow ar in table)
            {
                fcdms.Add((ulong)ar["UserId"]);
            }
            return fcdms;
        }
    }
}