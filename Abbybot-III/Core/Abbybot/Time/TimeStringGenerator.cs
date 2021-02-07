using System;
using System.Text;

namespace Abyplay
{
    class TimeStringGenerator
    {
        public static string MilistoTimeString(decimal milis)
        {
            StringBuilder sb = new StringBuilder();
            var (milistr, secs) = upgrade(milis, 1000);

            var (secstr, mins) = upgrade(secs, 60);

            var (minstr, hours) = upgrade(mins, 60);

            var (hrstr, days) = upgrade(hours, 24);

            var (daystr, weeks) = upgrade(days, 7);

            var (weekstr, months) = upgrade(weeks, 4);

            var (monstr, years) = upgrade(months, 12);

            var (yearstr, decades) = upgrade(years, 10);

            var (decastr, centuries) = upgrade(decades, 10);

            var (centstr, milenia) = upgrade(centuries, 10);

            var (milstr, magic) = upgrade(milenia, 10);

            if (milstr > 0)
            {
                sb.Append($"{milstr} milenia");
                if (milstr > 1) sb.Append("s");
                if (centstr > 0) sb.Append(", ");
            }

            if (centstr > 0)
            {
                sb.Append($"{centstr} centurie");
                if (centstr > 1) sb.Append("s");
                if (decastr > 0) sb.Append(", ");
            }

            if (decastr > 0)
            {
                sb.Append($"{decastr} decade");
                if (decastr > 1) sb.Append("s");
                if (yearstr > 0) sb.Append(", ");
            }

            if (yearstr > 0)
            {
                sb.Append($"{yearstr} year");
                if (yearstr > 1) sb.Append("s");
                if (monstr > 0) sb.Append(", ");
            }

            if (monstr > 0)
            {
                sb.Append($"{monstr} month");
                if (monstr > 1) sb.Append("s");
                if (weekstr > 0) sb.Append(", ");
            }

            if (weekstr > 0)
            {
                sb.Append($"{weekstr} week");
                if (weekstr > 1) sb.Append("s");
                if (daystr > 0) sb.Append(", ");
            }

            if (daystr > 0)
            {
                sb.Append($"{daystr} day");
                if (daystr > 1) sb.Append("s");
                if (hrstr > 0) sb.Append(", ");
            }

            if (hrstr > 0)
            {
                sb.Append($"{hrstr} hour");
                if (hrstr > 1) sb.Append("s");
                if (minstr > 0) sb.Append(", ");
            }

            if (minstr > 0)
            {
                sb.Append($"{minstr} min");
                if (minstr > 1) sb.Append("s");
                if (secstr > 0) sb.Append(", ");
            }
            if (secstr > 0)
            {
                sb.Append($"{secstr} sec");
                if (secstr > 1) sb.Append("s");
                if (milistr > 0) sb.Append(", ");
            }
            if (milistr > 0)
            {
                sb.Append($"{milistr} mili");
                if (milistr > 1) sb.Append("s");
            }
            return sb.ToString();
        }

        static (decimal startmod, decimal enddiv) upgrade(decimal start, int count)
        {
            return (Math.Floor(start % count), start / count);
        }
    }
}