using Abbybot_III.Commands.Contains.Gelbooru;

using AbbySql;
using AbbySql.Types;

using Capi.Commands.CommandReciever;
using Capi.Interfaces;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace Abbybot_III.Commands.Reciever
{
	//[Capi.CmdReciever(true)]
	class GelbooruCommandReciever : CmdReciever
	{
		List<iCommand> cmds = new List<iCommand>();

		public override async Task<List<iCommand>> RecieveCommands()
		{
			List<iCommand> cmds = new List<iCommand>();

			var table = await AbbysqlClient.FetchSQL("SELECT * FROM gelboorucommands");
			foreach (AbbyRow row in table)
			{
				ulong id = (ulong)row["Id"];
				string Comm = (row["Command"] is string comman) ? comman : "";
				string Tags = (row["Tags"] is string tags) ? tags : "";
				int RatingId = (int)row["RatingId"];
				NewGelbooruCommand gc = new NewGelbooruCommand($"abbybot {Comm}", Tags.Split(' '), (CommandRatings)RatingId);
				cmds.Add(gc);
			}

			return cmds;
		}
	}
}