using Abbybot_III.Core.CommandHandler.extentions;
using Abbybot_III.Core.CommandHandler.Types;
using Abbybot_III.Core.LevelingManager;
using Abbybot_III.extentions;
using Abbybot_III.Sql.Abbybot.Fun;
using Abbybot_III.Sql.Abbybot.User;

using Discord;

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
			bool side = r.Next(0, 100 + 1) <= 50;
			string coin = side ? heads : tails;

			var eb = new EmbedBuilder();

			string[] headsimgs = new string[] {
			"https://i.imgur.com/lXufTSD.gif",
			"https://i.imgur.com/AXoBzL0.gif"
			};
			string[] tailssimgs = new string[] {
			"https://i.imgur.com/KzL88HB.gif",
			"https://i.imgur.com/MNJX8oi.gif"
			};
			eb.ImageUrl = side ? headsimgs.random() : tailssimgs.random();

			eb.Title =await CoinMessageSql.GetMessage(false);
			sb.Append("You got **").Append(coin).Append("**!");
			eb.Color = Color.LightOrange;
			await abd.Send(eb);
			await Task.Delay(1000);
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