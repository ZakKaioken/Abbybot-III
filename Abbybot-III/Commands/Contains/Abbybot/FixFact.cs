using Abbybot_III.Core.CommandHandler.extentions;
using Abbybot_III.Core.CommandHandler.Types;
using Abbybot_III.extentions;
using Abbybot_III.Sql.Abbybot.Abbybot;

using System;
using System.Threading.Tasks;

namespace Abbybot_III.Commands.Normal
{
	[Capi.Cmd("abbybot fixfact", 1, 1)]
	class fixFact : Contains.ContainCommand
	{
		Random r = new Random();

		public override async Task DoWork(AbbybotCommandArgs a)
		{
			var facts = await FunAbbybotFactsSql.GetFactsList(await a.IsNSFW());
			var ra = r.Next(0, facts.Count);
			if (facts.Count > 0)
				await a.Send(facts[ra].fact.ReplaceA("ab!", "%"));
			else
				await a.Send("I'm sorry master... My facts list is empty... I... I can't send you a fact :(");
		}

		public override async Task<string> toHelpString(AbbybotCommandArgs aca)
		{
			var facts = await FunAbbybotFactsSql.GetFactsList(await aca.IsNSFW());
			if (facts.Count == 0) return "I... I don't understand... My facts list is empty... %fact won't do anything...";
			var ra = r.Next(0, facts.Count);
			return $"{facts[ra].fact}, get another abbybot fact with this command!!";
		}
	}
}