using Abbybot_III.Commands.Contains.Gelbooru.dataobject;
using Abbybot_III.Core.Data.User;
using Abbybot_III.extentions;

using Discord;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abbybot_III.Commands.Contains.Gelbooru.embed
{
    public class GelEmbed
    {
        public static EmbedBuilder Build(ImgData imgdata, StringBuilder sb)
        {
            StringBuilder message = new StringBuilder();

            EmbedBuilder embededodizer = new EmbedBuilder();
            var iu = imgdata.Imageurl;
            if (iu.Contains(new string[] { "mp4", "avi", "webm" }))
                embededodizer.Url = iu;
            else
                embededodizer.ImageUrl = iu;
            string fcn = fcbuilder(sb.ToString());

            message.Clear();
            MentionsEmbed(imgdata, message);
            string fixedsource = FixSource(imgdata.source);
            embededodizer.AddField($"{fcn}  :)", $"[Image Source]({fixedsource})");
            embededodizer.Color = Color.LightOrange;
            embededodizer.Description = message.ToString();

            return embededodizer;
        }

        static void MentionsEmbed(ImgData imgd, StringBuilder message)
        {
            if (imgd.mentions != null)
            {
                if (imgd.mentions.Count > 0)
                {
                    message.Append(" Hey");

                    for (int hu = 0; hu < imgd.mentions.Count; hu++)
                    {
                        message.Append($" **{imgd.mentions[hu].userNames.PreferedName}**");
                        if (imgd.mentions.Count - hu >= 1)
                            message.Append(", ");
                    }

                    message.Append("you were ");
                    message.Append(imgd.command.Replace("abbybot ", ""));
                    message.Append("ed by **");
                    message.Append(imgd.user.userNames.PreferedName);
                    message.Append("**! :)");
                }
            }
        }

        static string FixSource(string source)
        {
            //https://www.pixiv.net/en/artworks/77911151
            //http://www.pixiv.net/member_illust.php?mode=medium&amp;illust_id=66620949
            if (!string.IsNullOrEmpty(source))
                return source.Replace("/member_illust.php?mode=medium&amp;illust_id=", "/en/artworks/");
            return "";
        }
        public static string fcbuilder(string s)
        {
            return s.Replace("* ~ ", " or ").Replace("* ", " and ").Replace("{", "").Replace("}", "").Replace("_", " ").Replace("*","");
        }
        public static EmbedBuilder Build(string fileurl, string source, string fc, List<Core.Data.User.AbbybotUser> mentionedUsers, string command)
        {
            StringBuilder message = new StringBuilder();

            EmbedBuilder embededodizer = new EmbedBuilder();
            var iu = fileurl;
            if (iu.Contains(new string[] { "mp4", "avi", "webm" }))
                embededodizer.Url = iu;
            else
                embededodizer.ImageUrl = iu;
            string fcn = fcbuilder(fc.ToString());

            message.Clear();
            //MentionsEmbed(imgd, message);
            string fixedsource = FixSource(source);
            embededodizer.AddField($"{fcn}  :)", $"[Image Source]({fixedsource})");
            embededodizer.Color = Color.LightOrange;
            embededodizer.Description = message.ToString();

            return embededodizer;
        }

        public static EmbedBuilder Build(ImgData imgdrata)
        {
                StringBuilder message = new StringBuilder();
            EmbedBuilder embededodizer = new EmbedBuilder();

            var iu =new Uri(imgdrata.Imageurl).ToString();
            if (iu.Contains(new string[] { "mp4", "avi", "webm" }))
                message.AppendLine(iu).Replace("%20", "\\%20");

            else
                try
                {
                    embededodizer.ImageUrl = iu;
                } catch
                {
                    message.AppendLine(iu);
                }
            string fcn = fcbuilder(imgdrata.favoritecharacter.ToString());

                MentionsEmbed(imgdrata, message);
                string fixedsource = FixSource(imgdrata.source);
                embededodizer.AddField($"{fcn}  :)", $"[Image Source]({fixedsource})");
                embededodizer.Color = Color.LightOrange;
                embededodizer.Description = message.ToString();

                return embededodizer;
            
        }
    }
}
