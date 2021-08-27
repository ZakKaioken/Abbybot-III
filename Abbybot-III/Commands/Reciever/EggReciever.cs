using Abbybot_III.Commands.Contains;
using Abbybot_III.Sql.Abbybot.Abbybot;

using AbbySql;
using AbbySql.Types;

using Capi.Interfaces;

using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Abbybot_III.Commands.Reciever
{
	//[Capi.CmdReciever(true)]
	class EggReciever : Capi.Commands.CommandReciever.CmdReciever
	{
		public override async Task<List<iCommand>> RecieveCommands()
		{
			List<iCommand> cmds = new List<iCommand>();
			cmds.AddRange(await EggSql.GetEggsAsync());
			return cmds;
		}

	}
}