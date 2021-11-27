using Abbybot_III.Core.CommandHandler.extentions;
using Abbybot_III.Core.CommandHandler.Types;
using Abbybot_III.extentions;

using System.Linq;
using System.Threading.Tasks;

namespace Abbybot_III.Commands.Contains
{
[Capi.Cmd("abbybot emoji", 1,1)]
    class EmojiTest : ContainCommand
    {

        public override async Task DoWork(AbbybotCommandArgs md)
        {
            if (md.guild.Id == 0) return;
            var yes = await md.Send("did someone say abbybot emoji?");
            var emojis = await md.GetGuild(md.guild.Id).GetEmotesAsync();
            var emoji = emojis.ToList()[5];
            await yes.AddReactionAsync(emoji);
        }

        public override async Task<bool> ShowHelp(AbbybotCommandArgs aca)
        {
            await Task.CompletedTask;
            return false;
        }
    }
}