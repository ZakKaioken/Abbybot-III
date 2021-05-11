using AbbySql;

using System.Threading.Tasks;

namespace Abbybot_III.Core.Users.sql
{
	class FavoriteCharacterSql
	{
		public static async Task SetFavoriteCharacterAsync(ulong userId, string favoriteCharacter)
		{
			string fc = AbbysqlClient.EscapeString(favoriteCharacter);
			await AbbysqlClient.RunSQL($"UPDATE `discord`.`users` SET `FavoriteCharacter`= '{fc}' WHERE  `Id`= {userId};");
		}
	}
}