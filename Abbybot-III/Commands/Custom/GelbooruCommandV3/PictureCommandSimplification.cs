using System.Threading.Tasks;
using System.Linq;

using System;
using System.Collections.Generic;
using Capi.Interfaces;
using Abbybot_III.Commands.Contains.Gelbooru.embed;
using Discord;
using Abbybot_III.Apis;
using Abbybot_III.Core.CommandHandler.Types;
using Abbybot_III.Core.CommandHandler.extentions;
using Abbybot_III.extentions;

class PictureCommandSimplification
{
	public async Task<EmbedBuilder> GetPicture(AbbybotCommandArgs aca,  PictureCommandData message)
	{
		EmbedBuilder eb = new EmbedBuilder();

		var Commands = await AbbySql.AbbysqlClient.FetchSQL("Select * from `abbybooru`.`commands`;");
		if (Commands.Count <= 0) throw new Exception("I'm sorry master I'm so dizzy... I don't see any picture commands anymore...");

		var picture = Commands.ToList().Where(x => message.message.Contains(x["Command"] is string cc ? $"abbybot {cc}" : "anotherunlikelycommand")).Take(3).ToList();

		if (picture.Count <= 0) throw new Exception("The message did not have any matching picture commands");
		string pfc = message.favoriteCharacter;;
		foreach (var command in picture)
		{
			string tags = command["Tags"] is string ta ? ta : "";
			int rating = command["RatingId"] is int rI ? rI : -1;
			string nick = command["Nickname"] is string tw ? tw : "";
			if (tags == "" || rating == -1) continue;

			if (!message.ratings.Contains((CommandRatings)rating)) continue;
			if (message.favoriteCharacter.Contains(" ~ "))
				{
					var smfc = message.favoriteCharacter.Replace("{", "").Replace("}", "").Split(" ~ ");
					message.index %= (ulong)smfc.Length;
					pfc = (message.Rolling) ? smfc[message.index++] : smfc[(new Random()).Next(0, smfc.Length)];
				}
				Console.WriteLine("[");
				Console.WriteLine(message.favoriteCharacter);
				Console.WriteLine(pfc);
				Console.WriteLine("]");
			
				(string, string)[] types = new (string, string)[] {
				(message.channelFavoriteCharacter, pfc),
				(message.channelFavoriteCharacter, null),
				(pfc, null),
				("abigail_williams_(fate/grand_order)", null)
			};

			List<string> tagd = tags.Split(" ").ToList();
			if (message.isGuildChannel)
			{
				if (!message.isNSFW) tagd.Add("rating:safe");
				if (!message.isLoli)
				{
					tagd.Add("-loli");
					tagd.Add("-shota");
				}
			}
			foreach (var badTag in message.badTags)
			{
				tagd.Add($"-{badTag}");
			}

			int triesIndex = 0;
			ImageData imageData = null;
			do
			{
				try
				{
					var tagz = tagd.ToList();
					string ww = "";
					if (types[triesIndex].Item1 is string i1)
					{
						tagz.Add(i1);
						Console.Write($"[{i1}]");
						ww += i1;
					}
					if (types[triesIndex].Item2 is string i2)
					{
						tagz.Add(i2);
						Console.Write($"[{i2}]");
						ww += $" {i2}";
					}
					if (ww.Length < 3) throw new Exception("it didn't work right...");
					var imgdata = await aca.GetPicture(tagz.ToArray());
					imageData = new ImageData()
					{
						pictureCharacter = ww,
						FileUrl = imgdata.FileUrl,
						PreviewUrl = imgdata.PreviewUrl,
						Tags = imgdata.Tags.ToArray(),
						Source = (imgdata.Source is string sos && sos.Length > 0 && sos != "noimagefound") ? sos : "No source",
						Nsfw = imgdata.Rating != BooruSharp.Search.Post.Rating.Safe
					};
				}
				catch
				{
					triesIndex++;
				}
			} while (imageData == null && triesIndex <= 3);
			if (triesIndex > 3)
			{
				await aca.Send("No picture found.");
				continue;
			}

			if (imageData.Source == "No source")
			{
				await aca.Send($"Master... I didn't find a {command["Command"]}ing picture of {aca.BreakAbbybooruTag( imageData.pictureCharacter)}");
				continue;
			}
			Console.WriteLine(imageData.pictureCharacter);
			if (!message.isNSFW && imageData.Nsfw)
			{
				await aca.Send("Master that's a lewd image... I can't send it...");
				continue;
			}

			if (!message.isLoli && imageData.ContainsLoli)
			{
				await aca.Send("Master... I found an image, but it's against discord's tos so i'm not going to send it.");
				continue;
			}
			var pc = imageData.pictureCharacter;
			var filrl = imageData.FileUrl;
			var ssss = imageData.Nsfw;
			var soai = imageData.Source;
			var iisu = imageData.ContainsLoli;

			eb = GelEmbed.Build(aca, filrl.ToString(), soai, pc, message.mentions, command["Command"] as string, message.user);
			await aca.Send(eb);
		}
		return null;
	}
}