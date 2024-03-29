﻿using Abbybot_III.Core.CommandHandler.extentions;
using Abbybot_III.Core.CommandHandler.Types;
using Abbybot_III.extentions;
using Abbybot_III.Sql.Abbybot.Fun;

using Discord;
using Discord.WebSocket;
using System.Text;
using System.Threading.Tasks;

namespace Abbybot_III.Commands.Normal
{
	[Capi.Cmd("abbybot whisper", 1, 1)]
	class dm : Contains.ContainCommand
	{
		public override async Task DoWork(AbbybotCommandArgs a)
		{
			if (a.isMentioning) return;
			var FavoriteCharacter = a.Replace(Command).Replace("--debugmode", "");
		
			var o = await SaybadtaglistSql.GetbadtaglistTags();
			foreach (var oo in o)
			{
				FavoriteCharacter.Replace(oo, "");
			}

			FavoriteCharacter.Insert(0, "you have a givt from a secret sender!!!\n");
			var mu = a.getMentionedDiscordGuildUsers();
			StringBuilder sb = new StringBuilder();
			foreach (var muz in mu)
			{
				await muz.SendMessageAsync(FavoriteCharacter.ToString());
				await Task.Delay(100);
			}
			sb.Append("Sent a dm to ");
			sb.AppendJoin(", ", mu);

			if (!(a.channel is SocketDMChannel))
				await a.Delete();
			await a.Send(sb);
		}

		public override async Task<string> toHelpString(AbbybotCommandArgs aca)
		{
			return $"send an anonymous dm to someone you mention";
		}
	}
}