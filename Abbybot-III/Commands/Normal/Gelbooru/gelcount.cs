using Abbybot_III.Apis;
using Abbybot_III.Core.CommandHandler.extentions;
using Abbybot_III.Core.CommandHandler.Types;
using Abbybot_III.Core.Users.sql;

using Capi.Interfaces;

using Discord;

using System.Collections.Generic;
using System.Linq;

using System.Threading.Tasks;

namespace Abbybot_III.Commands.Normal.Gelbooru
{
	[Capi.Cmd("abbybot gelcount", 1, 1)]
	class gelcount : NormalCommand
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
			while (tagss[0] == ' ')
				tagss.Remove(0, 1);
			while (tagss[^1] == ' ')
				tagss.Remove(tagss.Length - 1, 1);

			if (a.user != null)
			{
				var fc = a.user.FavoriteCharacter;
				tagss.Replace("&fc", $"{fc}*");
			}
			a.BuildAbbybooruTag(tagss);

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
				if (!sgc.IsNsfw || !a.user.IsLewd || !ratings.Contains(CommandRatings.hot))
				{
					tags.Add("rating:safe");
				}
			}
			try
			{
				int o = await AbbyBooru.GetPostCount(tags.ToArray());
				await a.Send($"There are {o} posts by those tags.");
			}
			catch { }
		}

		public override async Task<string> toHelpString(AbbybotCommandArgs aca)
		{
			return $"a random picture finder, 1 to 1 ratio to gelbooru's own search bar";
		}
	}
}