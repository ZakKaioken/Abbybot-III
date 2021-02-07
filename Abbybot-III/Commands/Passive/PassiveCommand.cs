using Abbybot_III.Core.CommandHandler.Types;

using System.Threading.Tasks;

namespace Abbybot_III.Commands.Custom.PassiveUsage
{
    //[Cmd("PassiveCommand", 1, 1)]
    class PassiveCommand : BaseCommand
    {
        public override async Task<bool> ShowHelp(AbbybotCommandArgs aca)
        {
            return false;
        }

        public override async Task<bool> Evaluate(AbbybotCommandArgs aca)
        {
            return true;
        }
    }
}