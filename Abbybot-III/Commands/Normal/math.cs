using Abbybot_III.Core.CommandHandler.extentions;
using Abbybot_III.Core.CommandHandler.Types;
using Abbybot_III.extentions;
using Abbybot_III.Sql.Abbybot.Fun;

using Discord;
using Discord.WebSocket;


using System.Text;
using System.Threading.Tasks;

namespace Abbybot_III.Commands.Normal
{
	[Capi.Cmd("abbybot math", 1, 1)]
	class math : Contains.ContainCommand
	{
		public override async Task DoWork(AbbybotCommandArgs a)
		{
			if (a.isMentioning) return;
			var FavoriteCharacter = a.Replace(Command).Replace("--debugmode", "");
			var ap = FavoriteCharacter. Split(" ");
			BigInteger sum = 0;
		
			for (int i = 0; i < ap.Length; i++) {
				if (ap[i] == "+") {
					BigInteger s = 0;
					BigInteger o = 0;
					try
					{
						s = new BigInteger(ap[i - 1], 10);
						sum +=(s);
					} catch { continue; }
					try
					{
						o = new BigInteger(ap[i + 1], 10);
						sum+=(o);
						}
					catch { continue; }
				}
			}
			await a.Send(sum);
		}

		public override async Task<string> toHelpString(AbbybotCommandArgs aca)
		{
			return $"send an anonymous dm to someone you mention";
		}
	}
}