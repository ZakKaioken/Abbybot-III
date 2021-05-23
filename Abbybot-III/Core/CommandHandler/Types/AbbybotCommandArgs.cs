using Abbybot_III.Core.AbbyBooru;
using Abbybot_III.Core.Data.User;
using Abbybot_III.Core.Guilds;
using Abbybot_III.Core.Guilds.sql;
using Abbybot_III.Sql.Abbybot.Abbybot;
using Abbybot_III.Sql.Abbybot.User;
using BooruSharp.Search.Post;
using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abbybot_III.Core.CommandHandler.Types
{
	public class AbbybotCommandArgs : Capi.Interfaces.iMsgData
	{
		public string Message
		{
			get
			{
				return msg;
			}
			set
			{
				msg = value.Replace("pussy", "usb c", true, CultureInfo.InvariantCulture).Replace("Pussy", "Usb c").Replace("PUSSY", "USB C").Replace("%", "abbybot ");
			}
		}
		public string prefix {get => pre; set=>pre = value;}
		string pre;
		public string[] Split(string split, bool lowercase=false) {
			if (lowercase) return Message.ToLower().Split(split.ToLower());
			else return Message.Split(split);
		}

		public bool Contains(string item, bool lowercase = false) {
			if (lowercase)
				return Message.ToLower().Contains(item.ToLower(), System.StringComparison.InvariantCultureIgnoreCase);
			else 
				return Message.Contains(item);
			 
		}

		public StringBuilder Replace(string item, string replacement, bool lowercase = false) {
			StringBuilder sb;
			if (lowercase) sb = new StringBuilder(Message.ToLower()).Replace(msg.ToLower(), ""); 
			else sb = new StringBuilder(Message).Replace(msg, "");
			while (sb[0] == ' ')
				sb.Remove(0, 1);
			while (sb.ToString().Contains("  "))
				sb.Replace("  ", " ");
			while (sb[^1] == ' ')
				sb.Remove(sb.Length - 1, 1);
			return sb;
		}
		public StringBuilder Replace(string item, bool lowercase=false ) {
			StringBuilder sb;
			if (lowercase) sb = new StringBuilder(Message.ToLower()).Replace(item.ToLower(), ""); 
			else sb = new StringBuilder(Message).Replace(item, "");

			if (sb.Length > 0) {
			while (sb[0] == ' ')
				sb.Remove(0, 1);
			while (sb.ToString().Contains("  "))
				sb.Replace("  ", " ");
			while (sb[^1] == ' ')
				sb.Remove(sb.Length - 1, 1);
			}
			return sb;
		}

		public string[] GetFCList() {
			return user.FavoriteCharacter.Replace("{", "").Replace("}", "").Split(" ~ ");
		}

		public string BuildAbbybooruTag(string english) {
			return AbbybooruTagGenerator.FCBuilder(english);
		}
		public StringBuilder BuildAbbybooruTag(StringBuilder english) {
			return AbbybooruTagGenerator.FCBuilder(english);
		}

		public string BreakAbbybooruTag(string s)
		{
			return BreakAbbybooruTag(new StringBuilder(s)).ToString();
		}
		public StringBuilder BreakAbbybooruTag(StringBuilder s)
		{
			return s.Replace("* ~ ", " or ").Replace("* ", " and ").Replace("{", "").Replace("}", "").Replace("_", " ").Replace("*", "");
		}
		public string[] ReplaceSplit(string item, string replacement, string split) {
			return Replace(item, replacement).ToString().Split(split);
		}
		public string[] ReplaceSplit(string item, string split) {
			return Replace(item, "").ToString().Split(split);
		}
		public SocketUser author;
		public ISocketMessageChannel channel;
		public SocketGuild server;
		public SocketMessage originalMessage;
		public List<SocketUser> mentionedUsers;
		public bool hasMultipleFcs => GetFCList().Length >1;
		public async Task<List<AbbybotUser>> getMentionedAbbybotUsers() {
			List<AbbybotUser> us = new List<AbbybotUser>();
			foreach (var item in mentionedUsers)
			{
				us.Add(await AbbybotUser.GetUserFromSocketUser(item));
			}
			return us;
		}

		public List<SocketGuildUser> getMentionedDiscordGuildUsers() {
			List<SocketGuildUser> us = new List<SocketGuildUser>();
			foreach (var item in mentionedUsers)
			{
				us.Add(GetGuildUser(guild.Id, item.Id));
			}
			return us;
		}
		public AbbybotGuild guild;
		public AbbybotUser user;
		public AbbybotUser sudoUser;
		public DiscordSocketClient discordClient;
		public ulong abbybotId;
		public bool isGuild;
		public bool isMentioning {
			get => mentionedUsers.Count > 0;
		}
		public bool IsChannelNSFW {
			get => (channel is ITextChannel chch) ? chch.IsNsfw : false;
		}
		
		string msg;
		public SocketGuild GetGuild(ulong guildId) {
			return discordClient.GetGuild(guildId);
		}

		public async Task<List<(ulong channel, ulong stat)>> IncreasePassiveStat(string stat) {
			List<(ulong channel, ulong stat)> osi = null;
			try {
			await PassiveUserSql.IncreaseStat(abbybotId, guild?.Id != null ? guild.Id:0, channel.Id, user.Id, stat);
			 osi = await PassiveUserSql.GetChannelsinGuildStats(abbybotId, guild?.Id != null ? guild.Id:0, user.Id, "GelCommandUsages");
			} catch {}
			return osi;
		}
		public async Task<SearchResult> GetPicture(string[] tags)
		{
			return await Apis.AbbyBooru.Execute(tags);
		}
		public async Task<SearchResult> GetPicture(List<string> tags)
		{
			return await Apis.AbbyBooru.Execute(tags.ToArray());
		}
		public async Task<bool> IsAbbybotHere () {
			var abbybotids = await AbbybotSql.GetAbbybotIdAsync();
            abbybotids.Remove(abbybotId);
            bool b = false;
            foreach (var o in abbybotids)
            {
                var u = GetUser(o);
                b = u.MutualGuilds.ToList().Any(x => x.Id == guild.Id);
            }
            return b;
		}
		public SocketGuildUser GetGuildUser(ulong guildId,ulong userId) {
			return discordClient.GetGuild(guildId).GetUser(userId);
		}
		public SocketGuildChannel GetGuildChannel(ulong guildId, ulong channelId) {
			return discordClient.GetGuild(guildId).GetChannel(channelId);
		}
		public SocketUser GetUser(ulong userId) {
			return discordClient.GetUser(userId);
		}
		public static async Task<AbbybotCommandArgs> MakeArgsFromMessage(SocketMessage sm)
		{
			var aca = new AbbybotCommandArgs();
			aca.discordClient = Apis.Discord.__client;
			aca.abbybotId = Apis.Discord.__client.CurrentUser.Id;
			aca.Message = sm.Content;
			aca.author = sm.Author;
			aca.user = await AbbybotUser.GetUserFromSocketUser(sm.Author);
			aca.channel = sm.Channel;
			aca.originalMessage = sm;

			List<ulong> menids = new List<ulong>();
			foreach (var e in sm.MentionedUsers)
			{
				if (e.Id == aca.abbybotId)
					aca.sudoUser = await AbbybotUser.GetUserFromSocketUser(e);
				menids.Add(e.Id);
			}
			aca.mentionedUsers = sm.MentionedUsers.ToList();
			aca.isGuild= sm.Author is SocketGuildUser;
			if (sm.Author is SocketGuildUser sgux)
			{
				aca.guild = new AbbybotGuild { 
					Id = sgux.Guild.Id, 
					Name = sgux.Guild.Name 
				};
				await GuildSql.GetGuild(aca.guild);
			}
			return aca;
		}

        public string InvertName(string v)
        {
           return AbbybooruTagGenerator.InvertName(v);
        }
    }
}