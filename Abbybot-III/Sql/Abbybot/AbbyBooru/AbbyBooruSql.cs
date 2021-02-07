using Abbybot_III.Core.Guilds;

using AbbySql;

using Discord;
using Discord.WebSocket;

using System;
using System.Threading.Tasks;

namespace Abbybot_III.Core.AbbyBooru.sql
{
    class AbbyBooruSql
    {
        public static async Task AddCharacterAsync(ISocketMessageChannel channel, AbbybotGuild abbybotGuild, string v)
        {
            var tag = AbbysqlClient.EscapeString(v);
            var nsf = (channel as ITextChannel).IsNsfw ? 0 : 1;

            var tbl = await AbbysqlClient.FetchSQL($"SELECT * FROM `discord`.`abbyboorucharacters` WHERE `tag` = '{tag}' AND `channelId` = '{channel.Id}';");

            if (tbl.Count > 0)
            {
                throw new Exception("CharacterAlreadyAdded");
            }

            await AbbysqlClient.RunSQL($"INSERT INTO `discord`.`abbyboorucharacters` ( `tag`,`channelId`, `guildId`, `IsLewd` ) VALUES ('{tag}','{channel.Id}','{abbybotGuild.GuildId}', '{nsf}'); ");
        }

        public static async Task RemoveCharacterAsync(ISocketMessageChannel channel, string v)
        {
            var tag = AbbysqlClient.EscapeString(v);

            var tbl = await AbbysqlClient.FetchSQL($"SELECT * FROM `discord`.`abbyboorucharacters` WHERE `tag` = '{tag}' AND `channelId` = '{channel.Id}';");

            if (tbl.Count < 1)
            {
                throw new Exception("nocharacter");
            }

            await AbbysqlClient.RunSQL($"DELETE FROM `discord`.`abbyboorucharacters` WHERE `tag` = '{tag}' AND `channelId` = '{channel.Id}';");
        }
    }
}