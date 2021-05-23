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
			var table = await AbbysqlClient.FetchSQL($"SELECT `Id` FROM `users` WHERE `Id` = '{Id}';");
			if (table.Count < 1)
				await AbbysqlClient.RunSQL($"INSERT INTO `discord`.`users`(Id, FavoriteCharacter) VALUES ('{Id}','Abigail_Williams*');");

			table = await AbbysqlClient.FetchSQL($"SELECT users.* FROM `users` WHERE users.Id = {Id};");

			AbbyRow row = table[0];

			(string favoritecharacter, bool isLewd, ulong marriedid) gol = ("Abigail_Williams*", true, 0);

			gol.favoritecharacter = (row["FavoriteCharacter"] is string s) ? s : "abigail_williams*";
			gol.isLewd = (sbyte)row["IsLewd"] == 1;
			gol.marriedid = (ulong)row["MarriedUserId"];

			return gol;
		}
	}
}