using AbbySql;
using AbbySql.Types;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Abbybot_III.Sql.Abbybot.User
{
	class PassiveUserSql
	{
		public static async Task SetUsernameSql(ulong userId, string username, string nickname = "")
		{
			nickname ??= "";

			string u = (username.Length > 1) ? AbbysqlClient.EscapeString(username) : "";
			string n = (nickname.Length > 1) ? AbbysqlClient.EscapeString(nickname) : "";
			string ss = $"UPDATE `discord`.`users` SET `Username`='{u}'";
			if (n.Length > 1) ss += $", `Name`= '{n}'";
			ss += $" WHERE  `Id`= {userId}; ";
			await AbbysqlClient.RunSQL(ss);
		}

		public static async Task IncreaseStat(ulong abbybotId, ulong guildId, ulong channelId, ulong userId, string stat)
		{
			if (stat.Length == 0)
				return;

			var it = AbbysqlClient.EscapeString(stat);
			var month = DateTime.Now.ToString("MMMM yyyy");
			var a = await AbbysqlClient.FetchSQL($"select * from `discord`.`guilduserthismonthstats` where `Stat`='{stat}' and `AbbybotId` = '{abbybotId}' and `Month`='{month}' and `GuildId`='{guildId}' and `ChannelId`='{channelId}' and `UserId`='{userId}'");
			if (a.Count != 0)
			{
				ulong num = (a[0]["Points"] is ulong points) ? points : 0;

				ulong n = ++num;
				string s = $"UPDATE `discord`.`guilduserthismonthstats` SET `Points`= '{n}' WHERE `Stat`='{stat}' and `AbbybotId` = '{abbybotId}' and `Month`='{month}' and `GuildId`='{guildId}' and `ChannelId`='{channelId}' and `UserId`='{userId}';";
				await AbbysqlClient.RunSQL(s);
			}
			else
			{
				await AbbysqlClient.RunSQL($"insert into `discord`.`guilduserthismonthstats` (`AbbybotId`, `Month`, `GuildId`, `ChannelId`,`UserId`, `Stat`, `Points`) values ('{abbybotId}', '{month}', '{guildId}', '{channelId}', '{userId}','{stat}', '1');");
			}
		}

		public static async Task<List<(ulong userId, ulong stat)>> GetGuildStat(ulong abbybotId, ulong guildId, string v)
		{
			List<(ulong userId, ulong stat)> stats = new List<(ulong userId, ulong stat)>();
			var ori = AbbysqlClient.EscapeString(v);
			var month = DateTime.Now.ToString("MMMM yyyy");
			var a = await AbbysqlClient.FetchSQL($"select * from `discord`.`guilduserthismonthstats` where `AbbybotId` = '{abbybotId}' and `GuildId`='{guildId}' and `Stat`='{ori}';");
			foreach (AbbyRow abr in a)
			{
				var point = abr["Points"] is ulong p ? p : 0;
				var user = abr["UserId"] is ulong u ? u : 0;
				stats.Add((user, point));
			}
			var groupedstats = stats.ToList().GroupBy(x => x.userId);
			stats.Clear();
			foreach (var u in groupedstats)
			{
				ulong o = 0;
				foreach (var s in u)
				{
					o += s.stat;
				}
				stats.Add((u.Key, o));
			}
			return stats;
		}

		public static async Task<List<(ulong channel, ulong stat)>> GetChannelsinGuildStats(ulong abbybotId, ulong guildId, ulong userId, string v)
		{
			List<(ulong, ulong)> statchannels = new List<(ulong, ulong)>();
			var ori = AbbysqlClient.EscapeString(v);
			var month = DateTime.Now.ToString("MMMM yyyy");
			var a = await AbbysqlClient.FetchSQL($"select * from `discord`.`guilduserthismonthstats` where `AbbybotId` = '{abbybotId}' and `Month`='{month}' and `GuildId`='{guildId}' and `UserId`='{userId}' and `Stat`='{ori}';");

			foreach (AbbyRow abr in a)
			{
				ulong channelId = (abr["ChannelId"] is ulong chanid) ? chanid : 0;
				ulong points = (abr["Points"] is ulong poin) ? poin : 0;
				statchannels.Add((channelId, points));
			}

			return statchannels;
		}

		public static async Task IncStat(ulong userId, string item)
		{
			if (item.Length == 0)
				return;
			var it = AbbysqlClient.EscapeString(item);
			var a = await AbbysqlClient.FetchSQL($"select * from `discord`.`users` where `Id`='{userId}'");
			if (a.Count == 0) return;
			int num = (int)a[0][item];

			int n = ++num;
			string s = $"UPDATE `discord`.`users` SET `{it}`= '{n}' WHERE `Id`= {userId};";

			await AbbysqlClient.RunSQL(s);
		}
	}
}