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
        public static bool Contains(this StringBuilder s, string[] ss)
        {
            bool f = false;
            foreach (var x in ss)
            {
                if (s.Contains(x))
                    f = true;
            }
            return f;
        }

        public static StringBuilder RemoveEnd(this StringBuilder s, int length) {
            s.Reverse().Remove(0, length).Reverse();
            return s;
        }
        public static StringBuilder AppendStartEnd(this StringBuilder s, string beginning, string ending) {
            s.Insert(0, beginning).Append(ending);
            return s;
        }
        public static StringBuilder Remove(this StringBuilder s, string[] ss) {
            foreach(var so in ss)
                s.Replace(so, "");
            return s;
        }
        public static string Remove(this string s, string[] ss) {
            foreach(var so in ss)
                s.Replace(so, "");
            return s;
        }
        public static StringBuilder Remove(this StringBuilder s, string ss) {
                
            return s.Replace(ss, "");
        }
        public static string Remove(this string s, string ss) {
                
            return s.Replace(ss, "");
        }
        public static bool EndsWith(this StringBuilder s, char c) {
            return (s[s.Length-1]==c);
        }
        public static StringBuilder Reverse(this StringBuilder sb)
{
    char t;
    int end = sb.Length - 1;
    int start = 0;
    
    while (end - start > 0)
    {
        t = sb[end];
        sb[end] = sb[start];
        sb[start] = t;
        start++;
        end--;
    }
    return sb;
}
        public static StringBuilder Replace(this StringBuilder s, string[] ss, string replacement)
        {
            foreach (var x in ss)
            {
                s.Replace(x, replacement);
            }
            return s;
        }
public static bool Contains(this StringBuilder s, string ss)
        {
            
            return s.ToString().Contains(ss);
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