using Abbybot_III.Apis.Mysql;
using Abbybot_III.Apis.Twitter;
using Abbybot_III.Clocks;
using Abbybot_III.Core.AbbyBooru;
using Abbybot_III.Core.CommandHandler;
using Abbybot_III.Core.Heart;
using Abbybot_III.Core.Twitter;

using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Abbybot_III
{
    class Program
    {
        
        static async Task Main()
        {
            Abbybot.print("Abbybot III starting!");
            if (Process.GetProcessesByName("AbbybotSleep").Length<1) 
                Process.Start("AbbybotSleep.exe");
            await MysqlCore.CheckMysql(@"ApiKeys\mysqlbinpath.abbytxt");
            AbbySql.AbbysqlClient.connectionstring = Apis.Mysql.ApiKeys.MysqlApiKeys.Load(@"ApiKeys\Mysql.json").ToString();
            await InitAll();
            await Apis.Discord.Discord.DiscordMainAsync();
            AbbyHeart.Start();

            while(true)
            {
                await Task.Delay(1);
            }
 
        }

        static async Task InitAll()
        {
            
            await Twitter.init();
            AbbyBooruChecker.Init();
            if (Twitter.tson)
            AbbybotTwitter.init();
        }
    }
}
