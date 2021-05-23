using Abbybot_III.Core.CommandHandler.Types;
using Abbybot_III.Core.Guilds.sql;

using Capi;

using System.Threading.Tasks;

namespace Abbybot_III.Commands.Custom.PassiveUsage
{
    [Cmd("GuildnameUpdate", 1, 1)]
    class GuildnameUpdate : PassiveCommand
    {
        public override async Task DoWork(AbbybotCommandArgs aca)
        {
            if (aca.guild != null)
                await GuildSql.UpdateGuildName(aca.guild);
        }
    }
}