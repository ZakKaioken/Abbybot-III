using AbbySql;
using AbbySql.Types;

using System.Threading.Tasks;

namespace Abbybot_III.Core.Guilds.sql
{
    class GuildSql
    {
        public static async Task AddGuild(AbbybotGuild guild)
        {
            var name = AbbysqlClient.EscapeString(guild.Name);
            await AbbysqlClient.RunSQL($"INSERT INTO `discord`.`guilds`(Id, Name) VALUES ('{guild.Id}', '{name}'); ");
        }

        public static async Task UpdateGuildName(AbbybotGuild guild)
        {
            var name = AbbysqlClient.EscapeString(guild.Name);
            await AbbysqlClient.RunSQL($"UPDATE `discord`.`guilds` SET `Name` = '{name}' WHERE `Id` ='{guild.Id}';");
        }

        public static async Task GetGuild(AbbybotGuild abg)
        {
            var table = await AbbysqlClient.FetchSQL($"SELECT `Id` FROM `guilds` WHERE `Id` = '{abg.Id}';");
            bool e = table.Count > 0;
            if (!e)
            {
                await AddGuild(abg);
            }

            var table2 = await AbbysqlClient.FetchSQL($"SELECT * FROM `guilds` WHERE `Id` = '{abg.Id}';");
            foreach (AbbyRow row in table2)
            {
                abg.NoLoli = (sbyte)row["NoLoli"] == 1 ? true : false;
                abg.NoNSFW = (sbyte)row["NoNSFW"] == 1 ? true : false;
            }
        }
    }
}