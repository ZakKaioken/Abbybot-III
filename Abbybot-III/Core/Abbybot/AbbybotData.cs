using Abbybot_III.Sql.Abbybot.Abbybot;

using System.Linq;
using System.Threading.Tasks;

namespace Abbybot_III.Core.Abbybot
{
    class AbbybotData
    {
        public static async Task<bool> IsAbbybotHere(ulong guildId)
        {
            var abbybotids = await AbbybotSql.GetAbbybotIdAsync();
            abbybotids.Remove(Apis.Discord.Discord._client.CurrentUser.Id);
            bool b = false;
            foreach (var o in abbybotids)
            {
                var u = Apis.Discord.Discord._client.GetUser(o);
                b = u.MutualGuilds.ToList().Any(x => x.Id == guildId);
            }
            return b;
        }
    }
}