using AbbybotSleep;

using Abyplay;

using System;
using System.Diagnostics;
using System.Timers;

int milis = 1000 * 60 *30;
double twtqueueinitialstart = milis - ((DateTime.Now - DateTime.Today).TotalMilliseconds % (milis));
twtqueueinitialstart -= 60000;
var timestr = TimeStringGenerator.MilistoTimeString((decimal)twtqueueinitialstart);
Console.WriteLine($"Starting abbybot sleep process: {timestr}");
Timer timer = new Timer(twtqueueinitialstart);
timer.Elapsed += (a, b) =>
{
    timer.Interval = milis;
    Process.GetProcessesByName("Abbybot-III").Sleep();
    timestr = TimeStringGenerator.MilistoTimeString(milis);
    Console.WriteLine($"Dosing off... I'm back!! I'll probably dose off in {timestr}");
    Process.Start("Abbybot-III");
};
timer.Start();
Console.ReadLine();

