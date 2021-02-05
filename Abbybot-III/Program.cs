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
            AbbySql.AbbysqlClient.connectionstring = Apis.Mysql.ApiKeys.MysqlApiKeys.Load(@"ApiKeys\Mysql.json").ToString();
            Abbybot.print("Nanobot III starting!");
            if (Process.GetProcessesByName("NanobotSleep").Length<1) 
                Process.Start("NanobotSleep.exe");
            await MysqlCore.CheckMysql(@"ApiKeys\mysqlbinpath.abbytxt");
            await InitAll();
            await Apis.Discord.Discord.DiscordMainAsync();
            AbbyHeart.Start();

            while(true)
            {
                await Task.Delay(1);
            }
 
        }

        private static async Task InitAll()
        {
            
            //await Twitter.init();
            AbbyBooruChecker.Init();
            //if (Twitter.tson)
            //AbbybotTwitter.init();
        }
    }
}
