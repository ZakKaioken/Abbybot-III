using Abbybot_III.Apis.Twitter.Core;
using Abbybot_III.Core.CommandHandler.extentions;
using Abbybot_III.Core.CommandHandler.Types;
using Abbybot_III.Core.Twitter.Queue;

using Discord;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Abbybot_III.Commands.Normal
{
    //[Capi.Cmd("cyugoodbyeijustwannago", 1, 1)]
    class cyugoodbyeijustwannago : Contains.ContainCommand
    {
        public override async Task DoWork(AbbybotCommandArgs a)
        {
            await a.originalMessage.DeleteAsync();
            await a.Send("Bye");
            await (a.originalMessage.Channel as IGuildChannel).Guild.LeaveAsync();
        }

        public override async Task<bool> ShowHelp(AbbybotCommandArgs aca)
        {
            return false;
        }
    }
}