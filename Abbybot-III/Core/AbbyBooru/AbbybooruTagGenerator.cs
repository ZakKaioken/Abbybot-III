using System;
using System.Text;
using Abbybot_III.extentions;

namespace Abbybot_III.Core.AbbyBooru
{
    public class AbbybooruTagGenerator
    {
        public static string FCBuilder(string str) {
            return FCBuilder(new StringBuilder(str)).ToString();
        }
        public static StringBuilder FCBuilder(StringBuilder FCString, StringBuilder funbuilder = null)
		{
			if (FCString == null) return null;
			
			var noWildOrs = new string[] {"~_or_","~_~_", "~,_"};
			var wildOrs = new string[] {",_", "_~_", "_or_"};
			var noWildAnds = new string[] {"~_&&_","~_and_"};
			var wildAnds = new string[] {"_&&_", "_and_"};
			var abbyAlts = new string[] {"abbybot","abby_kaioken"};
			FCString.Replace(" ", "_").Replace(abbyAlts, "abigail_williams");
			if (FCString.Length > 0) {
			if (!FCString.EndsWith('~'))
				FCString.Append("*");
			else
				FCString.RemoveEnd(1);
			}
			if (FCString.Contains(wildOrs))
			{
				FCString.AppendStartEnd("{", "}");
				FCString.Replace(noWildOrs, " ~ ").Replace(wildOrs, "* ~ ");
			}
			FCString.Replace(noWildAnds, " ").Replace(wildAnds, "* ");
			while (FCString.Contains("**")) FCString.Replace("**", "*");
			return FCString;
		}

		public static string InvertName(string sbsb)
		{
			var sb = new StringBuilder();
			var orchars = sbsb.Split(" ~ ");
			foreach (var orchar in orchars)
			{
				var andchars = orchar.Remove(new string[] {"{","}"}).Split(" ");
				foreach (var andchar in andchars)
				{
					var subnames = andchar.Remove("*").Split("_");
					if (subnames.Length == 1)
					{
						sb.Append(subnames[0]);
					}
					if (subnames.Length == 2)
					{
						sb.Append($"{subnames[^1]}_{subnames[0]}*");
					}
					else if (subnames.Length == 3)
					{
						sb.Append($"{subnames[^1]}_{subnames[1]}_{subnames[0]}*");
					}
					if (andchars.Length > 1)
						sb.Append(" ");
				}
				if (orchars.Length > 1)
				{
					sb.Append(" ~ ");
				}
			}
			if (orchars.Length > 1)
			{
				sb.Insert(0, "{");
				sb.Append("}");
			}
			var s = sb.ToString();
			return s;
		}
    }
}