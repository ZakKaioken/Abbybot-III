using Abbybot_III.Core.LevelingManager.Types;

using System;
using System.Collections.Generic;
using System.Text;

namespace Abbybot_III.Core.LevelingManager
{
    class LevelCalculator
    {
        public static Stat CalculateStatLevel(ulong i, string statname)
        {
            float f = i;
            ulong level = 1;
            float lastlr;
            while (true)
            {

                lastlr = level * ((level + 1) / 1.5f) * 4;
                if (f > lastlr)
                {
                    level++;
                    f -= lastlr;
                }
                else
                    break;
            }
            return new Stat()
            {
                name = statname,
                level = level,
                exp = (int)Math.Round(f),
                expleft = (int)Math.Round(lastlr),
                totalstat = i
            };
        }

    }
}
