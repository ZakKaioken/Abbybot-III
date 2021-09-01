using AbbySql;
using AbbySql.Types;

using System;
using System.Collections.Generic;

using System.Threading.Tasks;

namespace Abbybot_III.Core.Users.sql
{
	class UserBadTagListSql
	{
		public static async Task AddBadTag(ulong did, string item, Action onSuccess = null, Action<string> onFail = null)
		{
			item = AbbysqlClient.EscapeString(item);
			var table = await AbbysqlClient.FetchSQL($"SELECT * FROM `user`.`gelbadtaglist` WHERE `UserId` = '{did}' && `Tag`= '{item}';");
			
			if (table.Count > 0) {
				onFail?.Invoke($"Sorry master... {item} was already added...");
				return;
			}

			var e = await AbbysqlClient.RunSQL($"INSERT INTO `user`.`gelbadtaglist`(`UserId`, `Tag`) VALUES ('{did}', '{item}'); ");
			if (e > 0) onSuccess?.Invoke();
			else onFail?.Invoke("I... Couldn't add the tag to the database...");
			}

		public static async Task<List<string>> GetbadtaglistTags(ulong id)
		{
			List<string> tags = new List<string>();

			var table = await AbbysqlClient.FetchSQL($"SELECT `Tag` FROM `user`.`gelbadtaglist` WHERE `UserId` = '{id}';");
			foreach (AbbyRow row in table)
			{
				tags.Add((row["Tag"] is string favchan) ? favchan : "");
			}
			return tags;
		}

		internal static async Task<bool> UnbadtaglistTag(ulong id, string item)
		{
			item = AbbysqlClient.EscapeString(item);
			var e = await AbbysqlClient.RunSQL($"DELETE FROM `user`.`gelbadtaglist` WHERE `UserId` = '{id}' and `Tag` = '{item}';");

			if (e < 1) throw new Exception("I failed to remove the tag from your list");
			return e > 0;
		}
	}
}