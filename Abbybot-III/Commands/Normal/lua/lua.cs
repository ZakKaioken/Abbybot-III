using Abbybot_III.Core.CommandHandler.extentions;
using Abbybot_III.Core.CommandHandler.Types;
using Abbybot_III.Sql.Abbybot.Fun;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abbybot_III.Commands.Normal.lua
{
	[Capi.Cmd("abbybot lua", 1,1)]
	class lua : NormalCommand
	{
		public override async Task DoWork(AbbybotCommandArgs e)
		{
			var cmd = e.ReplaceSplit(Command, " ");
			var sb = new StringBuilder("```log\n");
			foreach (var i in cmd)
			{
				sb.AppendLine(i);
			}
			sb.Append("```");
			if (cmd.Length > 0)
				await (cmd[0] switch 
				{ 
					"view" => View(e),
					"clear" => Clear(e),
					_ => Task.CompletedTask 
				});
		}

		public async Task Clear(AbbybotCommandArgs e) {
			//await View(e);
			await LuaSql.ClearLuaData(e.user.Id);
			await e.Send("ok master I cleared your script...");
		}
		public async Task View(AbbybotCommandArgs aca) {
			var luadata = await LuaSql.GetLuaData(aca.user.Id);
			StringBuilder sb = new("```lua\n");
			List<string> strings = new List<string>();
			luadata.LuaPieces.ForEach(piece => strings.AddRange(piece.Split("\n").Where(p => p.Length > 0)));
			if (strings.Count < 1)
			{
				await aca.Send("m-master... you don't have any lua history");
				return;
			}
			bool dms = strings.Count> 10;
			for (int i = 1; i <= strings.Count; i++)
			{
				sb.Append($"{i}. ").AppendLine(strings[i-1]);
			}
			sb.Append("```");
			if (dms)
			{
				await aca.Send("Master... Since your script is more than 10 lines long I'll send it to the dms.");
				var longer = sb.Length > 1000;
				await aca.SendDM(sb);
			} else 
				await	aca.Send(sb);

		}
	}
}
