using Abbybot_III.Apis.Booru;
using Abbybot_III.Core.AbbyBooru.sql;
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

namespace Abbybot_III.Commands.Normal.AbbyBooruChecker
{
    [Capi.Cmd("%acadd", 5, 1)]
    class AddCharacter : NormalCommand
    {

        public override async Task DoWork(AbbybotCommandArgs a)
        {
            StringBuilder FavoriteCharacter = new StringBuilder(a.Message.Replace(Command, ""));
            Console.WriteLine("acadd called");
            if (FavoriteCharacter.Length < 1)
                return;
            while (FavoriteCharacter[0] == ' ')
                FavoriteCharacter.Remove(0, 1);
            while (FavoriteCharacter[FavoriteCharacter.Length - 1] == ' ')
                FavoriteCharacter.Remove(FavoriteCharacter.Length - 1, 1);

            string fc = FavoriteCharacter.ToString();

            FavoriteCharacter.Replace(" ", "_").Replace("abbybot", "abigail_williams").Replace("abby", "abigail_williams").Replace("abby_kaioken", "abigail_williams");

            if (FavoriteCharacter.ToString().Contains("_~_") || FavoriteCharacter.ToString().Contains("_or_"))
            {
                FavoriteCharacter.Insert(0, "{").Append("}");
                FavoriteCharacter.Replace("_~_", " ~ ").Replace("_or_", " ~ ");
            }

            FavoriteCharacter.Replace("_&&_", "* ").Replace("_and_", "* ");
            string pictureurl = "https://img2.gelbooru.com/samples/ee/e2/sample_eee286783bfa37e088d1ffbcf8f098ba.jpg";
            var o = new string[1];
            o[0] = FavoriteCharacter.ToString() + "*";
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
            eb.ImageUrl = pictureurl;
            var u = a.abbybotUser;
            if (canrun)
            {
                try {
                await AbbyBooruSql.AddCharacterAsync(a.channel, a.abbybotGuild, FavoriteCharacter+"*");
                } catch (Exception e)
                {
                    Console.WriteLine(e.Message);

                    eb.Title = $"silly!!! {fc}!!!";
                    eb.Color = Color.Red;
                    eb.Description = $"silly!! {fc} was already added to this channel!!";
                    await a.Send(eb);
                    return;
                }
                eb.Title = $"{fc} Yayy!!";
                eb.Color = Color.Green;
                eb.Description = $"I added {fc} to the channel master!! ";
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
            return $"{Command}: lets you add a character to the Auto Character Poster on this channel. Usage ({Command} abigail williams) to add the character abigail williams";
        }

    }
}
