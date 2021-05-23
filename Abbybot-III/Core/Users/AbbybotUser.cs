
using Abbybot_III.Core.Mysql;
using Abbybot_III.Core.Users.sql;

using Capi.Interfaces;

using Discord.WebSocket;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Abbybot_III.Core.Data.User
{
	public class AbbybotUser
	{
		public ulong Id;
		public bool isNsfw;
		public bool isGuild;
		public ulong GuildId;
		public string Username;
		public bool admin;
		public List<AbbybotRole> Roles;

		public string Nickname = "";
		public string Preferedname = "";
		public List<CommandRatings> Ratings = new List<CommandRatings>();

		public string Twitter;
		public string TwitterChannel;
		public int TweetCount;

		public string FavoriteCharacter;
		public bool IsLewd = true;
		public ulong MarriedUserId;

		public int ActivityLevel;
		public int Love;

		public bool inTimeOut;
		public DateTime TimeOutEndDate;
		public string timeoutReason;

		public int MessagesSent;
		public int CommandsSent;

		public AbbybotUser()
		{
			//Init(user).GetAwaiter().GetResult();
		}

		public static async Task<AbbybotUser> GetUserFromSocketGuildUser(ulong guildId, ulong sguId)
		{
			var sgu = Apis.Discord.__client.GetGuild(guildId).GetUser(sguId);
			var abbybotuser = new AbbybotUser();
			await abbybotuser.Init(sgu);
			return abbybotuser;
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
		public bool HasRatings(int i) {
			return HasRatings((CommandRatings)i);
		}
		public bool HasRatings(CommandRatings rating) {
			return Ratings.Contains(rating);
		}
		async Task Init(SocketUser author)
		{
			if (author == null)
				throw new Exception("author is null");

			Id = author.Id;

			Username = author.Username;

			//Abbybot.print(sgu.Guild.Name);
			if (author is SocketGuildUser sgu)
			{
				Console.WriteLine("author is guild user");
				isGuild = true;
				GuildId = sgu.Guild.Id;

				Nickname = sgu.Nickname;
				var err = Apis.Discord.__client.GetGuild(sgu.Guild.Id).GetUser(sgu.Id);
				Roles = await RoleManager.GetUserRoles(err);
				admin = sgu.Roles.ToList().Any(rs => rs.Permissions.Administrator);
				Ratings = (await RoleManager.GetRatings(Roles)).ToList(); 
			}
			var eeeer = (isGuild && Nickname != null) ? Nickname : Username;
			Preferedname = Regex.Replace(eeeer.ToString(),
				@"([(\u2100-\u27ff)(\uD83C\uDC00 - \uD83C\uDFFF)(\uD83D\uDC00 - \uD83D\uDFFF)(\uD83E\uDD00 - \uD83E\uDFFF)])",
				@"\$1").Replace("\\ ", " ");

			await UserTrustSql.GetUserTimeout(Id);

			var u = await UserSql.GetUserData(Id);
			FavoriteCharacter = u.favoritecharacter;
			MarriedUserId = u.marriedid;

			IsLewd = u.isLewd;
		}

		public static object GetUserFromTwitterUser(string screenName)
		{
			throw new NotImplementedException();
		}

	}
}