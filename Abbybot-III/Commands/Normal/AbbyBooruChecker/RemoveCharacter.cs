using Abbybot_III.Core.AbbyBooru.sql;
using Abbybot_III.Core.CommandHandler.extentions;
using Abbybot_III.Core.CommandHandler.Types;

using Discord;

using System.Text;
using System.Threading.Tasks;

namespace Abbybot_III.Commands.Normal.AbbyBooruChecker
{
    [Capi.Cmd("abbybot acremove", 5, 1)]
    class RemoveCharacter : NormalCommand
    {
        public override async Task DoWork(AbbybotCommandArgs a)
        {
            StringBuilder FavoriteCharacter = new StringBuilder(a.Message.Replace(Command, ""));

            if (FavoriteCharacter.Length < 1)
                return;
            while (FavoriteCharacter[0] == ' ')
                FavoriteCharacter.Remove(0, 1);
            while (FavoriteCharacter[FavoriteCharacter.Length - 1] == ' ')
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

            var o = new string[1];
            o[0] = FavoriteCharacter.ToString() + "*";

            EmbedBuilder eb = new EmbedBuilder();

            var u = a.abbybotUser;

            try
            {
                await AbbyBooruSql.RemoveCharacterAsync(a.channel, FavoriteCharacter.ToString());
                eb.Title = $"{fc} aww ok...";
                eb.Color = Color.Green;
                eb.Description = $"I removed the character from the channel {u.userNames.PreferedName} master...";
            }
            catch
            {
                eb.Title = $"silly!!! {fc}!!!";
                eb.Color = Color.Red;
                eb.Description = $"silly!! {fc} was not in the channel in the first place!!!";
            }
            await a.Send(eb);
        }

        public override async Task<string> toHelpString(AbbybotCommandArgs aca)
        {
            return $"remove a gelbooru tag feed from a channel";
        }
    }
}