﻿using Abbybot_III;
using Abbybot_III.Apis.Mysql;
using Abbybot_III.Apis.Twitter;
using Abbybot_III.Core.AbbyBooru;
using Abbybot_III.Core.Heart;
using Abbybot_III.Core.RequestSystem;
using Abbybot_III.Core.Twitter;

using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

Abbybot.print("Abbybot III starting!");
/*
if (File.Exists("AbbybotSleep.exe"))
	if (Process.GetProcessesByName("AbbybotSleep").Length < 1)
		Process.Start("AbbybotSleep.exe");
		*/
await MysqlCore.CheckMysql(@"ApiKeys\mysqlbinpath.abbytxt");
AbbySql.AbbysqlClient.connectionstring = Abbybot_III.Apis.Mysql.ApiKeys.MysqlApiKeys.Load(@"ApiKeys\Mysql.json").ToString();
await InitAll();
await Abbybot_III.Apis.Discord.DiscordMainAsync();
AbbyHeart.Start();

while (true)
{
	await Task.Delay(1);
}
static async Task InitAll()
{
	await Twitter.init();
	RequestCore.Init();
	AbbyBooruChecker.Init();
	if (Twitter.tson)
		AbbybotTwitter.init();
}