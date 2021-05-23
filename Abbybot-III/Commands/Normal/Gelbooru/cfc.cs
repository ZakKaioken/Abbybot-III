using Abbybot_III.Commands.Contains.Gelbooru.embed;
using Abbybot_III.Core.CommandHandler.extentions;
using Abbybot_III.Core.CommandHandler.Types;
using Abbybot_III.Core.Users.sql;

using BooruSharp.Search.Post;

using Discord;

using System;
using System.Linq;

using System.Threading.Tasks;

namespace Abbybot_III.Commands.Normal.Gelbooru
{
	[Capi.Cmd("abbybot cfc", 1, 1)]
	class cfc : NormalCommand
	{
		public override async Task DoWork(AbbybotCommandArgs a)
		{
			var FavoriteCharacter =a.Replace(Command, true);
			var cfc = await ChannelFCOverride.GetFCMAsync(a.guild.Id, a.channel.Id);
			if (FavoriteCharacter.Length < 1)
			{
				if (cfc == "NO")
				{
					await a.Send($"Master... You didn't set the channel's cfc!!");
					return;
				}
				var fcfc = a.BreakAbbybooruTag(cfc);
				await a.Send($"The channel's favorite character is: {fcfc}\nFor help do ``{Command} help``");
				return;
			}

			while (FavoriteCharacter[0] == ' ')
				FavoriteCharacter.Remove(0, 1);
			while (FavoriteCharacter[^1] == ' ')
				FavoriteCharacter.Remove(FavoriteCharacter.Length - 1, 1);

			if (FavoriteCharacter.Length > 1)
			{
				string s = FavoriteCharacter.ToString();
				if (s == "help")
				{
					await a.Send($"To set the channel's favorite character use ``{Command} character name``\nTo remove the channel's favorite character do ``{Command} remove``\nYou can use the keywords **or** to randomly chose between multiple characters, **and** to have multiple characters in the same picture.\n**Tip:** You can use any gelbooru tag as the favorite character!");
					return;
				}
				if (s == "remove")
				{
					await ChannelFCOverride.SetCFCAsync(a.guild.Id, a.channel.Id, "NO");
					return;
				}
			}

			string fc = FavoriteCharacter.ToString();
			a.BuildAbbybooruTag(FavoriteCharacter);
			var o = new string[1];
			o[0] = FavoriteCharacter.ToString();

			var canr = await Fc.awa(a, o);
			fc = canr.fc;
			EmbedBuilder eb = new EmbedBuilder();
			eb.ImageUrl = canr.pictureurl.AbsoluteUri;
			var u = a.user;
			if (canr.canrun)
			{
				await ChannelFCOverride.SetCFCAsync(a.guild.Id, a.channel.Id, FavoriteCharacter.ToString());

				eb.Title = $"{fc} Yayy!!";
				eb.Color = Color.Green;
				eb.Description = $"I set the channels favorite character hehehehe!!! cutie master!! {u.Preferedname} master!! ";
			}
			else
			{
				eb.Title = $"oof... {fc}...";
				eb.Color = Color.Red;
				eb.Description = $"sorry {u.Preferedname}... i couldn't find {fc} ({FavoriteCharacter}) ...";
			}
			await a.Send(eb);
		}

		public override async Task<string> toHelpString(AbbybotCommandArgs aca)
		{
			return $"Set the channel's favorite character";
		}
	}
}