using System;

namespace Abbybot_III.extentions
{
    public class Chance
    {
        static Random r = new Random();

        public static bool Roll(float min, float max)
        {
            double nextdouble = r.NextDouble();
            float c = min / max;
			return nextdouble < c;
		}
	}
}