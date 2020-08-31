﻿using Abbybot_III.Apis.Twitter;
using Abbybot_III.Core.AbbyBooru;
using Abbybot_III.Core.Heart;

using System;

namespace Abbybot_III
{
    class Program
    {
        static void Main()
        {
            Twitter.init();
            AbbyBooruChecker.Init();
            AbbySql.AbbysqlClient.connectionstring = Apis.Mysql.ApiKeys.MysqlApiKeys.Load(@"ApiKeys\Mysql.json").ToString();
            AbbyHeart.Start();
            Apis.Discord.Discord.DiscordMain();
            Console.ReadLine(); 
        }
    }
}
