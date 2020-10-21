using Abbybot_III.Apis.Mysql;
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
            MysqlCore.CheckMysql(@"ApiKeys\mysqlbinpath.abbytxt");
            await InitAll();
            await Apis.Discord.Discord.DiscordMainAsync();
            AbbyHeart.Start();
            await Apis.Discord.Discord.IndefinitelyWaitUntilClose();
            Console.ReadLine(); 
        }

        private static async Task InitAll()
        {
            Twitter.init();
            AbbyBooruChecker.Init();
            AbbybotTwitter.init();
            AbbySql.AbbysqlClient.connectionstring = Apis.Mysql.ApiKeys.MysqlApiKeys.Load(@"ApiKeys\Mysql.json").ToString();
}
    }
}
