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
				await a.GetPicture(tags.ToArray(), s =>
				{
					ImgData im = new ();
					if (s.FileUrl != null)
						im.Imageurl = s.FileUrl.ToString();
					if (s.Source != null)
						im.source = s.Source;

					eb = Contains.Gelbooru.embed.GelEmbed.Build(a, im, new StringBuilder("abbybot"));
				});

			await a.Send(eb);
		}


		public override async Task<string> toHelpString(AbbybotCommandArgs aca)
		{
			return $"a random picture finder, 1 to 1 ratio to gelbooru's own search bar";
		}
	}
}