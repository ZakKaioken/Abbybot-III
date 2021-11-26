using Abbybot_III.Core.Data.User;

using AbbySql;
using AbbySql.Types;

using System.Threading.Tasks;

namespace Abbybot_III.Core.Users.sql
{
	class UserSql
	{
		public static async Task<(string favoritecharacter, bool isLewd, ulong marriedid)> GetUserData(ulong Id)
		{
			var table = await AbbysqlClient.FetchSQL($"SELECT `UserId` FROM `user`.`users` WHERE `UserId` = '{Id}';");
			if (table.Count < 1)
				await AbbysqlClient.RunSQL($"INSERT INTO `user`.`users`(UserId, FavoriteCharacter) VALUES ('{Id}','Abigail_Williams*');");

			table = await AbbysqlClient.FetchSQL($"SELECT `user`.`users`.* FROM `user`.`users` WHERE users.UserId = {Id};");

			AbbyRow row = table[0];

			(string favoritecharacter, bool isLewd, ulong marriedid) gol = ("Abigail_Williams*", true, 0);

			gol.favoritecharacter = (row["FavoriteCharacter"] is string s) ? s : "abigail_williams*";
			gol.isLewd = row["IsLewd"] is ulong il && il == 1;
			gol.marriedid = row["MarriedUserId"] is ulong mid ? mid : 0;

			return gol;
		}
	}
}