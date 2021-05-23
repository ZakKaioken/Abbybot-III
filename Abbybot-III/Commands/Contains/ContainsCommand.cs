using Abbybot_III.Core.CommandHandler.Types;
using System.Text;
using System.Threading.Tasks;

namespace Abbybot_III.Commands.Contains
{
    //[Capi.Cmd("abbybot !contains", 1,1)]
    public class ContainCommand : BaseCommand
    {
        public override async Task<bool> Evaluate(AbbybotCommandArgs aca)
        {
            bool v = aca.Contains(Command, true);
            if (v) return await base.Evaluate(aca);
            else return false;
        }

        public override Task<bool> ShowHelp(AbbybotCommandArgs aca)
        {
            return base.Evaluate(aca);
        }

        public override async Task DoWork(AbbybotCommandArgs aca)
        {
            //$" was called with the message: {aca.Replace(Command)}";
           
            await Task.CompletedTask;
        }

        public override async Task<string> toHelpString(AbbybotCommandArgs aca)
        {
            await Task.CompletedTask;
            return $"a contains command.";
        }
    }
}