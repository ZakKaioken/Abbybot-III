using System.Text;

namespace Abbybot_III.Core.AbbyBooru
{
    public class AbbybooruTagGenerator
    {
        public static string FCBuilder(string str) {
            return FCBuilder(new StringBuilder(str)).ToString();
        }
        public static StringBuilder FCBuilder(StringBuilder FavoriteCharacter)
		{
			FavoriteCharacter.Replace(" ", "_").Replace("abbybot", "abigail_williams").Replace("abby_kaioken", "abigail_williams");
			if (FavoriteCharacter[^1] != '~')
				FavoriteCharacter.Append("*");
			else
				FavoriteCharacter.Remove(FavoriteCharacter.Length - 1, 1);

			if (FavoriteCharacter.ToString().Contains("_~_") || FavoriteCharacter.ToString().Contains("_or_"))
			{
				FavoriteCharacter.Insert(0, "{").Append("}");
				FavoriteCharacter.Replace("~_or_", " ~ ").Replace("~_~_", " ~ ").Replace("_~_", "* ~ ").Replace("_or_", "* ~ ");
			}
			FavoriteCharacter.Replace("~_&&_", " ").Replace("~_and_", " ").Replace("_&&_", "* ").Replace("_and_", "* ");
			return FavoriteCharacter;
		}

		public static string InvertName(string sbsb)
		{
			var sb = new StringBuilder();
			var orchars = sbsb.Split(" ~ ");
			foreach (var orchar in orchars)
			{
				var andchars = orchar.Replace("{", "").Replace("}", "").Split(" ");
				foreach (var andchar in andchars)
				{
					var subnames = andchar.Replace("*", "").Split("_");
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