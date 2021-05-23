using Abbybot_III.Core.Data.User;
using Abbybot_III.Core.Guilds;
using Abbybot_III.Core.Guilds.sql;

using Discord.WebSocket;

using System.Collections.Generic;
using System.Globalization;
using System.Linq;
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

		public SocketUser author;
		public ISocketMessageChannel channel;
		public SocketMessage originalMessage;
		public List<SocketUser> mentionedUserIds;
		public AbbybotGuild guild;
		public AbbybotUser user;
		public AbbybotUser sudoUser;

		string msg;

		public static async Task<AbbybotCommandArgs> MakeArgsFromMessage(SocketMessage sm)
		{
			var aca = new AbbybotCommandArgs();
			aca.Message = sm.Content;
			aca.author = sm.Author;
			aca.user = await AbbybotUser.GetUserFromSocketUser(sm.Author);
			aca.channel = sm.Channel;
			aca.originalMessage = sm;

			List<ulong> menids = new List<ulong>();
			foreach (var e in sm.MentionedUsers)
			{
				if (e.Id == Apis.Discord._client.CurrentUser.Id)
					aca.sudoUser = await AbbybotUser.GetUserFromSocketUser(e);
				menids.Add(e.Id);
			}
			aca.mentionedUserIds = sm.MentionedUsers.ToList();

			if (sm.Author is SocketGuildUser sgux)
			{
				aca.guild = new AbbybotGuild { Id = sgux.Guild.Id, Name = sgux.Guild.Name };
				await GuildSql.GetGuild(aca.guild);
			}
			return aca;
		}
	}
}