using AbbySql;
using AbbySql.Types;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Abbybot_III.Core.Users.sql
{
	class UserBlacklistSql
	{
		public static async Task<bool> BlackListTag(ulong did, string item)
		{
			item = AbbySql.AbbysqlClient.EscapeString(item);
			var abisb = new StringBuilder();
			abisb.Append($"SELECT * FROM `usergelblacklist` WHERE `userId` = '{did}' && `tag`= '{item}';");
			var table = await AbbysqlClient.FetchSQL(abisb.ToString());
			//Abbybot.print(table.Count);
			if (table.Count > 0)
				throw new Exception($"You already have {item} blacklisted...");

			abisb.Clear();
			//INSERT INTO `discord`.`usergelblacklist` (`userId`, `tag`) VALUES ('1', 'o');
			abisb.Append($"INSERT INTO `discord`.`usergelblacklist`(`userId`, `tag`) VALUES ('{did}', '{item}'); ");
			var e = await AbbysqlClient.RunSQL(abisb.ToString());
			return e > 0;
		}

		public static async Task<List<string>> GetBlackListTags(ulong id)
		{
			List<string> tags = new List<string>();

			var table = await AbbysqlClient.FetchSQL($"SELECT `tag` FROM `usergelblacklist` WHERE `userId` = '{id}';");
			foreach (AbbyRow row in table)
			{
				tags.Add((row["tag"] is string favchan) ? favchan : "");
			}
			return tags;
		}

		internal static async Task<bool> UnBlackListTag(ulong id, string item)
		{
			item = AbbySql.AbbysqlClient.EscapeString(item);
			var e = await AbbysqlClient.RunSQL($"DELETE FROM `discord`.`usergelblacklist` WHERE `userId` = '{id}' and `tag` = '{item}';");

			if (e < 1) throw new Exception("I failed to remove the tag from your list");
			return e > 0;
		}
	}
}