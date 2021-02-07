using Abbybot_III.Commands.Contains;
using Abbybot_III.Core.CommandHandler.extentions;
using Abbybot_III.Core.CommandHandler.Types;

using System.Threading.Tasks;

namespace Abbybot_III.Commands.Normal
{
    [Capi.Cmd("abbybot avatar", 1, 1)]
    class avatar : ContainCommand
    {
        public override async Task DoWork(AbbybotCommandArgs a)
        {
            var mu = a.mentionedUserIds.ToArray();
            if (mu.Length < 1)
                return;
            await a.Send(mu[0].GetAvatarUrl());
        }

        public override async Task<string> toHelpString(AbbybotCommandArgs aca)
        {
            return "~~Steal~~ I mean, get somebody's profile picture";
        }
    }
}