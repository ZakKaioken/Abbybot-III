using Abbybot_III.Core.CommandHandler.extentions;
using Abbybot_III.Core.CommandHandler.Types;

using AbbySql.Types;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abbybot_III.Commands.Contains
{
[Capi.Cmd("abbybot joke",1,1)]
	class Joke :ContainCommand
	{
		public override async Task DoWork(AbbybotCommandArgs aca)
		{
			AbbyTable table = await AbbySql.AbbysqlClient.FetchSQL("select * from jokes");
			if (table.Count == 0) return;
			
				int i = aca.AbbyRngRoll(0, table.Count);

			await aca.Send(table[i]["Joke"] is string joke ? joke : "");
			await Task.Delay(1000);
			await aca.Send(table[i]["punchline"] is string pl ? pl : "");
			

		}
	}
}
