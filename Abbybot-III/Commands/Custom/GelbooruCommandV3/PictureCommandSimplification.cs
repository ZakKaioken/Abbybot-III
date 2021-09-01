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
using Abbybot_III.Core.RequestSystem;

class PictureCommandSimplification
{
	public async Task GetPicture(AbbybotCommandArgs aca,  PictureCommandData message, Action<EmbedBuilder> OnSuccess = null, Action<string> onFail = null)
	{
		EmbedBuilder eb = new();

		var Commands = await AbbySql.AbbysqlClient.FetchSQL("Select * from `abbybooru`.`commands`;");
		if (Commands.Count <= 0)
		{
			onFail?.Invoke("I'm sorry master I'm so dizzy... I don't see any picture commands anymore...");
			return;
		}
		var picture = Commands.ToList().Where(x => message.message.Contains(x["Command"] is string cc ? $"abbybot {cc}" : "anotherunlikelycommand")).Take(3).ToList();

		if (picture.Count <= 0) return;

		string pfc = message.favoriteCharacter;
		foreach (var command in picture)
		{
			string tags = command["Tags"] is string ta ? ta : "";
			int rating = command["RatingId"] is int rI ? rI : -1;
			string commandi = command["Command"] is string cmdo ? cmdo : "missing";
			string nick = command["Nickname"] is string tw && tw.Length > 0 ? tw : commandi;
			if (tags == "" || rating == -1) continue;

			if (!message.ratings.Contains((CommandRatings)rating)) continue;
			if (message.favoriteCharacter.Contains(" ~ "))
			{
				var smfc = message.favoriteCharacter.Replace("{", "").Replace("}", "").Split(" ~ ");
				message.index %= (ulong)smfc.Length;
				pfc = (message.Rolling) ? smfc[message.index++] : smfc[(new Random()).Next(0, smfc.Length)];
			}

			(string, string)[] types = new (string, string)[] {
				(message.channelFavoriteCharacter, pfc),
				(message.channelFavoriteCharacter, null),
				(pfc, null),
				("abigail_williams_(fate/grand_order)", null)
			};

			List<string> tagd = tags.Split(" ").ToList();
			if (message.isGuildChannel)
			{
				if (!message.isNSFW)
					tagd.Add("rating:safe");
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
				List<string> tagz = null;
				string ww = "";

				await AddTags(tagd, types, triesIndex,
					OnSuccess: (a, b) =>
					{
						tagz = a;
						ww = b;
					});

				await aca.GetPicture(tagz.ToArray(),
					GotResult: imgdata => imageData = new ImageData()
					{
						pictureCharacter = ww,
						FileUrl = imgdata.FileUrl,
						PreviewUrl = imgdata.PreviewUrl,
						Tags = imgdata.Tags.ToArray(),
						Source = (imgdata.Source is string sos && sos.Length > 0 && sos != "noimagefound") ? sos : "No source",
						Nsfw = imgdata.Rating != BooruSharp.Search.Post.Rating.Safe
					},
					OnFail: e => triesIndex++
				);
			} while (imageData == null && triesIndex <= types.Length);

			if (imageData == null)
			{
				await aca.Send("No picture found.");
				continue;
			}

			if (imageData.Source == "No source")
			{
				await aca.Send($"Master... I didn't find a {nick}ing picture of {aca.BreakAbbybooruTag(imageData.pictureCharacter)}");
				continue;
			}
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

			OnSuccess?.Invoke(GelEmbed.Build(aca,
				fileurl: imageData.FileUrl.ToString(),
				source: imageData.Source,
				fc: imageData.pictureCharacter,
				mentionedUsers: message.mentions,
				command: nick,
				user: message.user
			));
		}
	}

	private static async Task QueueDelete(AbbybotCommandArgs aca, ImageData imageData, Discord.Rest.RestUserMessage abbybotMessage)
	{
		int deleteTime = aca.guild.AutoDeleteTime;
		var adt = 0;
		if (imageData.ContainsLoli)
			adt = deleteTime / 2;
		else if (imageData.Nsfw)
			adt = deleteTime;

		if (adt > 0)
			await aca.Send(abbybotMessage, aca.originalMessage, RequestType.Delete, DateTime.Now.AddSeconds(adt));
	}

	private async Task AddTags(List<string> tags, (string, string)[] types, int index, Action<List<string>, string> OnSuccess, Action OnFailure=null  )
	{
		var tagz = tags.ToList();
		string ww = "";

		if (types[index].Item1 is string i1)
		{
			tagz.Add(i1);
			ww += i1;
		}
		if (types[index].Item2 is string i2)
		{
			tagz.Add(i2);
			ww += $" {i2}";
		}

		if (ww.Length <1) 
			OnFailure?.Invoke();
		OnSuccess?.Invoke(tagz, ww);
	}
}