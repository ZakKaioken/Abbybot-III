using Abbybot_III.Core.CommandHandler.Types;
using Abbybot_III.Core.Guilds.sql;
using Abbybot_III.Sql.Abbybot.User;

using Capi;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abbybot_III.Commands.Custom.PassiveUsage
{
    [Cmd("GuildnameUpdate", 1, 1)]
    class GuildnameUpdate : PassiveCommand
    {
        public override async Task DoWork(AbbybotCommandArgs aca)
        {
            if (aca.abbybotGuild != null)
            {
               if (!aca.abbybotGuild.AbbybotIsHere)
                await GuildSql.UpdateGuildName(aca.abbybotGuild);
            }
        }
    }
}
