using Abbybot_III.Core.CommandHandler.extentions;
using Abbybot_III.Core.CommandHandler.Types;
using Abbybot_III.Core.LevelingManager;
using Abbybot_III.extentions;
using Abbybot_III.Sql.Abbybot.User;

using System;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Abbybot_III.Commands.Contains
{
	[Capi.Cmd("d", 1, 1)]
	class roll : BaseCommand
	{
		public override bool SelfRun { get => true; set => base.SelfRun = value; }

		Random r = new Random();

		public override async Task DoWork(AbbybotCommandArgs abd)
		{
			StringBuilder sb = new StringBuilder();
			int d = 6;
			var i = abd.Split(" ", true);
			bool def = true;
			foreach (var a in i)
			{
				int count = 1;
				if (!a.Contains("d")) continue;
				var b = a.Split("d");

				try
				{
					count = int.Parse(b[0]);
				}
				catch { }
				BigInteger sum = 0;
				for (int z = 0; z < count; z++)
				{
					try
					{
						int parsed = int.Parse(b[1]);
						int coin = r.Next(0, parsed) + 1;
						if (count <= 1)
							sb.Append("**d").Append(parsed).Append("**: **").Append(coin).Append("**! ");
						sum += coin;
						def = false;
					}
					catch
					{
					}
				}
				sb.Append("**").Append(count).Append('d').Append(b[1]).Append("**: **").Append(sum).Append("**! ");
			}

			if (def)
			{
				int coin = r.Next(0, d) + 1;

				sb.Append("You rolled a **d").Append(d).Append("** and got a **").Append(coin).Append("**! \n");
			}

			await abd.Send(sb.ToString());
		}

		public override async Task<bool> Evaluate(AbbybotCommandArgs aca)
		{
			bool go = false;
			var i = aca.Message.ToLower().Split(" ");
			foreach (var a in i)
			{
				if (!a.Contains('d')) continue;
				var b = a.Split("d");
				try
				{
					int par2 = int.Parse(b[1]);
					if (par2 > 0) go = true;
				}
				catch { }
			}
			bool ev = await base.Evaluate(aca) && go;
			if (ev) Multithreaded = true;
			return ev;
		}

		public override async Task<bool> ShowHelp(AbbybotCommandArgs aca)
		{
			return true;
		}

		public override async Task<string> toHelpString(AbbybotCommandArgs aca)
		{
			return $"roll a d6, or any combination of the command you'd like 2d8, 4d6, 24d76!";
		}
	}
}