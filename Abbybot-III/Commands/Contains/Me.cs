using Abbybot_III.Core.CommandHandler.extentions;
using Abbybot_III.Core.CommandHandler.Types;
using Abbybot_III.Core.Data.User;
using Abbybot_III.Core.LevelingManager;
using Abbybot_III.Core.Users.sql;
using Abbybot_III.Sql.Abbybot.User;

using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abbybot_III.Commands.Contains
{
	[Capi.Cmd("abbybot me", 1, 1)]
	class Me : ContainCommand
	{
		public override bool SelfRun { get => false; set => base.SelfRun = value; }

		public override async Task DoWork(AbbybotCommandArgs abd)
		{
			StringBuilder sb = new StringBuilder();

			sb.AppendLine($"**{abd.user.Preferedname}**'s profile:");
			sb.Append($"Username: {abd.user.Username}");
			if (abd.user.Nickname != null)
				if (abd.user.Nickname.Length > 0)
					sb.Append($", Nickname: {abd.user.Nickname}");
			sb.Append("\n");
			sb.AppendLine($"Your favorite character is: {abd.user.FavoriteCharacter}");
			if (abd.user.MarriedUserId != 0)
			{
				var married = await AbbybotUser.GetUserFromSocketGuildUser(abd.guild.Id, abd.user.Id);
				sb.AppendLine($"You're married to {married.Preferedname}");
			}
			else
			{
				sb.AppendLine("You're not married to anyone.");
			}
			sb.Append("AutoFcDms: ");
			sb.Append((await abd.GetAutoFcDms())? "✅ ":"❌ ");
			sb.Append("FCMentions: ");
			sb.Append((await abd.GetFCMentions())? "✅ ":"❌ ");


			sb.Append("\n");
			if (abd.guild != null)
			{
				sb.Append("Your favorite channel in this server is: ");

				var MSC = await abd.GetPassiveStat("MessagesSent");
				var orderedlist = MSC.OrderBy(x => x.stat).ToList()[0];
				var chan = abd.GetGuildChannel(abd.guild.Id, orderedlist.channel);
				sb.AppendLine(chan.Name);

				ulong i = 0;
				foreach (var sta in MSC)
				{
					i += sta.stat;
				}
				var e = LevelCalculator.CalculateStatLevel(i, "Messagessent");

				sb.AppendLine($"You are level {e.level}. ({e.exp}/{e.expleft})");
			}

			sb.AppendLine($"You have sent {abd.user.MessagesSent} messages and {abd.user.CommandsSent} commands");

			await abd.Send(sb.ToString());
		}

		public override async Task<bool> ShowHelp(AbbybotCommandArgs aca)
		{
			return true;
		}

		public override async Task<string> toHelpString(AbbybotCommandArgs aca)
		{
			return $"Check your profile!!";
		}
	}
}