using Abbybot_III.Commands.Contains;

using AbbySql;
using AbbySql.Types;

using Capi.Interfaces;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abbybot_III.Sql.Abbybot.Abbybot
{
	class EggSql
	{
		public static async Task<List<Egg>> GetEggsAsync() {
			List<Egg> cmds = new List<Egg>();
			var table = await AbbysqlClient.FetchSQL("SELECT * FROM eggs");
			foreach (AbbyRow row in table)
			{
				ulong id = (ulong)row["Id"];
				string Word = (row["Word"] is string word) ? word : "";
				string Reply = (row["Reply"] is string reply) ? reply : "";
				int Min = (int)row["Min"];
				int Max = (int)row["Max"];

				Egg gc = new Egg(Min, Max, Word, Reply);
				gc.Rating = (CommandRatings)1;
				gc.Type = (CommandType)1;
				cmds.Add(gc);
			}
			return cmds;
		} 
	}
}
