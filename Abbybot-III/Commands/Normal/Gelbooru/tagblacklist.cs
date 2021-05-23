using Abbybot_III.Apis.Booru;
using Abbybot_III.Core.CommandHandler.extentions;
using Abbybot_III.Core.CommandHandler.Types;
using Abbybot_III.Core.Users.sql;

using Discord;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Abbybot_III.Commands.Normal.Gelbooru
{
	//[Capi.Cmd("abbybot tagbadtaglist", 1, 1)]
	class tagbadtaglist : NormalCommand
	{
		public override async Task DoWork(AbbybotCommandArgs message)
		{
			EmbedBuilder eb = new EmbedBuilder();
			StringBuilder sb = new StringBuilder();
			eb.Title = $"here's your bad tag list";
			eb.Color = Color.Blue;
			foreach (var tag in await UserBadTagListSql.GetbadtaglistTags(message.user.Id))
				sb.AppendLine(tag);
			eb.Description = sb.ToString();

			await message.Send(eb);
		}

		public override async Task<string> toHelpString(AbbybotCommandArgs aca)
		{
			return await Task.FromResult($"Get the list of tags you badtaglisted");
		}
	}
}