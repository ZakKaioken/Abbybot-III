using AbbySql;
using AbbySql.Types;

using System.Threading.Tasks;

namespace Abbybot_III.Core.Users.sql
{
    class ChannelFCOverrideSQL
    {
        public static async Task SetCFCAsync(ulong guildId, ulong channelId, bool nsfw, string fc)
        {
            string fcm = AbbysqlClient.EscapeString(fc);
            int insfw = nsfw ? 1 : 0;
            AbbyTable table = await AbbysqlClient.FetchSQL($"select * from `channel`.`fcsettings` where `GuildId` = {guildId} and `ChannelId`= {channelId}");
            if (table.Count < 1)
                await AbbysqlClient.RunSQL($"insert into `channel`.`fcsettings` (`GuildId`, `ChannelId`, `FavoriteCharacter`,`IsNSFW` ) values ('{guildId}', '{channelId}', '{fcm}', {insfw});");
			else
				await AbbysqlClient.RunSQL($"UPDATE `channel`.`fcsettings` SET `FavoriteCharacter`= '{fcm}' WHERE  `GuildId`= {guildId} and `ChannelId` = {channelId};");
        }

        public static async Task<(string fc, bool nsfw)> GetFCMAsync(ulong guildId, ulong channelId)
        {
            AbbyTable table = await AbbysqlClient.FetchSQL($"select * from `channel`.`fcsettings` where `GuildId`= {guildId} and `ChannelId` = {channelId};");
            if (table.Count >= 1)
            {
                var fc = (table[0]["FavoriteCharacter"] is string s && s.Length > 1) ? s : "NO";
                var nsfw = table[0]["IsNSFW"] is int nsf && nsf == 1;
                return (fc, nsfw);
            }
			return ("NO", false);
        }
    }
}