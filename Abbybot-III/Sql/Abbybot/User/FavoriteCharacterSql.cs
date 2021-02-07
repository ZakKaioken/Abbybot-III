using AbbySql;

using System.Threading.Tasks;

namespace Abbybot_III.Core.Users.sql
{
    class FavoriteCharacterSql
    {
        public static async Task SetFavoriteCharacterAsync(ulong userId, string favoriteCharacter)
        {
            await AbbysqlClient.RunSQL($"UPDATE `discord`.`users` SET `FavoriteCharacter`= '{favoriteCharacter}' WHERE  `Id`= {userId};");
        }
    }
}