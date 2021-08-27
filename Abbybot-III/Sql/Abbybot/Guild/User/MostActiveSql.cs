using AbbySql;
using AbbySql.Types;

using System;
using System.Collections.Generic;
using System.Linq;

using System.Threading.Tasks;

namespace Abbybot_III.Sql.Abbybot.Guild.User
{
	class MostActiveSql
	{
		public static async Task<List<(ulong guildId, ulong roleId)>> GetRoles()
		{
			List<(ulong guildId, ulong roleId)> roleIds = new List<(ulong guildId, ulong roleId)>();
			AbbyTable table = await AbbysqlClient.FetchSQL($"select * from `guild`.`mostactiveroles`;");
			if (table != null)
				foreach (AbbyRow row in table)
				{
					var g = row["GuildId"] is ulong guild ? guild : 0;
					var u = row["RoleId"] is ulong role ? role : 0;
					roleIds.Add((g, u));
				}
			return roleIds;
		}

		public static async Task<ulong> GetRole(ulong guildId)
		{
			AbbyTable t = await AbbysqlClient.FetchSQL($"select *  from `guild`.`mostactiveroles` where `GuildId`='{guildId}';");
			if (t.Count > 0)
				return t[0]["roleId"] is ulong u ? u : 0;
			return 0;
		}

		public static async Task<bool> AddRole(ulong guildId, ulong roleId)
		{
			var t = await AbbysqlClient.FetchSQL($"select *  from `guild`.`mostactiveroles` where `GuildId`='{guildId}' and `RoleId` = '{roleId}';");
			if (t.Count < 1)
				return await AbbysqlClient.RunSQL($"insert into `guild`.`mostactiveroles`(`GuildId`, `RoleId`) values ('{guildId}', '{roleId}');") > 0;
			return false;
		}

		public static async Task<bool> RemoveRole(ulong guildId, ulong roleId)
		{
			AbbyTable t = await AbbysqlClient.FetchSQL($"select *  from `guild`.`mostactiveroles` where `GuildId`='{guildId}';");
			if (t.Count > 0)
				return await AbbysqlClient.RunSQL($"insert into `guild`.`mostactiveroles`(`GuildId`, `RoleId`) values ('{guildId}', '{roleId}');") > 0;
			return false;
		}

		public static async Task UpdateRole(ulong id)
		{
		}
	}
}