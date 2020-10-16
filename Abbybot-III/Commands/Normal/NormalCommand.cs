using Abbybot_III.Apis.Discord.Events;
using Abbybot_III.Core.CommandHandler.extentions;
using Abbybot_III.Core.CommandHandler.Types;

using Capi.Interfaces;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Abbybot_III.Commands.Normal
{
    [Capi.Cmd("%!normal", 1, 1)]
    class NormalCommand : BaseCommand
    {
        public override async Task DoWork(AbbybotCommandArgs e)
        {
            List<string> args = new List<string>(e.Message.Replace(Command, "").Split(' '));
            if (args[0] == " ")
                args.Remove(" ");

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
            var e = aca.Message.Split();

            return e[0] == Command && await base.Evaluate(aca);
        }
        public override async Task<string> toHelpString(AbbybotCommandArgs aca)
        {
            await Task.CompletedTask;
            return $"{Command}: a normal command.";
        }

    }
}
