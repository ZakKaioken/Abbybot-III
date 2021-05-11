using Abbybot_III.Apis.Booru;
using Abbybot_III.Core.AbbyBooru.sql;
using Abbybot_III.Core.CommandHandler.extentions;
using Abbybot_III.Core.CommandHandler.Types;

using BooruSharp.Search.Post;

using Discord;

using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abbybot_III.Commands.Normal.AbbyBooruChecker
{
	[Capi.Cmd("abbybot acadd", 5, 1)]
	class AddCharacter : NormalCommand
	{
		public override async Task DoWork(AbbybotCommandArgs a)
		{
			StringBuilder FavoriteCharacter = new StringBuilder(a.Message.Replace(Command, ""));
			if (a.abbybotUser.userGuild != null && !a.abbybotUser.userGuild.admin)
				await a.Send($"silly {a.abbybotUser.userNames.PreferedName}, you're not an admin here!!!");
			Abbybot.print("acadd called");
			if (FavoriteCharacter.Length < 1)
				return;
			while (FavoriteCharacter[0] == ' ')
				FavoriteCharacter.Remove(0, 1);
			while (FavoriteCharacter[^1] == ' ')
				FavoriteCharacter.Remove(FavoriteCharacter.Length - 1, 1);

			Gelbooru.Fc.FCBuilder(FavoriteCharacter);
			string pictureurl = "https://img2.gelbooru.com/samples/ee/e2/sample_eee286783bfa37e088d1ffbcf8f098ba.jpg";
			var o = new string[1];
			o[0] = FavoriteCharacter.ToString() + "*";
			bool canrun = false;
			int tries = 0;
			var ee = await Gelbooru.Fc.awa(o);
			EmbedBuilder eb = new EmbedBuilder();
			eb.ImageUrl = pictureurl;
			var u = a.abbybotUser;
			if (ee.canrun)
			{
				try
				{
					await AbbyBooruSql.AddCharacterAsync(a.channel, a.abbybotGuild, ee.fc);
				}
				catch (Exception e)
				{
					Abbybot.print(e.Message);

					eb.Title = $"silly!!! {a.abbybotUser.userNames.PreferedName}!!!";
					eb.Color = Color.Red;
					eb.Description = $"silly!! {ee.fc} was already added to this channel!!";
					await a.Send(eb);
					return;
				}
				eb.Title = $"{a.abbybotUser.userNames.PreferedName} Yayy!!";
				eb.Color = Color.Green;
				eb.Description = $"I added {ee.fc} to the channel master!! ";
			}
			else
			{
				eb.Title = $"oof... {a.abbybotUser.userNames.PreferedName}...";
				eb.Color = Color.Red;
				eb.Description = $"sorry {u.userNames.PreferedName}... i couldn't find {ee.fc} ({FavoriteCharacter}) ...";
			}
			await a.Send(eb);
		}

		public override async Task<string> toHelpString(AbbybotCommandArgs aca)
		{
			return $"Add a gelbooru tag feed to a channel";
		}
	}
}