﻿using Abbybot_III.Commands.Contains.Gelbooru.dataobject;
using Abbybot_III.Core.Data.User;

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

        private static void MentionsEmbed(ImgData imgd, StringBuilder message)
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
                    message.Append(imgd.command.Replace("%", ""));
                    message.Append("ed by **");
                    message.Append(imgd.user.userNames.PreferedName);
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

        internal static EmbedBuilder Build(string fileurl, string source, string fc, List<Core.Data.User.AbbybotUser> mentionedUsers, string command)
        {
            StringBuilder message = new StringBuilder();
            EmbedBuilder embededodizer = new EmbedBuilder
            {
                ImageUrl = fileurl
            };
            string fcn = fc.ToString().Replace("* ", " and ").Replace(" ~ ", " or ").Replace("{", "").Replace("}", "").Replace("_", " ");

            message.Clear();
            //MentionsEmbed(imgd, message);
            string fixedsource = FixSource(source);
            embededodizer.AddField($"{fcn}  :)", $"[Image Source]({fixedsource})");
            embededodizer.Color = Color.LightOrange;
            embededodizer.Description = message.ToString();

            return embededodizer;
        }

        internal static EmbedBuilder Build(ImgData imgdrata)
        {
                StringBuilder message = new StringBuilder();
                EmbedBuilder embededodizer = new EmbedBuilder
                {
                    ImageUrl = imgdrata.Imageurl
                };
                string fcn = imgdrata.favoritecharacter.ToString().Replace("* ", " and ").Replace(" ~ ", " or ").Replace("{", "").Replace("}", "").Replace("_", " ");

                message.Clear();
                MentionsEmbed(imgdrata, message);
                string fixedsource = FixSource(imgdrata.source);
                embededodizer.AddField($"{fcn}  :)", $"[Image Source]({fixedsource})");
                embededodizer.Color = Color.LightOrange;
                embededodizer.Description = message.ToString();

                return embededodizer;
            
        }
    }
}
