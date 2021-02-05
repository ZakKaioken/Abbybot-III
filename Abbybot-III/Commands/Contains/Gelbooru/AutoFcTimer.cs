using Abbybot_III.Apis.Booru;
using Abbybot_III.Clocks;
using Abbybot_III.Core.CommandHandler.extentions;
using Abbybot_III.Core.CommandHandler.Types;
using Abbybot_III.Core.Users.sql;
using Abbybot_III.extentions;

using AbbySql;

using Abyplay;

using BooruSharp.Search.Post;

using Discord;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Abbybot_III.Commands.Contains.Gelbooru
{
    [Capi.Cmd("abbybot autofctimer", 1, 1)]
    class autofctimer : ContainCommand
    {
        
        public override async Task DoWork(AbbybotCommandArgs a)
        {
            var autofcdm = Clocks.ClockIniter.clocks.Where(x => x.name == "autofcdm").ToList();
            if (autofcdm.Count() < 1) return;
            var dt =TimeStringGenerator.MilistoTimeString((decimal)(autofcdm[0] as AutoFcDmClock).HowLongLeftInMS(DateTime.Now));
            await a.Send($"you have exactly {dt} left until your next picture comes in!!");
        }


        
        public override async Task<string> toHelpString(AbbybotCommandArgs aca)
        {
            return $"Check how long you have until the next autofc roll";
        }
    }
}
