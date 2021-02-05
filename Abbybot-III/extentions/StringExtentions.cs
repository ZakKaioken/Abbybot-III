using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;

namespace Abbybot_III.extentions
{
    public static class StringExtentions
    {
        public static bool Contains (this string s, string[] ss)
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
    }
}
