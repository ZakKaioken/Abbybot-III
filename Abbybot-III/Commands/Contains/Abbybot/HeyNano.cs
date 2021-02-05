using Abbybot_III.Clocks;
using Abbybot_III.Core.CommandHandler.extentions;
using Abbybot_III.Core.CommandHandler.Types;
using Abbybot_III.Sql.Abbybot.Abbybot;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Abbybot_III.Commands.Contains.Abbybot
{
    [Capi.Cmd("Good morning cute sister abbybot", 1,1)]
    class GmNano :ContainCommand
    {
        public override async Task<bool> Evaluate(AbbybotCommandArgs aca)
        {
            var abbybots = await AbbybotSql.GetAbbybotIdAsync();
            bool e = false;
            foreach (var abbybot in abbybots)
                if (aca.abbybotUser.Id == abbybot) e = true;

            bool listenChannel = false;
            var abbybotchannels = await AbbybotSql.GetAbbybotChannelIdAsync();
            foreach (var abbybotchannel in abbybotchannels)
                if (aca.channel.Id == abbybotchannel.channelId) listenChannel = true;

            return e && listenChannel && await base.Evaluate(aca);
        }
        public override async Task DoWork(AbbybotCommandArgs aca)
        {
            await aca.Send("thank you for waking me up cutie nano!!");
            PingAbbybotClock.o = 1;
        }
        public override async Task<bool> ShowHelp(AbbybotCommandArgs aca)
        {
            return false;
        }
    }
}
