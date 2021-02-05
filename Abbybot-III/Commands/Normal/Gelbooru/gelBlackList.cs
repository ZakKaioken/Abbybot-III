using Abbybot_III.Apis.Booru;
using Abbybot_III.Core.CommandHandler.extentions;
using Abbybot_III.Core.CommandHandler.Types;
using Abbybot_III.Core.Users.sql;

using Discord;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Abbybot_III.Commands.Normal.Gelbooru
{
        [Capi.Cmd("abbybot blacklisttag", 1, 1)]
        class blackListTag : NormalCommand
        {
            public override async Task DoWork(AbbybotCommandArgs message)
            {
                StringBuilder FavoriteCharacter = new StringBuilder(message.Message.Replace(Command, ""));

                while (FavoriteCharacter[0] == ' ')
                    FavoriteCharacter.Remove(0, 1);
                List<string> tags = new List<string>();
                FavoriteCharacter = FavoriteCharacter.Replace(" ", "_");
                string fc = FavoriteCharacter.ToString().ToLower();
                foreach (var item in fc.Replace("_and_", "&&").Replace(",", "&&").Split("&&"))
                {
                    FavoriteCharacter.Clear().Append(item);
                    while (FavoriteCharacter[0] == '_')
                        FavoriteCharacter.Remove(0, 1);
                    while (FavoriteCharacter[^1] == '_')
                        FavoriteCharacter.Remove(FavoriteCharacter.Length - 1, 1);
                    tags.Add(FavoriteCharacter.ToString());
                }
                string reason = "";
                FavoriteCharacter.Clear();
                List<string> blt = new List<string>();
                foreach (var item in tags)
                {
                    try
                    {
                        bool canrun;
                        try
                        {
                            BooruSharp.Search.Post.SearchResult imgdata = await AbbyBooru.Execute(new string[] { item });
                        }
                        catch
                        {
                            throw new Exception("this tag doesn't have images. I can't add it to your blacklist.");
                        }

                        bool addedtag = await UserBlacklistSql.BlackListTag(message.abbybotUser.Id, item);
                        if (addedtag)
                        {
                            blt.Add(item);
                            FavoriteCharacter.Append($"{item} ");
                        }

                    }
                    catch (Exception ecx)
                    {
                        reason = ecx.Message;
                    }
                }
                //FavoriteCharacter.AppendJoin(",", tags);
                EmbedBuilder eb = new EmbedBuilder();

                if (blt.Count > 0)
                {

                    eb.Title = $"{fc} Yayy!!";
                    eb.Color = Color.Green;

                    eb.Description = $"I added tags {FavoriteCharacter} to your gel blacklist cutie {message.abbybotUser.userNames.PreferedName} master!! ";
                }
                else
                {
                    eb.Title = reason;
                    eb.Color = Color.Red;
                    eb.ImageUrl = "https://cdn.discordapp.com/avatars/595308053448884294/69542a3eb0866c37f33aa63704fe3726.png";
                    eb.Description = $"sorry {message.abbybotUser.userNames.PreferedName} master...\n";
                }

                await message.Send(eb);
            }
        public override async Task<string> toHelpString(AbbybotCommandArgs aca)
        {
            return $"blacklist tags you don't like. Personally, i hate large breasts, but you do you.";
        }
    }


}
