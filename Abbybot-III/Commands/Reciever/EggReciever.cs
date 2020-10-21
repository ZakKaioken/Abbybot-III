using Abbybot_III.Commands.Contains;

using AbbySql;
using AbbySql.Types;

using Capi.Interfaces;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Abbybot_III.Commands.Reciever
{
        [Capi.CmdReciever(true)]
        class EggReciever : Capi.Commands.CommandReciever.CmdReciever
        {
            public override async Task<List<iCommand>> RecieveCommands()
            {
                var sb = new StringBuilder();

                List<iCommand> cmds = new List<iCommand>();

                sb.Append("SELECT * FROM eggs");
                await ReadCommands(sb, cmds);

                return cmds;
            }

            private static async Task ReadCommands(StringBuilder sb, List<iCommand> cmds)
            {

                var table = await AbbysqlClient.FetchSQL(sb.ToString());
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
            }

        }
    
}
