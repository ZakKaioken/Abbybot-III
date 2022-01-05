using Abbybot_III.Core.AbbyBooru;
using Abbybot_III.Core.CommandHandler.extentions;
using Abbybot_III.Core.Data.User;
using Abbybot_III.Core.Guilds;
using Abbybot_III.Core.Guilds.sql;
using Abbybot_III.Core.Users.sql;
using Abbybot_III.Sql.Abbybot.Abbybot;
using Abbybot_III.Sql.Abbybot.User;

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
		public int commandsTested;
		public int commandsRun;
		public string Message
		{
			get => msg;
			set
			{
				msg = value.Replace("pussy", "usb c", true, CultureInfo.InvariantCulture).Replace("Pussy", "Usb c").Replace("PUSSY", "USB C").Replace("%", "abbybot ");
			}
		}
		public string prefix => pre;
		string pre;
		public SocketUser author;
		public ISocketMessageChannel channel;
		public SocketGuild server;
		public SocketMessage originalMessage;
		public List<SocketUser> mentionedUsers;
		public bool hasMultipleFcs => this.GetFCList().Length >1;
		public AbbybotGuild guild;
		public AbbybotUser user;
		public AbbybotUser sudoUser;
		public DiscordSocketClient discordClient;
		public ulong abbybotId;
		public bool isGuild;
		public Random random;
		public bool isMentioning {
			get => mentionedUsers.Count > 0;
		}
		public bool IsChannelNSFW {
			get => (channel is ITextChannel chch) && chch.IsNsfw;
		}
		
		string msg;
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

    }
}