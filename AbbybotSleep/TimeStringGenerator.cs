using System;
using System.Text;

namespace Abyplay
{
    class TimeStringGenerator
    {
        public static string MilistoTimeString(decimal milis)
        {
            StringBuilder sb = new();
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
                sb.Append($"{milstr} milenias");
                if (centstr > 0) sb.Append(", ");
            }

            if (centstr > 0)
            {
                sb.Append($"{centstr} centuries");
                if (decastr > 0) sb.Append(", ");
            }

            if (decastr > 0)
            {
                sb.Append($"{decastr} decades");
                if (yearstr > 0) sb.Append(", ");
            }

            if (yearstr > 0)
            {
                sb.Append($"{yearstr} years");
                if (monstr > 0) sb.Append(", ");
            }

            if (monstr > 0)
            {
                sb.Append($"{monstr} months");
                if (weekstr > 0) sb.Append(", ");
            }

            if (weekstr > 0)
            {
                sb.Append($"{weekstr} weeks");
                if (daystr > 0) sb.Append(", ");
            }

            if (daystr > 0)
            {
                sb.Append($"{daystr} days");
                if (hrstr > 0) sb.Append(", ");
            }

            if (hrstr > 0)
            {
                sb.Append($"{hrstr} hours");
                if (minstr > 0) sb.Append(", ");
            }

            if (minstr > 0)
            {
                sb.Append($"{minstr} mins");
                if (secstr > 0) sb.Append(", ");
            }

            if (secstr > 0)
            {
                sb.Append($"{secstr} secs");
                if (milistr > 0) sb.Append(", ");
            }
            if (milistr > 0)
            {
                sb.Append($"{milistr} milis");
            }
            sb.Append('!');
            return sb.ToString();
        }

        static (decimal startmod, decimal enddiv) upgrade(decimal start, int count)
        {
            return (Math.Floor(start % count), start / count);
        }
    }
}