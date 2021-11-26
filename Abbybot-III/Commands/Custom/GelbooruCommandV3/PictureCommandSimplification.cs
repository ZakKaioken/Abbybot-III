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
using System.Text;

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
		Console.Clear();
		string pfc = message.favoriteCharacter;
		foreach (var command in picture)
		{

			string tags = command["Tags"] is string ta ? ta : "";
			int rating = command["RatingId"] is int rI ? rI : -1;
			string commandi = command["Command"] is string cmdo ? cmdo : "missing";
			string nick = command["Nickname"] is string tw && tw.Length > 0 ? tw : commandi;

			var cmdu = await aca.IncreasePassiveStat($"{commandi.ToLower()}Uses");

			Console.WriteLine($"{nick}: {((CommandRatings)rating)}");
			if (tags == "" || rating == -1) continue;
			if (!message.isNSFW && ((int)CommandRatings.hot) == rating)
			{
				await aca.Send($"I could not show you a {nick}ing picture here, as it's nsfw.");
				continue;
			}
			if (!message.isLoli && ((int)CommandRatings.omy) == rating)
			{
				await aca.Send($"I'm not even allowed to send a {nick}ing picture.");
				continue;
			}
			if (!message.ratings.Contains((CommandRatings)rating)) continue;
			if (message.favoriteCharacter.Contains(" ~ "))
			{
				Console.WriteLine($"there are multiple favorite characters");
				var smfc = message.favoriteCharacter.Replace("{", "").Replace("}", "").Split(" ~ ");
				var e = await aca.IncreasePassiveStat("GelCommandUsages");
				foreach (var sta in e)
					message.index += sta.stat;
				message.index %= (ulong)smfc.Length;
				pfc = (message.Rolling) ? smfc[message.index++] : smfc[(new Random()).Next(0, smfc.Length)];
				Console.WriteLine($"rolling {message.Rolling}: ");

			}

			List<(string, string)> types = new() {
				(message.channelFavoriteCharacter, pfc),
				(message.channelFavoriteCharacter, null)
			};
			if (!(message.channelFavoriteCharacter != null && message.channelFavoriteCharacter.Length! > 0))
			{
				types.Add((pfc, null));
				types.Add(("abigail_williams*", null));
			}
			if (aca.Contains("-<testingmode>-")) {
				StringBuilder sb = new StringBuilder("testing fcs:");
				foreach (var type in types)
				{
					sb.AppendLine($"cfc: {type.Item1}, ffc: {type.Item2}");
				}
				await aca.Send(sb);
			}
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
			Console.WriteLine($"adding tags/removing bad tags");
			int triesIndex = 0;
			ImageData imageData = null;

			Console.WriteLine($"trying to find a picture");
			bool cfcavailable;
			do
			{
				List<string> tagz = null;
				string ww = "";
				if (types.Count > triesIndex)
				Console.WriteLine($"{triesIndex}: fc {types[triesIndex].Item1}, channel fc {types[triesIndex].Item2}");
				await AddTags(tagd, types.ToArray(), triesIndex,
					OnSuccess: (a, b) =>
					{
						tagz = a;
						ww = b;
						Console.WriteLine($"chose {ww}");
					});
				await aca.GetPicture(tagz.ToArray(),
					GotResult: imgdata =>
					{
						imageData = new ImageData()
						{
							pictureCharacter = ww,
							FileUrl = imgdata.FileUrl,
							PreviewUrl = imgdata.PreviewUrl,
							Tags = imgdata.Tags.ToArray(),
							Source = (imgdata.Source is string sos && sos.Length > 0 && sos != "noimagefound") ? sos : "No source",
							Nsfw = imgdata.Rating != BooruSharp.Search.Post.Rating.Safe
						};
						Console.WriteLine($"got a picture. nsfw: {imgdata.Rating != BooruSharp.Search.Post.Rating.Safe}");
					},
					OnFail: e =>
					{
						Console.WriteLine($"failed to get a picture: \n\n{e}\n");
						//triesIndex++;
					}
				);
				Console.WriteLine("--Trying again...--");
			} while (imageData == null && ++triesIndex < types.Count);
			
			if (imageData == null)
			{
				await aca.Send($"Master... I didn't find a {nick}ing picture of {pfc}");
				continue;
			}
			if (!message.isNSFW && imageData.Nsfw)
			{
				Console.WriteLine("server's nsfw is off");
				await aca.Send("Master that's a lewd image... I can't send it...");
				continue;
			}

			if (!message.isLoli && imageData.ContainsLoli)
			{
				Console.WriteLine("server loli is off");
				await aca.Send("Master... I found an image, but it's against discord's tos so i'm not going to send it.");
				continue;
			}

			int deleteTime = aca.guild.AutoDeleteTime;
			var adt = -1;
			if (imageData.ContainsLoli)
				adt = deleteTime / 2;
			else if (imageData.Nsfw)
				adt = deleteTime;

			var embed = GelEmbed.Build(aca,
				fileurl: imageData.FileUrl.ToString(),
				source: imageData.Source,
				fc: imageData.pictureCharacter,
				mentionedUsers: message.mentions,
				command: nick,
				user: message.user,
				autoDeleteTime: adt

			); 

			var embi = await aca.Send(embed);
			await QueueDelete(aca, adt, imageData, embi);
		}
	}

	private static async Task QueueDelete(AbbybotCommandArgs aca, int autoDeleteTime, ImageData imageData, Discord.Rest.RestUserMessage abbybotMessage)
	{
		if (autoDeleteTime > 0)
			await aca.Send(abbybotMessage, aca.originalMessage, RequestType.Delete, DateTime.Now.AddSeconds(autoDeleteTime));
	}

	private async Task AddTags(List<string> tags, (string, string)[] types, int index, Action<List<string>, string> OnSuccess, Action OnFailure=null  )
	{
		var tagz = tags.ToList();
		string ww = "";
		if (index >= types.Length)
		{
			OnFailure?.Invoke();
			return;
		}
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