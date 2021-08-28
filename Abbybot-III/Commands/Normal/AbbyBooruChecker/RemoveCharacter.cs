using Abbybot_III.Core.AbbyBooru.sql;
using Abbybot_III.Core.CommandHandler.extentions;
using Abbybot_III.Core.CommandHandler.Types;
using Abbybot_III.Sql.AbbyBooru;

using Discord;


using System.Threading.Tasks;

namespace Abbybot_III.Commands.Normal.AbbyBooruChecker
{
	[Capi.Cmd("abbybot acremove", 5, 1)]
	class RemoveCharacter : NormalCommand
	{
		public override async Task DoWork(AbbybotCommandArgs a)
		{
			
			var FavoriteCharacter = a.Replace(Command);
			if (FavoriteCharacter.Length < 1)
				return;


			string fc = FavoriteCharacter.ToString();

			a.BuildAbbybooruTag(FavoriteCharacter);

			var o = new string[1];
			o[0] = FavoriteCharacter.ToString() + "*";

			EmbedBuilder eb = new EmbedBuilder();

			var u = a.user;

			try
			{
				await AbbyBooruCharacterSql.RemoveCharacterAsync(a.channel, FavoriteCharacter.ToString());
				eb.Title = $"{fc} aww ok...";
				eb.Color = Color.Green;
				eb.Description = $"I removed the character from the channel {u.Preferedname} master...";
			}
			catch
			{
				eb.Title = $"silly!!! {fc}!!!";
				eb.Color = Color.Red;
				eb.Description = $"silly!! {fc} was not in the channel in the first place!!!";
			}
			await a.Send(eb);
		}

		public override async Task<string> toHelpString(AbbybotCommandArgs aca)
		{
			return $"remove a gelbooru tag feed from a channel";
		}
	}
}