using Abbybot_III.Core.CommandHandler.extentions;
using Abbybot_III.Core.CommandHandler.Types;
using Abbybot_III.Sql.Abbybot.Fun;

using Discord.WebSocket;

using System.Text;
using System.Threading.Tasks;

namespace Abbybot_III.Commands.Normal
{
	[Capi.Cmd("abbybot say", 1, 1)]
	class Say : Contains.ContainCommand
	{
		public override async Task DoWork(AbbybotCommandArgs a)
		{
			StringBuilder FavoriteCharacter = new StringBuilder(a.Message).Replace(Command, "").Replace("--debugmode", "");
			while (FavoriteCharacter[0] == ' ')
				FavoriteCharacter.Remove(0, 1);

			var o = await SaybadtaglistSql.GetbadtaglistTags();
			foreach (var oo in o)
			{
				FavoriteCharacter.Replace(oo, "");
			}

			if (!(a.channel is SocketDMChannel))
				await a.Delete();
			Abbybot.print("tried to run say");
			await a.Send(FavoriteCharacter);
		}

		public override async Task<string> toHelpString(AbbybotCommandArgs aca)
		{
			return await Task.FromResult($"make me say something!!!");
		}
	}
}