using Abbybot_III.Core.Data.User;
using Abbybot_III.Core.Data.User.Subsets;

using AbbySql;
using AbbySql.Types;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Abbybot_III.Core.Users.sql
{
    class UserSql
    {
        public static async Task<AbbybotUser> GetUser(ulong Id)
        {

            var table = await AbbysqlClient.FetchSQL($"SELECT `Id` FROM `users` WHERE `Id` = '{Id}';");
            if (table.Count < 1)
                await AbbysqlClient.RunSQL($"INSERT INTO `discord`.`users`(Id, FavoriteCharacter) VALUES ('{Id}','Abigail_Williams*');");

            table = await AbbysqlClient.FetchSQL($"SELECT users.* FROM `users` WHERE users.Id = {Id};");
            
            AbbyRow row = table[0];
            var user = new AbbybotUser();
            user.userFavoriteCharacter = new UserFavoriteCharacter
            {
                FavoriteCharacter = (row["FavoriteCharacter"] is string s) ? s : "abigail_williams*",
                IsLewd = (sbyte)row["IsLewd"] == 1 ? true : false
            };

            user.userMarry = new UserMarry
            {
                MarriedUserId = (ulong)row["MarriedUserId"]
            };

            user.userInterestingFacts = new UserInterestingFacts
            {
                CommandsSent = (int)row["CommandsSent"],
                MessagesSent = (int)row["MessagesSent"]
            };

            user.userTrust = new UserTrust
            {
                ActivityLevel = (int)row["LonlinessRating"]
            };

            /* user.userTwitter = new UserTwitter
             {
                  Twitter = (row["Twitter"] is string twitter) ? twitter : "",
                  TwitterChannel = (row["Channel"] is string chan) ? chan : "",
                  TweetCount = (row["TweetCount"] is int twwc) ? twwc : 0
             };*/
            return user;
        }
        static async Task AddUser(ulong Id)
        {
            }
    }
}
