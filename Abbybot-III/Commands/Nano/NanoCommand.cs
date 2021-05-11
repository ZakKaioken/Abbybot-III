using Abbybot_III.Commands.Custom.PassiveUsage;
using Abbybot_III.Core.CommandHandler.Types;
using Abbybot_III.Sql.Abbybot.Abbybot;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abbybot_III.Commands.Nano
{
    class NanoCommand : PassiveCommand
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

            return e && listenChannel && await Eval(aca);
        }

        public async Task<bool> Eval(AbbybotCommandArgs aca)
        {
            bool v = (aca.Message.ToLower().Contains(Command.ToLower()));
            if (v) return await base.Evaluate(aca);
            else return false;
        }

        public override async Task<bool> ShowHelp(AbbybotCommandArgs aca)
        {
            return false;
        }
    }
}