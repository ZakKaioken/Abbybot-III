using Abbybot_III.Apis.Booru;
using Abbybot_III.Core.CommandHandler.extentions;
using Abbybot_III.Core.CommandHandler.Types;
using Abbybot_III.Core.Users.sql;

using Capi.Interfaces;

using Discord;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abbybot_III.Commands.Normal.Gelbooru
{
	[Capi.Cmd("abbybot geltags", 1, 1)]
	class geltags : NormalCommand
	{
		public override async Task DoWork(AbbybotCommandArgs a)
		{
			StringBuilder tagss = new StringBuilder(a.Message.Replace(Command, ""));
			if (tagss.Length < 1)
			{
				await a.Send("You gotta tell me some tags too silly!!!");
				return;
			}
			while (tagss[0] == ' ')
				tagss.Remove(0, 1);
			while (tagss[^1] == ' ')
				tagss.Remove(tagss.Length - 1, 1);

			var fc = a.user.FavoriteCharacter;
			tagss.Replace("&fc", $"{fc}");

			Fc.FCBuilder(tagss).Replace("*", "");

			var tags = tagss.ToString().Split(' ').ToList();

			List<string> sts = new List<string>();
			foreach (var t in tags)
			{
				sts.Add($"{t}");
			}
			tags = sts;
			/*
			var badtaglisttags = await UserbadtaglistSql.GetbadtaglistTags(a.abbybotUser.Id);

			foreach (var item in badtaglisttags)
			{
				tags.Add($"-{item}");
			}
			*/
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
				var o = await AbbyBooru.GetTagData(tags.ToArray());

				EmbedBuilder eb = new EmbedBuilder();

				eb.Title = ("Here's what i found");
				eb.Color = Color.Purple;
				Abbybot.print(o.Count);
				foreach (var ooooo in o)
				{
					EmbedFieldBuilder efb = new EmbedFieldBuilder();
					efb.IsInline = true;
					efb.Name = "\u200b";
					efb.Value = $"({ooooo.Type}) *{ooooo.Name.Replace("_", "\\_")}*";
					eb.AddField(efb);
				}

				await a.Send(eb);
			}
			catch { }
		}

		public override async Task<string> toHelpString(AbbybotCommandArgs aca)
		{
			return $"a random picture finder, 1 to 1 ratio to gelbooru's own search bar";
		}
	}
}