using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Abbybot_III.extentions
{
    public static class StringExtentions
    {
        public static bool Contains(this string s, string[] ss)
        {
            bool f = false;
            foreach (var x in ss)
            {
                if (s.Contains(x))
                    f = true;
            }
            return f;
        }

        public static string ReplaceA(this string s, string word, string replacement)
        {
            return Regex.Replace(s, word, replacement, RegexOptions.IgnoreCase);
        }

        public static string[] Split(this StringBuilder stringBuilder, string split) {
            return stringBuilder.ToString().Split(split);
        }

    }
}