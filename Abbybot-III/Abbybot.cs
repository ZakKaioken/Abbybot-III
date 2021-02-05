using System;
using System.Collections.Generic;
using System.Text;

namespace Abbybot_III
{
    class Abbybot
    {
        public static void print(object o)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine(o.ToString());
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}
