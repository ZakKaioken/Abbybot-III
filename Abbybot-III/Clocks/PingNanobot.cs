using Abbybot_III.Commands.Contains.Gelbooru.embed;
using Abbybot_III.Core.Data.User;
using Abbybot_III.Core.Users.sql;
using Abbybot_III.Sql.Abbybot.Abbybot;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Abbybot_III.Clocks
{
    class PingAbbybotClock : RepeatingClock
    {
        public override async Task OnInit(DateTime time)
        {
            name = "ping nanobot";
            delay = TimeSpan.FromMinutes(1);
            await base.OnInit(time);
        }

        public static int o = 0;
        static int o2 = 0;
        Random r = new Random();
        public override async Task OnWork(DateTime time)
        {
            var abbybotchannels = await AbbybotSql.GetAbbybotChannelIdAsync();
            var er = r.Next(0, abbybotchannels.Count);
            var ch = abbybotchannels[er];
            var c = Apis.Discord.Discord._client.GetGuild(ch.guildId).GetTextChannel(ch.channelId);
                
            if (o == 0)
                {
                    var abbybot = Process.GetProcessesByName("Nanobot");
                    if (abbybot.Length == 0)
                    {
                        ShellExecute(IntPtr.Zero, "open", @"C:\Users\AbbybotLaptop\Documents\AbbybotTemporary\AbbybotSource\Abbybot-III-Public\Abbybot-III\Abbybot\III\Nanobot.exe", @"", @"C:\Users\AbbybotLaptop\Documents\AbbybotTemporary\AbbybotSource\Abbybot-III-Public\Abbybot-III\Abbybot\III\", ShowCommands.SW_MINIMIZE);
                    }
                    o = 1;
                }
                else if (o == 1)
                {
                    await c.SendMessageAsync("Hey nano");
                    o = 2;
                }
                else if (o == 2)
                {
                    await c.SendMessageAsync("i'm gonna poke you with a stick silly nano!!");
                    WakeAbbybot();
                    await Task.Delay(5000);
                    await c.SendMessageAsync("good morning nano");

                    o = 1;
                } else if (o == 5)
                {
                    if (o2 > 4)
                    {
                        await c.SendMessageAsync("I failed at work nano...");
                        o = 1;
                    }
                o2++;
                }
        }
        

        static void WakeAbbybot()
        {
            var abbybotsleep = Process.GetProcessesByName("NanobotSleep");
            if (abbybotsleep.Length > 0)
            {
                abbybotsleep[0].Kill();
            }
            var abbybot = Process.GetProcessesByName("Nanobot");
            if (abbybot.Length > 0)
            {
                abbybot[0].Kill();
                ShellExecute(IntPtr.Zero, "open", @"C:\Users\AbbybotLaptop\Documents\AbbybotTemporary\AbbybotSource\Abbybot-III-Public\Abbybot-III\Abbybot\III\Nanobot.exe", @"", @"C:\Users\AbbybotLaptop\Documents\AbbybotTemporary\AbbybotSource\Abbybot-III-Public\Abbybot-III\Abbybot\III\", ShowCommands.SW_MINIMIZE);
            }
            else
            {
                ShellExecute(IntPtr.Zero, "open", @"C:\Users\AbbybotLaptop\Documents\AbbybotTemporary\AbbybotSource\Abbybot-III-Public\Abbybot-III\Abbybot\III\Nanobot.exe", @"", @"C:\Users\AbbybotLaptop\Documents\AbbybotTemporary\AbbybotSource\Abbybot-III-Public\Abbybot-III\Abbybot\III\", ShowCommands.SW_MINIMIZE);
            }
        }

        public enum ShowCommands : int
        {
            SW_HIDE = 0,
            SW_SHOWNORMAL = 1,
            SW_NORMAL = 1,
            SW_SHOWMINIMIZED = 2,
            SW_SHOWMAXIMIZED = 3,
            SW_MAXIMIZE = 3,
            SW_SHOWNOACTIVATE = 4,
            SW_SHOW = 5,
            SW_MINIMIZE = 6,
            SW_SHOWMINNOACTIVE = 7,
            SW_SHOWNA = 8,
            SW_RESTORE = 9,
            SW_SHOWDEFAULT = 10,
            SW_FORCEMINIMIZE = 11,
            SW_MAX = 11
        }

        [DllImport("shell32.dll")]
        static extern IntPtr ShellExecute(
            IntPtr hwnd,
            string lpOperation,
            string lpFile,
            string lpParameters,
            string lpDirectory,
            ShowCommands nShowCmd);
    }
}
