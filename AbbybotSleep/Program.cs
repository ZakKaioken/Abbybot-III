using AbbybotSleep;

using Abyplay;

using System;
using System.Diagnostics;
using System.Timers;

int milis = 1000 * 60 *60* 3;
double twtqueueinitialstart = milis - ((DateTime.Now - DateTime.Today).TotalMilliseconds % (milis));
twtqueueinitialstart -= 60000;
var timestr = TimeStringGenerator.MilistoTimeString((decimal)twtqueueinitialstart);
Console.WriteLine($"Starting abbybot sleep process: {timestr}");
Timer timer = new Timer(twtqueueinitialstart);
timer.Elapsed += (a, b) =>
{
    timer.Interval = milis;
    Process.GetProcessesByName("Abbybot-III").Sleep();
    Console.WriteLine("Dosing off...");
    Process.Start("Abbybot-III");
};
timer.Start();
Console.ReadLine();

