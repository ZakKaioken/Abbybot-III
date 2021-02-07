using Abbybot_III.Core.CommandHandler.extentions;
using Abbybot_III.Core.CommandHandler.Types;
using Abbybot_III.Core.LevelingManager;
using Abbybot_III.Core.Users.sql;
using Abbybot_III.Sql.Abbybot.User;

using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abbybot_III.Commands.Contains
{
    [Capi.Cmd("abbybot me", 1, 1)]
    class Me : ContainCommand
    {
        public override bool SelfRun { get => false; set => base.SelfRun = value; }

        public override async Task DoWork(AbbybotCommandArgs abd)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"**{abd.abbybotUser.userNames.PreferedName}**'s profile:");
            sb.Append($"Username: {abd.abbybotUser.userNames.Username}");
            if (abd.abbybotUser.userNames.Nickname != null)
                if (abd.abbybotUser.userNames.Nickname.Length > 0)
                    sb.Append($", Nickname: {abd.abbybotUser.userNames.Nickname}");
            sb.Append("\n");
            sb.AppendLine($"Your favorite character is: {abd.abbybotUser.userFavoriteCharacter.FavoriteCharacter}");
            if (abd.abbybotUser.userMarry.MarriedUserId != 0)
            {
                var married = await UserSql.GetUser(abd.abbybotUser.Id);
                sb.AppendLine($"You're married to {married.userNames.PreferedName}");
            }
            else
            {
                sb.AppendLine("You're not married to anyone.");
            }
            sb.Append("AutoFcDms: ");
            if (await AutoFcDmSqls.GetAutoFcDmAsync(abd.abbybotUser.Id))
                sb.Append("✅ ");
            else
                sb.Append("❌ ");

            sb.Append("FCMentions: ");
            if (await FCMentionsSql.GetFCMAsync(abd.abbybotUser.Id))
                sb.Append("✅ ");
            else
                sb.Append("❌ ");

            sb.Append("\n");
            var client = Apis.Discord.Discord._client;
            var abbybotid = client.CurrentUser.Id;
            if (abd.abbybotGuild != null)
            {
                sb.Append("Your favorite channel in this server is: ");

                var MSC = await PassiveUserSql.GetChannelsinGuildStats(abbybotid, abd.abbybotGuild.GuildId, abd.abbybotUser.Id, "MessagesSent");
                var orderedlist = MSC.OrderBy(x => x.stat).ToList()[0];
                var chan = client.GetGuild(abd.abbybotGuild.GuildId).GetChannel(orderedlist.channel);
                sb.AppendLine(chan.Name);

                ulong i = 0;
                foreach (var sta in MSC)
                {
                    i += sta.stat;
                }
                var e = LevelCalculator.CalculateStatLevel(i, "Messagessent");

                sb.AppendLine($"You are level {e.level}. ({e.exp}/{e.expleft})");
            }

            await abd.Send(sb.ToString());
        }

        public override async Task<bool> ShowHelp(AbbybotCommandArgs aca)
        {
            return true;
        }

        public override async Task<string> toHelpString(AbbybotCommandArgs aca)
        {
            return $"Check your profile!!";
        }
    }
}