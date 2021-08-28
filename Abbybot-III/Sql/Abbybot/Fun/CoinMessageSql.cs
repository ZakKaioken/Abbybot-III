using System;
using System.Threading.Tasks;

namespace Abbybot_III.Sql.Abbybot.Fun
{
    public class CoinMessageSql
    {
        public static async Task<string> GetMessage(bool nsfw) {
            var n = nsfw?1:0;
            var messages = await AbbySql.AbbysqlClient.FetchSQL($"SELECT * FROM CoinMessages WHERE `nsfw`='{n}'");
            if (messages.Count == 0) return null;

            Random r = new Random(); 
            return messages[r.Next(0, messages.Count)]["Message"] is string s ? s : null;
        }
    }
}