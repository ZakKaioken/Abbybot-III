using Abbybot_III.Core.CommandHandler.extentions;
using Abbybot_III.Core.CommandHandler.Types;
using Abbybot_III.extentions;

using Discord;

using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abbybot_III.Commands.Contains
{
[Capi.Cmd("abbybot button", 1,1)]
    class EmojiTest : ContainCommand
    {
        public override async Task DoWork(AbbybotCommandArgs md)
        {
            ComponentBuilder builder = new();
            builder.WithButton("I'm a button!!!", "button!", ButtonStyle.Primary);
            await md.originalMessage.Channel.SendMessageAsync(text: "here's a button:", components: builder.Build());
        }
    }
}