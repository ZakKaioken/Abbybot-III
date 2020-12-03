using AbbybotSleep;

using System;
using System.Diagnostics;
using System.Timers;

Console.WriteLine("Starting abbybot sleep process");
Timer timer = new Timer(1000*((60 * 60) - 100));
timer.Elapsed += (a, b) =>
{
    Process.GetProcessesByName("Abbybot-III").Sleep();
    Process.Start("Abbybot-III");
};
timer.Start();
Console.ReadLine();

