using Abbybot_III.Commands.Contains.Gelbooru.dataobject;
using Abbybot_III.Core.CommandHandler.extentions;
using Abbybot_III.Core.CommandHandler.Types;
using Abbybot_III.Core.Users.sql;

using Capi.Interfaces;

using Discord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abbybot_III.Commands.Normal.Gelbooru
{
	[Capi.Cmd("abbybot gel", 1, 1)]
	class gel : NormalCommand
	{
		public override async Task DoWork(AbbybotCommandArgs a)
		{
			//StringBuilder tagss = new StringBuilder(a.Message.Replace(Command, ""));
			var tagss = a.Replace(Command);
			if (tagss.Length < 1)
			{
				await a.Send("You gotta tell me some tags too silly!!!");
				return;
			}

			var fc = a.user.FavoriteCharacter;
			tagss.Replace("&fc", $"{fc}*");

			var tags = tagss.ToString().Split(' ').ToList();

			var badtaglisttags = await UserBadTagListSql.GetbadtaglistTags(a.user.Id);

			foreach (var item in badtaglisttags)
			{
				tags.Add($"-{item}");
			}

			if (a.guild != null)
			{
				var ratings = a.user.Ratings;
				var sgc = (ITextChannel)a.channel;
				if (sgc == null) return;
				if (!a.IsChannelNSFW || !a.user.IsLewd || !a.user.HasRatings(CommandRatings.hot))
				{
					tags.Add("rating:safe");
				}
			}
			EmbedBuilder eb = null;
			try
			{
				BooruSharp.Search.Post.SearchResult imgdata = await a.GetPicture(tags);
				ImgData im = (new ImgData { });

				if (imgdata.FileUrl != null)
				{
					im.Imageurl = imgdata.FileUrl.ToString();
				}
				if (imgdata.Source != null)
				{
					im.source = imgdata.Source;
				}

				try
				{
					eb = Contains.Gelbooru.embed.GelEmbed.Build(a,im, new StringBuilder("abbybot"));
				}
				catch (Exception e)
				{
					Console.WriteLine(e.Message+e.StackTrace);
					eb = new EmbedBuilder { Description = "It didn't work... :(" };
				}
			}
			catch { }

			await a.Send(eb);
		}


		public override async Task<string> toHelpString(AbbybotCommandArgs aca)
		{
			return $"a random picture finder, 1 to 1 ratio to gelbooru's own search bar";
		}
	}
}