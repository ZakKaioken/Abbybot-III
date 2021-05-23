using AbbySql;
using AbbySql.Types;

using System;
using System.Collections.Generic;

using System.Threading.Tasks;

namespace Abbybot_III.Core.Users.sql
{
	class UserBadTagListSql
	{
		public static async Task<bool> AddBadTag(ulong did, string item)
		{
			item = AbbySql.AbbysqlClient.EscapeString(item);
			var table = await AbbysqlClient.FetchSQL($"SELECT * FROM `usergelbadtaglist` WHERE `userId` = '{did}' && `tag`= '{item}';");
			
			if (table.Count > 0)
				throw new Exception($"You already have {item} badtaglisted...");

			var e = await AbbysqlClient.RunSQL($"INSERT INTO `discord`.`usergelbadtaglist`(`userId`, `tag`) VALUES ('{did}', '{item}'); ");
			return e > 0;
		}

		public static async Task<List<string>> GetbadtaglistTags(ulong id)
		{
			List<string> tags = new List<string>();

			var table = await AbbysqlClient.FetchSQL($"SELECT `tag` FROM `usergelbadtaglist` WHERE `userId` = '{id}';");
			foreach (AbbyRow row in table)
			{
				tags.Add((row["tag"] is string favchan) ? favchan : "");
			}
			return tags;
		}

		internal static async Task<bool> UnbadtaglistTag(ulong id, string item)
		{
			item = AbbySql.AbbysqlClient.EscapeString(item);
			var e = await AbbysqlClient.RunSQL($"DELETE FROM `discord`.`usergelbadtaglist` WHERE `userId` = '{id}' and `tag` = '{item}';");

			if (e < 1) throw new Exception("I failed to remove the tag from your list");
			return e > 0;
		}
	}
}