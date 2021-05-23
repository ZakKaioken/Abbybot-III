using Abbybot_III.Core.Data.User;
using Abbybot_III.Core.Mysql;

using AbbySql;
using AbbySql.Types;

using Capi.Interfaces;

using Discord.WebSocket;

using System;
using System.Collections.Generic;
using System.Linq;

using System.Threading.Tasks;

namespace Abbybot_III.Core.Roles.sql
{
	public class RoleSql
	{
		public static async Task<bool> SetRole(ulong user, ulong guild, ulong role)
		{
			bool dre = await DoesRoleExist(user, guild, role);

			if (dre)
				return false;
			

			bool ran = true;
			try
			{
				await AbbysqlClient.RunSQL($"INSERT INTO `discord`.`roles` (`UserId`,`GuildId`,`RoleId`) VALUES ('{user}', '{guild}','{role}');");
			}
			catch (Exception e)
			{
				Abbybot_III.Abbybot.print(e.Message + e.StackTrace);
			}
			return ran;
		}

		public static async Task<List<AbbybotRole>> GetRoles(SocketGuild g)
		{
			List<AbbybotRole> roles = new List<AbbybotRole>();

			var table = await AbbysqlClient.FetchSQL($"SELECT * FROM `discord`.`guildroles` WHERE `GuildId` = '{g.Id}';");
			foreach (AbbyRow row in table)
			{
				ulong Roleid = (ulong)(row["Id"]);
				int Ranking = (int)row["Ranking"];
				roles.Add(new AbbybotRole((ulong)Roleid, new CommandRatings[] { (CommandRatings)Ranking }));
			}
			return roles;
		}

		public static async Task<List<AbbybotRole>> GetRolesFromUser(AbbybotUser u)
		{
			List<AbbybotRole> roles = new List<AbbybotRole>();

			var table = await AbbysqlClient.FetchSQL($"SELECT * FROM `discord`.`roles` WHERE `UserId` = '{u.Id}' && `GuildId` = '{u.GuildId}';");
			foreach (AbbyRow row in table)
			{
				long Roleid = (long)(row["RoleId"]);
				roles.AddRange(RoleManager.roles.Where(rx => rx.role == (ulong)Roleid).Select(rx => rx));
			}
			return roles;
		}

		public static async Task<bool> DoesRoleExist(ulong user, ulong guild, ulong role)
		{
			var table = await AbbysqlClient.FetchSQL($"SELECT * FROM `discord`.`roles` WHERE `UserId` = '{user}' && `Roleid` ='{role}' && `GuildId` ='{guild}';");

			return (table.Count > 0);
		}
	}
}