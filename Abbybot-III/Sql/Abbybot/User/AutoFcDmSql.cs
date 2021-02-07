using AbbySql;
using AbbySql.Types;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Abbybot_III.Core.Users.sql
{
    class AutoFcDmSqls
    {
        public static async Task SetAutoFcDmAsync(ulong userId, bool FCMentions)
        {
            int fcm = FCMentions ? 1 : 0;

            AbbyTable table = await AbbysqlClient.FetchSQL($"select * from `discord`.`autofcdms` where `UserId` = {userId}");
            if (table.Count <1)
            {
                await AbbysqlClient.RunSQL($"insert into `discord`.`autofcdms` (`UserId`, `On`) values ('{userId}','{fcm}');");
                return;
            }
            await AbbysqlClient.RunSQL($"UPDATE `discord`.`autofcdms` SET `On`= '{fcm}' WHERE  `UserId`= {userId};");
        }

        public static async Task<bool> GetAutoFcDmAsync(ulong userId)
        {
            AbbyTable table = await AbbysqlClient.FetchSQL($"select * from `discord`.`autofcdms` where `UserId` = {userId}");
            if (table.Count < 1) return false;

            return (table[0]["On"] is sbyte sba) ? ((int)sba == 1) : false;
            
        }

        public static async Task<List<ulong>> GetListAutoFcDmsAsync()
        {
            List<ulong> fcdms = new List<ulong>();
            AbbyTable table = await AbbysqlClient.FetchSQL($"select * from `discord`.`autofcdms` where `On` = 1;");
            foreach(AbbyRow ar in table)
            {
                fcdms.Add((ulong)ar["UserId"]);
            }
            return fcdms;
        }

    }
}
