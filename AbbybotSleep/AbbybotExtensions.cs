using System.Diagnostics;

namespace AbbybotSleep
{
    public static class AbbybotExtensions
    {
        public static void Sleep(this Process[] processes)
        {
            foreach (var p in processes)
            {
                p.Kill();
            }
        }
    }
}