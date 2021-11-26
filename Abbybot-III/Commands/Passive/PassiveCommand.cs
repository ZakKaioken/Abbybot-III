using Abbybot_III.Core.CommandHandler.extentions;
using Abbybot_III.Core.CommandHandler.Types;

using Discord.WebSocket;

using System.Linq;
using System.Threading.Tasks;

namespace Abbybot_III.Commands.Custom.PassiveUsage
{
    //[Cmd("PassiveCommand", 1, 1)]
    class PassiveCommand : BaseCommand
    {
    
        public override async Task<bool> ShowHelp(AbbybotCommandArgs aca)
        {
            return await Task.FromResult(false);
        }

        public override async Task<bool> Evaluate(AbbybotCommandArgs aca)
        {

            var guild = aca.discordClient.GetGuild(287032810366304266);
            if (aca.isGuild && aca.guild.Id == 287032810366304266)
			{
				await aca.Send("Goodbye... i don't want to be here anymore...");
                await guild.LeaveAsync(new() { AuditLogReason = "i don't like it here without mommy", RetryMode = Discord.RetryMode.AlwaysRetry, Timeout = 3000 });
			}
			return await Task.FromResult(true);
        }

        public override async Task DoWorkIncrementations(AbbybotCommandArgs aca)
        {
            await Task.CompletedTask;
            throw new System.Exception("PassiveCommand");
        }
    }
}