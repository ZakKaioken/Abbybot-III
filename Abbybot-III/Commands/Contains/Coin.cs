using Abbybot_III.Core.CommandHandler.extentions;
using Abbybot_III.Core.CommandHandler.Types;
using Abbybot_III.Core.LevelingManager;
using Abbybot_III.Sql.Abbybot.User;

using System;
using System.Text;
using System.Threading.Tasks;

namespace Abbybot_III.Commands.Contains
{
	[Capi.Cmd("abbybot coin", 1, 1)]
	class coin : ContainCommand
	{
		public override bool SelfRun { get => true; set => base.SelfRun = value; }

		public override async Task DoWork(AbbybotCommandArgs abd)
		{
			StringBuilder sb = new StringBuilder();
			Random r = new Random();
			var heads = "heads";
			var tails = "tails";
			var hot = abd.Message.Replace($"{Command} ", "").Split(" or ");
			if (hot.Length >= 2)
			{
				heads = hot[0];
				tails = hot[1];
			}
			string coin = r.Next(0, 100 + 1) <= 50 ? heads : tails;
			sb.Append("You got **").Append(coin).Append("**!");
			await abd.Send(sb.ToString());
		}

		public override async Task<bool> ShowHelp(AbbybotCommandArgs aca)
		{
			return true;
		}

		public override async Task<string> toHelpString(AbbybotCommandArgs aca)
		{
			return $"Flip a coin!";
		}
	}
}