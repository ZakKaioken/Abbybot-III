using System.Text;
using System.Threading.Tasks;
using Abbybot_III.Core.AbbyBooru;
using Abbybot_III.Core.CommandHandler.extentions;
using Abbybot_III.Core.CommandHandler.Types;

namespace Abbybot_III.Commands.Normal.Gelbooru
{
    [Capi.Cmd("abbybot fcvisor",1,1)]
    class FCVisualizer : NormalCommand
    {
        public override async Task DoWork(AbbybotCommandArgs e)
        {
            var message = e.Replace(Command);
            var sb = new StringBuilder();
            AbbybooruTagGenerator.FCBuilder(message, sb);
            await e.Send(sb);
        }
    }
}