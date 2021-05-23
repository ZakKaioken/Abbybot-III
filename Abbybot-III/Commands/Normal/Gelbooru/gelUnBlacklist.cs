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
	//[Capi.Cmd("abbybot unbadtaglisttag", 1, 1)]
	class UnbadtaglistTag : NormalCommand
	{
		public override async Task DoWork(AbbybotCommandArgs message)
		{
			StringBuilder FavoriteCharacter = new StringBuilder(message.Message.Replace(Command, ""));

			while (FavoriteCharacter[0] == ' ')
				FavoriteCharacter.Remove(0, 1);
			while (FavoriteCharacter[^1] == ' ')
				FavoriteCharacter.Remove(FavoriteCharacter.Length - 1, 1);
		}

		public override async Task<string> toHelpString(AbbybotCommandArgs aca)
		{
			return await Task.FromResult($"remove badtaglisted tags you badtaglisted.");
		}
	}
}