using Abbybot_III.Core.CommandHandler.extentions;
using Abbybot_III.Core.CommandHandler.Types;

using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Abbybot_III.Commands.Normal
{
    //[Capi.Cmd("abbybot !normal", 1, 1)]
    class NormalCommand : BaseCommand
    {
        public override async Task DoWork(AbbybotCommandArgs e)
        {
            var args = e.ReplaceSplit(Command, " ");

            StringBuilder sb = new StringBuilder();
            sb.Append($"normal command {Command} was called. got back these args:");
            foreach (string item in args)
            {
                sb.Append($" {item}");
            }

            await e.Send(sb);
        }

        public override async Task<bool> Evaluate(AbbybotCommandArgs aca)
        {
            var e = aca.Split(" ");
            //Abbybot.print($"{Command}");
            if (e.Length < 2) return false;
            if ((e[0].ToLower() + " " + e[1].ToLower()) == Command.ToLower())
            {
                //Abbybot.print($"{(e[0].ToLower() + " " + e[1].ToLower())}");
                return await base.Evaluate(aca);
            }
            else
                return false;
        }

        public override Task<bool> ShowHelp(AbbybotCommandArgs aca)
        {
            return base.Evaluate(aca);
        }

        public override async Task<string> toHelpString(AbbybotCommandArgs aca)
        {
            await Task.CompletedTask;
            return $"a normal command.";
        }
    }
}