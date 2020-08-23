using Abbybot_III.Core.Heart;

using System;

namespace Abbybot_III
{
    class Program
    {
        static void Main()
        {
            AbbyHeart.Start();
            Apis.Discord.Discord.DiscordMain();
            Console.ReadLine(); 
        }
    }
}
