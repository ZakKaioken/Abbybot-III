using Abbybot_III.Commands.Contains.Gelbooru.dataobject;

using Discord;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Abbybot_III.Commands.Contains.Gelbooru.embed
{
    public class GelEmbed
    {
        public static EmbedBuilder Build(ImgData imgdata, StringBuilder sb)
        {
            StringBuilder message = new StringBuilder();
            EmbedBuilder embededodizer = new EmbedBuilder
            {
                ImageUrl = imgdata.Imageurl
            };
            string fcn = sb.ToString().Replace("* ", " and ").Replace(" ~ ", " or ").Replace("{", "").Replace("}", "").Replace("_", " ");

            message.Clear();
            MentionsEmbed(imgdata, message);
            string fixedsource = FixSource(imgdata.source);
            embededodizer.AddField($"{fcn}  :)", $"[Image Source]({fixedsource})");
            embededodizer.Color = Color.LightOrange;
            embededodizer.Description = message.ToString();

            return embededodizer;
        }

        private static void MentionsEmbed(ImgData abca, StringBuilder message)
        {
            if (abca.mentions != null)
            {
                var mentions = abca.mentions.ToList();
                if (mentions.Count > 0)
                {
                    message.Append(" Hey");

                    for (int hu = 0; hu < mentions.Count; hu++)
                    {
                        message.Append($" **{mentions[hu].userNames.PreferedName}**");
                        if (mentions.Count - hu >= 1)
                            message.Append(", ");
                    }

                    message.Append("you were ");
                    message.Append(abca.command.Replace("%", ""));
                    message.Append("ed by **");
                    var us = abca.sudouser != null ? abca.sudouser : abca.user;
                    message.Append(us.userNames.PreferedName);
                    message.Append("**! :)");
                }
            }
        }

        private static string FixSource(string source)
        {
            //https://www.pixiv.net/en/artworks/77911151
            //http://www.pixiv.net/member_illust.php?mode=medium&amp;illust_id=66620949
            if (!string.IsNullOrEmpty(source))
                return source.Replace("/member_illust.php?mode=medium&amp;illust_id=", "/en/artworks/");
            return "";
        }


    }
}
