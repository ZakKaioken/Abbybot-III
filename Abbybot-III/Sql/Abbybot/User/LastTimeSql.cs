using AbbySql;

using System.Threading.Tasks;

namespace Abbybot_III.Sql.Abbybot.User
{
	class LastTimeSql
	{
		public static async Task SetTimeSql(ulong userId, ulong guildId, string item, string time)
		{
			if (item.Length == 0)
				return;

			var it = AbbysqlClient.EscapeString(item);
			var a = await AbbysqlClient.FetchSQL($"select * from `user`.`activity` where `UserId`='{userId}' and `GuildId` = '{guildId}' and `Kind` = '{it}'");
			if (a.Count == 0)
			{
				await AbbysqlClient.RunSQL($"insert into `user`.`activity` (`UserId`, `GuildId`, `Kind`, `Time`) values ('{userId}','{guildId}','{it}','{time}');");
				return;
			}
			string s = $"UPDATE `user`.`activity` SET `Time`= '{time}' WHERE `UserId`='{userId}' and `GuildId` = '{guildId}' and `Kind` = '{it}';";

			await AbbysqlClient.RunSQL(s);
		}

		public static async Task<string> GetLastTime(ulong userId, ulong guildId, string item)
		{
			var it = AbbysqlClient.EscapeString(item);
			var a = await AbbysqlClient.FetchSQL($"select * from `user`.`activity` where `UserId`='{userId}' and `GuildId` = '{guildId}' and `Kind` = '{it}'");
			return (a.Count > 0 && a[0]["Time"] is string s) ? s : "";
		}
	}
}