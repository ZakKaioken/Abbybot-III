using AbbySql;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Abbybot_III.Core.Users.sql
{
    class FavoriteCharacterSql
    {
        public async Task SetFavoriteCharacterAsync(ulong userId, string favoriteCharacter)
        {
            await AbbysqlClient.RunSQL($"UPDATE `discord`.`users` SET `FavoriteCharacter`= '{favoriteCharacter}' WHERE  `Id`= {userId};");
        }

    }
}
