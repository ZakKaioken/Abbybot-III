using AbbySql;
using AbbySql.Types;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Abbybot_III.Core.Guilds.sql
{
    class GuildSql
    {
        public static async Task AddGuild(AbbybotGuild guild)
        {
            var name =AbbysqlClient.EscapeString(guild.Name);
            await AbbysqlClient.RunSQL($"INSERT INTO `discord`.`guilds`(Id, Name) VALUES ('{guild.GuildId}', '{name}'); ");
        }
        public static async Task UpdateGuildName(AbbybotGuild guild)
        {
            var name = AbbysqlClient.EscapeString(guild.Name);
            await AbbysqlClient.RunSQL($"UPDATE `discord`.`guilds` SET `Name` = '{name}' WHERE `Id` ='{guild.GuildId}';");
        }
        public static async Task GetGuild(AbbybotGuild abg) 
        {

            var table = await AbbysqlClient.FetchSQL($"SELECT `Id` FROM `guilds` WHERE `Id` = '{abg.GuildId}';");
            bool e = table.Count > 0;
            if (!e)
            {
                await AddGuild(abg);
            }

            var table2 = await AbbysqlClient.FetchSQL($"SELECT * FROM `guilds` WHERE `Id` = '{abg.GuildId}';");
            foreach (AbbyRow row in table2)
            {
                abg.NoLoli = (sbyte)row["NoLoli"] == 1 ? true : false;
                abg.NoNSFW = (sbyte)row["NoNSFW"] == 1 ? true : false;
            }
        }

        }
}
