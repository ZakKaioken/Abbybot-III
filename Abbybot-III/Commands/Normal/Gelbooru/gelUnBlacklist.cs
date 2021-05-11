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
    [Capi.Cmd("abbybot unblacklisttag", 1, 1)]
    class UnBlackListTag : NormalCommand
    {
        public override async Task DoWork(AbbybotCommandArgs message)
        {
            StringBuilder FavoriteCharacter = new StringBuilder(message.Message.Replace(Command, ""));

            while (FavoriteCharacter[0] == ' ')
                FavoriteCharacter.Remove(0, 1);
            while (FavoriteCharacter[^1] == ' ')
                FavoriteCharacter.Remove(FavoriteCharacter.Length - 1, 1);

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
            List<string> failedblt = new List<string>();
            foreach (var item in tags)
            {
                try
                {
                    bool addedtag = await UserBlacklistSql.UnBlackListTag(message.abbybotUser.Id, item);
                    if (addedtag)
                    {
                        blt.Add(item);
                        FavoriteCharacter.Append($"{item} ");
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    failedblt.Add(item);
                }
            }
            //FavoriteCharacter.AppendJoin(",", tags);
            EmbedBuilder eb = new EmbedBuilder();
            var ff = new StringBuilder();
            if (failedblt.Count > 0)
            {
                for (int i = 0; i < failedblt.Count; i++)
                {
                    string b = failedblt[i];
                    ff.Append(b);
                    if (i != failedblt.Count - 1)
                    {
                        ff.Append(", ");
                    }
                }
            }
            StringBuilder fcmaa = new StringBuilder();
            if (blt.Count > 0)
            {
                eb.Title = $"{fc} Yayy!!";
                eb.Color = Color.Green;
                fcmaa.Append($"I removed tags {FavoriteCharacter} from your gel blacklist cutie {message.abbybotUser.userNames.PreferedName} master!!");
                if (ff.Length > 0) fcmaa.Append(" (").Append(ff).Append(")");

                eb.Description = fcmaa.ToString();
            }
            else
            {
                eb.Title = reason;
                eb.Color = Color.Red;
                eb.ImageUrl = "https://cdn.discordapp.com/avatars/595308053448884294/69542a3eb0866c37f33aa63704fe3726.png";
                fcmaa.Append($"sorry {message.abbybotUser.userNames.PreferedName} master...");
                fcmaa.Append(" I could not remove these tags: ").Append(ff).Append("...");
                eb.Description = fcmaa.ToString();
            }

            await message.Send(eb);
        }

        public override async Task<string> toHelpString(AbbybotCommandArgs aca)
        {
            return await Task.FromResult($"remove blacklisted tags you blacklisted.");
        }
    }
}