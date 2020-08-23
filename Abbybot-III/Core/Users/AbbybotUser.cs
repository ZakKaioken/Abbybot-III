using Abbybot_III.Apis.Discord.Events;
using Abbybot_III.Core.Data.User.Subsets;
using Abbybot_III.Core.Mysql;
using Abbybot_III.Core.Users.sql;

using Discord.WebSocket;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Abbybot_III.Core.Data.User
{
    public class AbbybotUser
    {
        public ulong Id;
        public UserGuild userGuild;
        public UserNames userNames;
        public UserPerms userPerms;
        public UserTwitter userTwitter;
        public UserFavoriteCharacter userFavoriteCharacter;
        public UserMarry userMarry;
        public UserTrust userTrust;
        public UserInterestingFacts userInterestingFacts;

        public AbbybotUser()
        {
            //Init(user).GetAwaiter().GetResult();
        }

        public static async Task<AbbybotUser> GetUserFromSocketGuildUser(SocketGuildUser sgu)
        {
            var abbybotuser = new AbbybotUser();
            await abbybotuser.Init(sgu);
            return abbybotuser;
        }

        public static async Task<AbbybotUser> GetUserFromSocketUser(SocketUser su)
        {
            var abbybotuser = new AbbybotUser();
            await abbybotuser.Init(su);
            return abbybotuser;
        }

        private async Task Init(SocketUser author)
        {
            
            if (author == null)
                throw new Exception("author is null");

            Id = author.Id;

            userNames = new UserNames
            {
                Username = author.Username
            };
            userPerms = new UserPerms();

            try {
            await GetGuild(author);
            }catch{ }

            var eeeer = (userGuild != null) ? userNames.Nickname : userNames.Username;
            userNames.PreferedName = eeeer;


            await UserSql.GetUser(this);
        }

        private async Task GetGuild(SocketUser author)
        {
            var sgu = author as SocketGuildUser;
            //Console.WriteLine(sgu.Guild.Name);
            if (sgu != null)
            {
                //Console.WriteLine("found guild");
                userGuild = new UserGuild();
                userGuild.GuildId = sgu.Guild.Id;
                userNames.Nickname = sgu.Nickname;
                userGuild.Roles = await RoleManager.GetUserRoles(sgu);
                userPerms.Ratings = (await RoleManager.GetRatings(userGuild.Roles)).ToList();
                //userPerms.Ratings.Add(Capi.Interfaces.CommandRatings.cutie);
            }
        }
    }
}
