using Abbybot_III.Core.CommandHandler.extentions;
using Abbybot_III.Core.CommandHandler.Types;
using Abbybot_III.extentions;

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Abbybot_III.Commands.Contains
{
    class Egg : ContainCommand
    {
        public int Min;
        public int Max;
        public string Reply;

        public Egg(int min, int max, string word, string reply)
        {
            Min = min;
            Max = max;
            Command = word;
            Reply = reply;
        }

        public Egg()
        {

        }

        public override async Task DoWork(AbbybotCommandArgs md)
        {
            string o = Chance.Roll(Min, Max) ? Reply : string.Empty;
            await md.Send(o);
        }

        public override async Task<bool> ShowHelp(AbbybotCommandArgs aca)
        {
            await Task.CompletedTask;
            return false;
        }
    }
}
