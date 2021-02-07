using AbbySql;
using AbbySql.Types;

using System.Threading.Tasks;

namespace Abbybot_III.Core.Users.sql
{
    class ChannelFCOverride
    {
        public static async Task SetCFCAsync(ulong guildId, ulong channelId, string fc)
        {
            string fcm = AbbysqlClient.EscapeString(fc);

            AbbyTable table = await AbbysqlClient.FetchSQL($"select * from `discord`.`channelfcsettings` where `GuildId` = {guildId} and `ChannelId`= {channelId}");
            if (table.Count < 1)
            {
                await AbbysqlClient.RunSQL($"insert into `discord`.`channelfcsettings` (`GuildId`, `ChannelId`, `FavoriteCharacter`) values ('{guildId}', '{channelId}', '{fcm}');");
                return;
            }
            await AbbysqlClient.RunSQL($"UPDATE `discord`.`channelfcsettings` SET `FavoriteCharacter`= '{fcm}' WHERE  `GuildId`= {guildId} and `ChannelId` = {channelId};");
        }

        public static async Task<string> GetFCMAsync(ulong guildId, ulong channelId)
        {
            AbbyTable table = await AbbysqlClient.FetchSQL($"select * from `discord`.`channelfcsettings` where `GuildId`= {guildId} and `ChannelId` = {channelId};");

            return (table.Count >= 1 && table[0]["FavoriteCharacter"] is string s && s.Length > 1) ? s : "NO";
        }
    }
}