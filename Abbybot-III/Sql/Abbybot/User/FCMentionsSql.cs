using AbbySql;
using AbbySql.Types;

using System.Threading.Tasks;

namespace Abbybot_III.Core.Users.sql
{
    class FCMentionsSql
    {
        public static async Task SetFCMAsync(ulong userId, bool FCMentions)
        {
            int fcm = FCMentions ? 1 : 0;

            AbbyTable table = await AbbysqlClient.FetchSQL($"select * from `discord`.`fcmentions` where `UserId` = {userId}");
            if (table.Count < 1)
            {
                await AbbysqlClient.RunSQL($"insert into `discord`.`fcmentions` (`UserId`, `FCM`) values ('{userId}','{fcm}');");
                return;
            }
            await AbbysqlClient.RunSQL($"UPDATE `discord`.`fcmentions` SET `FCM`= '{fcm}' WHERE  `UserId`= {userId};");
        }

        public static async Task<bool> GetFCMAsync(ulong userId)
        {
            AbbyTable table = await AbbysqlClient.FetchSQL($"select * from `discord`.`fcmentions` where `UserId` = {userId}");
            return table.Count >= 1 && (((sbyte)table[0]["FCM"]) == 1 ? true : false);
            ;
        }
    }
}