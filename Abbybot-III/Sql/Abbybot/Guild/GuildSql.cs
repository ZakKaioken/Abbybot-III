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
            await AbbysqlClient.RunSQL($"INSERT INTO `abbybot`.`guilds`(GuildId, Name) VALUES ('{guild.Id}', '{name}'); ");
        }

        public static async Task UpdateGuildName(AbbybotGuild guild)
        {
            var name = AbbysqlClient.EscapeString(guild.Name);
            await AbbysqlClient.RunSQL($"UPDATE `abbybot`.`guilds` SET `Name` = '{name}' WHERE `GuildId` ='{guild.Id}';");
        }

        public static async Task GetGuild(AbbybotGuild abg)
        {
            var table = await AbbysqlClient.FetchSQL($"SELECT `GuildId` FROM `abbybot`.`guilds` WHERE `GuildId` = '{abg.Id}';");
            bool e = table.Count > 0;
            if (!e)
            {
                await AddGuild(abg);
            }

            var table2 = await AbbysqlClient.FetchSQL($"SELECT * FROM `abbybot`.`guilds` WHERE `GuildId` = '{abg.Id}';");
            foreach (AbbyRow row in table2)
            {
                abg.NoLoli = (sbyte)row["NoLoli"] == 1 ? true : false;
                abg.NoNSFW = (sbyte)row["NoNSFW"] == 1 ? true : false;
                abg.PrefAbbybot = row["PrefAbbybot"] is ulong pabi ? pabi : 0;
                abg.AutoDeleteTime = row["DeleteAfterSeconds"] is int secs ? secs : -1;
            }
        }
        public static async Task<AbbybotGuild> GetGuild(ulong abg)
        {
            var g = new AbbybotGuild() { Id = abg };
            var table = await AbbysqlClient.FetchSQL($"SELECT `GuildId` FROM `abbybot`.`guilds` WHERE `GuildId` = '{g.Id}';");
			bool e = table.Count > 0;
            if (abg == 0) return null;

            var table2 = await AbbysqlClient.FetchSQL($"SELECT * FROM `abbybot`.`guilds` WHERE `GuildId` = '{g.Id}';");
            foreach (AbbyRow row in table2)
            {
                g.NoLoli = (sbyte)row["NoLoli"] == 1 ? true : false;
                g.NoNSFW = (sbyte)row["NoNSFW"] == 1 ? true : false;
                g.PrefAbbybot = row["PrefAbbybot"] is ulong pabi ? pabi : 0;
                g.AutoDeleteTime = row["DeleteAfterSeconds"] is int secs ? secs : -1;
            }
            return g;
        }

		internal static async Task UpdatePrefAbbybot(ulong guildId, ulong abbybotId)
		{
            await AbbysqlClient.RunSQL($"UPDATE `abbybot`.`guilds` SET `PrefAbbybot` = '{abbybotId}' WHERE `GuildId` ='{guildId}';");
        }
    }
}