using Abbybot_III.Core.CommandHandler.extentions;
using Abbybot_III.Core.CommandHandler.Types;
using Abbybot_III.Core.LevelingManager;
using Abbybot_III.Sql.Abbybot.User;

using System.Text;
using System.Threading.Tasks;

namespace Abbybot_III.Commands.Contains
{
	[Capi.Cmd("abbybot level", 1, 1)]
	class level : ContainCommand
	{
		public override bool SelfRun { get => true; set => base.SelfRun = value; }

		public override async Task DoWork(AbbybotCommandArgs abd)
		{
			StringBuilder sb = new StringBuilder();
			var client = Apis.Discord._client;
			var abbybotid = client.CurrentUser.Id;
			ulong guildid = 0;
			if (abd.guild != null)
			{
				guildid = abd.guild.Id;
			}

			var MSC = await PassiveUserSql.GetChannelsinGuildStats(abbybotid, guildid, abd.user.Id, "MessagesSent");

			ulong i = 0;
			foreach (var sta in MSC)
			{
				i += sta.stat;
			}
			var e = LevelCalculator.CalculateStatLevel(i, "MessagesSent");

			sb.AppendLine($"You are level {e.level}. ({e.exp}/{e.expleft})");

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