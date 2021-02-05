﻿using Abbybot_III.Apis.Booru;
using Abbybot_III.Commands.Contains.Gelbooru.embed;
using Abbybot_III.Core.CommandHandler.extentions;
using Abbybot_III.Core.CommandHandler.Types;
using Abbybot_III.Core.Users.sql;

using BooruSharp.Search.Post;

using Discord;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abbybot_III.Commands.Normal.Gelbooru
{
    [Capi.Cmd("abbybot fc", 1, 1)]
    class fc : NormalCommand
    {
        public override async Task DoWork(AbbybotCommandArgs a)
        {
            StringBuilder FavoriteCharacter = new StringBuilder(a.Message.ToLower().Replace(Command.ToLower(), ""));

            if (FavoriteCharacter.Length < 1)
            {

                var fcfc = GelEmbed.fcbuilder(a.abbybotUser.userFavoriteCharacter.FavoriteCharacter);
                await a.Send($"Your favorite character is: {fcfc}");
                return;
            }
            while (FavoriteCharacter[0] == ' ')
                FavoriteCharacter.Remove(0, 1);
            while (FavoriteCharacter[^1] == ' ')
                FavoriteCharacter.Remove(FavoriteCharacter.Length - 1, 1);
            string fc = FavoriteCharacter.ToString();

            FavoriteCharacter.Replace(" ", "_").Replace("abbybot", "abigail_williams").Replace("abby", "abigail_williams").Replace("abby_kaioken", "abigail_williams");
            if (FavoriteCharacter[^1] != '~')
                FavoriteCharacter.Append("*");
            else
                FavoriteCharacter.Remove(FavoriteCharacter.Length - 1, 1);
            if (FavoriteCharacter.ToString().Contains("_~_") || FavoriteCharacter.ToString().Contains("_or_"))
            {
                FavoriteCharacter.Insert(0, "{").Append("}");
                FavoriteCharacter.Replace("~_or_", " ~ ").Replace("~_~_", " ~ ").Replace("_~_", "* ~ ").Replace("_or_", "* ~ ");
            }
            FavoriteCharacter.Replace("~_&&_", " ").Replace("~_and_", " ").Replace("_&&_", "* ").Replace("_and_", "* ");

            string pictureurl = "https://img2.gelbooru.com/samples/ee/e2/sample_eee286783bfa37e088d1ffbcf8f098ba.jpg";
            var o = new string[1];
            o[0] = FavoriteCharacter.ToString();

            bool canrun = false;
            int tries = 0;
            do
            {
                try
                {


                    SearchResult imgdata = await AbbyBooru.Execute(o);

                    if (imgdata.Rating == BooruSharp.Search.Post.Rating.Safe)
                        pictureurl = imgdata.FileUrl.ToString();
                    else
                    {
                        var e = o.ToList();
                        e.Add(" rating:safe ");
                        try
                        {
                            var i = await AbbyBooru.Execute(e.ToArray());
                            pictureurl = i.FileUrl.ToString();
                        }
                        catch { }
                    }

                    canrun = true;
                }
                catch
                {
                    canrun = false;
                }
                tries++;
            } while (tries <= 3);
            EmbedBuilder eb = new EmbedBuilder();
            var urirl = new Uri(pictureurl).AbsoluteUri;
            eb.ImageUrl = urirl;
            var u = a.abbybotUser;
            if (canrun)
            {

                await FavoriteCharacterSql.SetFavoriteCharacterAsync(u.Id, FavoriteCharacter.ToString());

                eb.Title = $"{fc} Yayy!!";
                eb.Color = Color.Green;
                eb.Description = $"I know your favorite character now hehehehe cutie {u.userNames.PreferedName} master!! ";
            }
            else
            {
                eb.Title = $"oof... {fc}...";
                eb.Color = Color.Red;
                eb.Description = $"sorry {u.userNames.PreferedName}... i couldn't find {fc} ({FavoriteCharacter}) ...";
            }
            await a.Send(eb);
        }

        public override async Task<string> toHelpString(AbbybotCommandArgs aca)
        {
            return $"set your favorite character. (all pictures are of your favorite character)";
        }
    }
}
