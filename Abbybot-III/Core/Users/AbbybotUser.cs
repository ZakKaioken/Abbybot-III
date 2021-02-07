using Abbybot_III.Core.Data.User.Subsets;
using Abbybot_III.Core.Mysql;
using Abbybot_III.Core.Users.sql;

using Discord.WebSocket;

using System;
using System.Linq;
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

        async Task Init(SocketUser author)
        {
            if (author == null)
                throw new Exception("author is null");

            Id = author.Id;

            userNames = new UserNames
            {
                Username = author.Username
            };
            userPerms = new UserPerms();

            try
            {
                await GetGuild(author);
            }
            catch { }

            var eeeer = (userGuild != null && userNames.Nickname != null) ? userNames.Nickname : userNames.Username;

            var eex = Regex.Replace(eeeer.ToString(),
                @"([(\u2100-\u27ff)(\uD83C\uDC00 - \uD83C\uDFFF)(\uD83D\uDC00 - \uD83D\uDFFF)(\uD83E\uDD00 - \uD83E\uDFFF)])",
                @"\$1").Replace("\\ ", " ");

            userNames.PreferedName = eex;
            userTrust = new UserTrust();
            await UserTrustSql.GetUserTimeout(this);

            var u = await UserSql.GetUser(Id);
            userFavoriteCharacter = u.userFavoriteCharacter;
            userMarry = u.userMarry;
            userInterestingFacts = u.userInterestingFacts;
            userTrust = u.userTrust;
        }

        public static object GetUserFromTwitterUser(string screenName)
        {
            throw new NotImplementedException();
        }

        async Task GetGuild(SocketUser author)
        {
            var sgu = author as SocketGuildUser;
            //Abbybot.print(sgu.Guild.Name);
            if (sgu != null)
            {
                //Abbybot.print("found guild");
                userGuild = new UserGuild();
                userGuild.GuildId = sgu.Guild.Id;
                userNames.Nickname = sgu.Nickname;
                userGuild.Roles = await RoleManager.GetUserRoles(sgu);
                userGuild.admin = sgu.Roles.ToList().Any(rs => rs.Permissions.Administrator);
                userPerms.Ratings = (await RoleManager.GetRatings(userGuild.Roles)).ToList();
                //userPerms.Ratings.Add(Capi.Interfaces.CommandRatings.cutie);
            }
        }
    }
}