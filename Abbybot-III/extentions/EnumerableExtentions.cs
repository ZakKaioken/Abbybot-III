using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Abbybot_III.extentions
{
    public static class EnumerableExtentions
    {
        static Random r = new Random();

        public static T random<T>(this List<T> ts)
        {
            return ts[r.Next(0, ts.Count())];
        }

        public static T random<T>(this IList<T> ts)
        {
            return ts[r.Next(0, ts.Count())];
        }

    }
}
