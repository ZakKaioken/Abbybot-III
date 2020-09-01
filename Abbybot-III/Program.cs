using Abbybot_III.Apis.Twitter;
using Abbybot_III.Core.AbbyBooru;
using Abbybot_III.Core.Heart;
using Abbybot_III.Core.Twitter;

using System;

namespace Abbybot_III
{
    class Program
    {
        static void Main()
        {
            Twitter.init();
            AbbyBooruChecker.Init();
            AbbybotTwitter.init();
            AbbySql.AbbysqlClient.connectionstring = Apis.Mysql.ApiKeys.MysqlApiKeys.Load(@"ApiKeys\Mysql.json").ToString();
            AbbyHeart.Start();
            Apis.Discord.Discord.DiscordMain();
            Console.ReadLine(); 
        }
    }
}
