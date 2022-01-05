
using Abbybot_III.Commands.Contains.GelbooruV4;
using Abbybot_III.Core.CommandHandler.extentions;
using Abbybot_III.Core.CommandHandler.Types;
using Abbybot_III.Core.Data.User;
using Abbybot_III.extentions;

using Discord;

using Nano.XML;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Abbybot_III.Commands.Contains.GelbooruV4.embed
{
	public class GelEmbed
	{

		public static EmbedBuilder GlobalBuild(GelbooruCommand cmd, Post result) {
			StringBuilder message = new();
			EmbedBuilder embededodizer = new();

			if ( result.fileUrl.Contains( new string[ ] { "mp4", "avi", "webm" } ) )
				embededodizer.Url = result.fileUrl;
			else
				embededodizer.ImageUrl = result.fileUrl;

			message.Append(cmd.message.pfc);
			message = message.Replace("* ~ ", " or ").Replace("* ", " and ").Replace("{", "").Replace("}", "").Replace("_", " ").Replace("*", "");
			while (message.Contains("**")) message.Replace("**", "*");
			string ufc = message.ToString();
			
			message.Clear();
			if (cmd.message.mentions != null)
				if (cmd.message.mentions.Count > 0)
				{
					message = MentionsEmbed(cmd.message.user, cmd.command, cmd.message.mentions);
				}


			string fixedsource = FixSource(result.source);
			embededodizer.AddField($"{ufc}  :)", $"[Image Source]({fixedsource})");
			embededodizer.Color = Color.LightOrange;
			embededodizer.Description = message.ToString();
			return embededodizer;
		}
		static StringBuilder MentionsEmbed(AbbybotUser user, string command, List<AbbybotUser> mentions)
		{
			if (mentions == null || mentions.Count == 0) return null;
			StringBuilder message = new StringBuilder();
			if (mentions.Count == 1 && mentions[0].Id == Apis.Discord.__client.CurrentUser.Id)
			{
				message.Append("You ");
				message.Append(command.Replace("abbybot ", ""));
				message.Append("ed me!! Thank you so much!!! **");
				message.Append(user.Preferedname);
				message.Append("**! <:abbyheart:699636931839000606> <a:AbbyHearts:829759075969531984>");
			}
			else if (user.Id == Apis.Discord.__client.CurrentUser.Id)
			{
				message.Append("I ");
				message.Append(command.Replace("abbybot ", ""));
				message.Append("ed you ");
				for (int hu = 0; hu < mentions.Count; hu++)
				{
					message.Append($"**{mentions[hu].Preferedname}**");
					if (mentions.Count - hu >= 1)
						message.Append(", ");
				}
				message.Append("!!! <a:abbyrich:731562550923100162> <:abbybearsquish:744219531454709820>");
			}
			else
			{
				message.Append(" Hey");

				for (int hu = 0; hu < mentions.Count; hu++)
				{
					message.Append($" **{mentions[hu].Preferedname}**");
					if (mentions.Count - hu >= 1)
						message.Append(", ");
				}

				message.Append("you were ");
				message.Append(command.Replace("abbybot ", ""));
				message.Append("ed by **");
				message.Append(user.Preferedname);
				message.Append("**! :)");
			}
			return message;
		}

		static string FixSource(string source)
		{
			//https://www.pixiv.net/en/artworks/77911151
			//http://www.pixiv.net/member_illust.php?mode=medium&amp;illust_id=66620949
			return !string.IsNullOrEmpty(source) ? source.Replace("/member_illust.php?mode=medium&amp;illust_id=", "/en/artworks/") : "";
		}
	}
}