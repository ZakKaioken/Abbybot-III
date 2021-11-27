using Abbybot_III.Commands.Contains.Gelbooru.embed;
using Abbybot_III.Core.CommandHandler.extentions;
using Abbybot_III.Core.CommandHandler.Types;
using Abbybot_III.Core.RequestSystem;
using Abbybot_III.Core.Users.sql;
using Abbybot_III.Sql.Abbybot.User;

using AbbySql.Types;

using Capi.Interfaces;

using Discord;
using Discord.WebSocket;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abbybot_III.Commands.Contains.GelbooruV4
{
	[Capi.Cmd("GelbooruCommandV4", 1, 1)]
	class GelbooruCommandV4 : BaseCommand
	{
		public override async Task DoWork(AbbybotCommandArgs aca)
		{
			var Commands = await AbbySql.AbbysqlClient.FetchSQL("Select * from `abbybooru`.`commands`;");
			
			if (Commands.Count <= 0)
				return;
			
			var picture = Commands.ToList().Where(x => aca.Contains(x["Command"] is string cc ? $"abbybot !{cc}" : "anotherunlikelycommand")).Take(3).ToList();

			var msg = new Message();
			await msg.Init(aca);

			foreach (var item in picture)
			{
				GelbooruCommand cmd = new() {
					tags = (item["Tags"] is string ta ? ta : "").Split(" ").ToArray(),
					rating = item["RatingId"] is int rI ? rI : -1,
					command = item["Command"] is string cmdo ? cmdo : "missing",
				};
				cmd.nickname = item["Nickname"] is string tw && tw.Length > 0 ? tw : cmd.command;
				cmd.message = msg;
				
				int index = 0;
				var e = await aca.IncreasePassiveStat("GelCommandUsages");
				foreach(var i in e) 
					index += (int)i.stat;
				msg.pfc = cmd.GetRandomFC(index);
				if (cmd.TestCommand()) continue;
				cmd.GenTypes(msg.pfc);
				var tags = cmd.AddBadTags();

				List<GelbooruResult> results;
				string usedfc;
				index = 0;
				do {
				var ee = cmd.AddTags(tags, index);
				usedfc = ee.usedfc;
				results = await cmd.GetPictureAsync(ee.tagz.ToArray());
				} while (results == null  && ++index < cmd.types.Count);

				if (results == null||results.Count == 0)
				{
					await aca.Send($"Master... I didn't find a {cmd.nickname}ing picture of {msg.pfc}");
					continue;
				}

				if (!msg.isNSFW && results[0].Nsfw)
				{
					Console.WriteLine("server's nsfw is off");
					await aca.Send("Master that's a lewd image... I can't send it...");
					continue;
				}

				if (!msg.isLoli && results[0].ContainsLoli)
				{
					Console.WriteLine("server loli is off");
					await aca.Send("Master... I found an image, but it's against discord's tos so i'm not going to send it.");
					continue;
				}

				var imageData = results[0];

				int deleteTime = aca.guild.AutoDeleteTime;
				var adt = -1;
				if (imageData.ContainsLoli)
					adt = deleteTime / 2;
				else if (imageData.Nsfw)
						adt = deleteTime;

			var embed = GelEmbed.Build(aca,
				fileurl: imageData.FileUrl.ToString(),
				source: imageData.Source,
				fc: usedfc,
				mentionedUsers: msg.mentions,
				command: cmd.nickname,
				user: msg.user,
				autoDeleteTime: adt
			); 

			var abm = await aca.Send(embed);
			
			if (adt > 0)
				await QueueDelete(aca, adt, abm);

			}
			
		}

		static async Task QueueDelete(AbbybotCommandArgs aca, int autoDeleteTime, Discord.Rest.RestUserMessage abbybotMessage)
		{
			await aca.Send(abbybotMessage, aca.originalMessage, RequestType.Delete, DateTime.Now.AddSeconds(autoDeleteTime));
		}
		public override async Task<bool> Evaluate(AbbybotCommandArgs aca)
		{
			return (await base.Evaluate(aca));
		}
        public override async Task<bool> ShowHelp(AbbybotCommandArgs aca)
        {
            return true;
        }
		public override async Task<string> toHelpString(AbbybotCommandArgs aca)
		{
			var Commands = await AbbySql.AbbysqlClient.FetchSQL("Select * from `abbybooru`.`commands` ORDER BY `RatingId` ASC, `Command` ASC  ;");
			if (Commands.Count <= 0) return ("I'm sorry master I'm so dizzy... I don't see any picture commands anymore...");
			StringBuilder sb = new StringBuilder("\n**Gelbooru Commands: **\n");
			foreach (AbbyRow command in Commands)
			{
				int rating = command["RatingId"] is int rI ? rI : -1;
				string cmd = command["Command"] is string cmdi ? cmdi : "";
				bool rate = aca.channel is not SocketDMChannel;
				if (rate) if (!aca.user.Ratings.Contains((CommandRatings)rating)) continue;
				sb.Append($"``%!{cmd}`` ");
			}
			return sb.ToString();
		}

	}
}