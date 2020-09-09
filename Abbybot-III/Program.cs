using Abbybot_III.Apis.Twitter;
using Abbybot_III.Core.AbbyBooru;
using Abbybot_III.Core.Heart;
using Abbybot_III.Core.Twitter;

using System;
using System.Threading.Tasks;

namespace Abbybot_III
{
    class Program
    {
        static async Task Main()
        {
            Twitter.init();
            AbbyBooruChecker.Init();
            AbbybotTwitter.init();
            AbbySql.AbbysqlClient.connectionstring = Apis.Mysql.ApiKeys.MysqlApiKeys.Load(@"ApiKeys\Mysql.json").ToString();
            AbbyHeart.Start();
            await Apis.Discord.Discord.DiscordMainAsync();
            Console.ReadLine(); 
        }
    }
}
