using Abbybot_III.Core.AbbyBooru.sql;
using Abbybot_III.Core.CommandHandler.extentions;
using Abbybot_III.Core.CommandHandler.Types;
using Abbybot_III.Sql.abbybooru;

using BooruSharp.Search.Post;

using Discord;

using System;
using System.Linq;

using System.Threading.Tasks;

namespace Abbybot_III.Commands.Normal.AbbyBooruChecker
{
	[Capi.Cmd("abbybot acadd", 5, 1)]
	class AddCharacter : NormalCommand
	{
		public override async Task DoWork(AbbybotCommandArgs a)
		{
			var FavoriteCharacter = a.Replace(Command);

			
			if (a.user.isGuild && !a.user.admin)
				await a.Send($"silly {a.user.Preferedname}, you're not an admin here!!!");
			Abbybot.print("acadd called");
			if (FavoriteCharacter.Length < 1)
				return;
			a.BuildAbbybooruTag(FavoriteCharacter);

			string pictureurl = "https://img2.gelbooru.com/samples/ee/e2/sample_eee286783bfa37e088d1ffbcf8f098ba.jpg";
			var o = new string[1];
			o[0] = FavoriteCharacter.ToString() + "*";

			var ee = await Gelbooru.Fc.awa(a, o);
			EmbedBuilder eb = new EmbedBuilder();
			eb.ImageUrl = pictureurl;
			var u = a.user;
			if (ee.canrun)
			{
				try
				{
					await AbbyBooruSql.AddCharacterAsync(a.channel, a.guild, ee.fc);
				}
				catch (Exception e)
				{
					Abbybot.print(e.Message);

					eb.Title = $"silly!!! {a.user.Preferedname}!!!";
					eb.Color = Color.Red;
					eb.Description = $"silly!! {ee.fc} was already added to this channel!!";
					await a.Send(eb);
					return;
				}
				eb.Title = $"{a.user.Preferedname} Yayy!!";
				eb.Color = Color.Green;
				eb.Description = $"I added {ee.fc} to the channel master!! ";
			}
			else
			{
				eb.Title = $"oof... {a.user.Preferedname}...";
				eb.Color = Color.Red;
				eb.Description = $"sorry {u.Preferedname}... i couldn't find {ee.fc} ({FavoriteCharacter}) ...";
			}
			await a.Send(eb);
		}

		public override async Task<string> toHelpString(AbbybotCommandArgs aca)
		{
			return $"Add a gelbooru tag feed to a channel";
		}
	}
}