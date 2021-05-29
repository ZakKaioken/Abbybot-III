using Abbybot_III.Commands.Contains.Gelbooru.dataobject;
using Abbybot_III.Core.CommandHandler.Types;
using Abbybot_III.Core.Data.User;
using Abbybot_III.extentions;

using Discord;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Abbybot_III.Commands.Contains.Gelbooru.embed
{
	public class GelEmbed
	{
		public static EmbedBuilder Build(AbbybotCommandArgs a, ImgData imgdata, StringBuilder sb)
		{
			StringBuilder message = new StringBuilder();

			EmbedBuilder embededodizer = new EmbedBuilder();
			var iu = imgdata.Imageurl;
			if (iu.Contains(new string[] { "mp4", "avi", "webm" }))
				embededodizer.Url = iu;
			else
				embededodizer.ImageUrl = iu;
			string fcn = a.BreakAbbybooruTag(sb).ToString();

			message.Clear();
			if (imgdata.mentions != null)
			if (imgdata.mentions.Count > 0)
			{
				message = MentionsEmbed(imgdata.user, imgdata.command, imgdata.mentions);
			}
			string fixedsource = FixSource(imgdata.source);
			embededodizer.AddField($"{fcn}  :)", $"[Image Source]({fixedsource})");
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
			if (!string.IsNullOrEmpty(source))
				return source.Replace("/member_illust.php?mode=medium&amp;illust_id=", "/en/artworks/");
			return "";
		}

		

		public static EmbedBuilder Build(AbbybotCommandArgs a,  string fileurl, string source, string fc, List<Core.Data.User.AbbybotUser> mentionedUsers, string command, AbbybotUser user)
		{
			StringBuilder message = new StringBuilder();

			EmbedBuilder embededodizer = new EmbedBuilder();

			var iu = fileurl;
			if (iu.Contains(new string[] { "mp4", "avi", "webm" }))
				embededodizer.Url = iu;
			else
				embededodizer.ImageUrl = iu;
			string fcn = a.BreakAbbybooruTag(fc.ToString());

			message.Clear();
			string fixedsource = FixSource(source);
			if (mentionedUsers.Count > 0)
			{
				message = MentionsEmbed(user, command, mentionedUsers);
			}
			embededodizer.AddField($"{fcn}  :)", $"[Image Source]({fixedsource})");
			embededodizer.Color = Color.LightOrange;
			embededodizer.Description = message.ToString();

			return embededodizer;
		}

		public static EmbedBuilder Build(AbbybotCommandArgs aca, ImgData imgdrata, bool found, bool rolling)
		{
			StringBuilder message = new StringBuilder();
			EmbedBuilder embededodizer = new EmbedBuilder();

			var iu = new Uri(imgdrata.Imageurl).ToString();
			if (iu.Contains(new string[] { "mp4", "avi", "webm" }))
				message.AppendLine(iu).Replace("%20", "\\%20");
			else
				try
				{
					embededodizer.ImageUrl = iu;
				}
				catch
				{
					message.AppendLine(iu);
				}
			string fcn = aca.BreakAbbybooruTag(imgdrata.favoritecharacter.ToString());

			string fixedsource = FixSource(imgdrata.source);
			string title = ((rolling) ? "r" : "") + $"{fcn} :)";
			if (found)
				embededodizer.AddField($"{fcn}  :)", $"[Image Source]({fixedsource})");
			else
				embededodizer.AddField($"I couldn't find a {imgdrata.command.ReplaceA("abbybot ", "")} picture of {fcn}...  :o", $"[Image Source]({fixedsource})");
			embededodizer.Color = Color.LightOrange;
			if (imgdrata.mentions.Count > 0)
				message = MentionsEmbed(imgdrata.user, imgdrata.command, imgdrata.mentions);

			embededodizer.Description = message.ToString();

			return embededodizer;
		}
	}
}