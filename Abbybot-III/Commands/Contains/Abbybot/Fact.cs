
using Abbybot_III.Core.CommandHandler.extentions;
using Abbybot_III.Core.CommandHandler.Types;
using Abbybot_III.Sql.Abbybot.Abbybot;

using Discord.WebSocket;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Abbybot_III.Commands.Normal
{
    [Capi.Cmd("abbybot fact", 1, 1)]
    class Fact : Contains.ContainCommand
    {
        Random r = new Random();
        public override async Task DoWork(AbbybotCommandArgs a)
        {
            var facts = await FunAbbybotFactsSql.GetLatestMentionIdsAsync(await a.IsNSFW());
            var ra = r.Next(0, facts.Count);
            await a.Send(facts[ra].fact);
        }
        public override async Task<string> toHelpString(AbbybotCommandArgs aca)
        {
            var facts = await FunAbbybotFactsSql.GetLatestMentionIdsAsync(await aca.IsNSFW());
            var ra = r.Next(0, facts.Count);
            return $"{facts[ra].fact}, get another abbybot fact with this command!!";
        }
    }
}
