using BooruSharp.Booru;
using BooruSharp.Search.Post;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Abbybot_III.Apis.Booru
{
	class AbbyBooru
	{
		static List<ABooru> boorus = new List<ABooru>()
		{
			new Gelbooru(),
			new SankakuComplex(),
			new DanbooruDonmai(),
			new Safebooru()
		};

		public static string[] blacklist = { "bondage", "beastiality", "suicide", "injury", "furry", "guro", "sofra" };

		public static async Task<SearchResult> Execute(string[] tags)
		{
			var e = ExecuteAsync(tags).GetAwaiter();

			while (!e.IsCompleted)
				await Task.Delay(1);

			return e.GetResult();
		}

		public static async Task<int> GetPostCount(string[] tags)
		{
			int totalposts = 0;

			foreach (var booru in boorus)
			{
				try
				{
					totalposts += await booru.GetPostCountAsync(tags);
					break;
				}
				catch (Exception e)
				{
					Console.ForegroundColor = ConsoleColor.Red;
					Console.WriteLine(e.ToString());
					Console.ForegroundColor = ConsoleColor.White;
				}
			}

			return totalposts;
		}

		public static async Task<List<BooruSharp.Search.Tag.SearchResult>> GetTagData(string[] tags)
		{
			List<BooruSharp.Search.Tag.SearchResult> totalposts = new List<BooruSharp.Search.Tag.SearchResult>();

			foreach (var booru in boorus)
			{
				try
				{
					var w = string.Join(' ', tags);
					Abbybot.print(w);
					var e = await booru.GetTagsAsync(w);

					totalposts.AddRange(e);
					break;
				}
				catch (Exception e)
				{
					Console.ForegroundColor = ConsoleColor.Red;
					Console.WriteLine(e.ToString());
					Console.ForegroundColor = ConsoleColor.White;
				}
			}

			return totalposts;
		}

		static async Task<SearchResult> ExecuteAsync(string[] tags)
		{
			List<string> tagz = GetTags(tags);

			SearchResult searchResult = new SearchResult(new Uri("https://img2.gelbooru.com/samples/ee/e2/sample_eee286783bfa37e088d1ffbcf8f098ba.jpg"), new Uri("https://img2.gelbooru.com/samples/ee/e2/sample_eee286783bfa37e088d1ffbcf8f098ba.jpg"), new Uri("https://gelbooru.com/index.php?page=post&s=view&id=4325788&tags=crying+abigail_williams_%28fate%2Fgrand_order%29"), Rating.Safe, new string[] { "abigail_williams_(fate/grand_order)" }, 0, null, 1000, 1000, null, null, null, "noimagefound", 1000000, "47");
			foreach (var booru in boorus)
			{
				bool allowedtopost;
				try
				{
					do
					{
						searchResult = await booru.GetRandomPostAsync(tagz.ToArray());
						allowedtopost = true;
						if (searchResult.Source.Contains("sofra"))
						{ //please eventually add twitter user block list, so we can skip artists that hate us.
							allowedtopost = false;
						}
					} while (!allowedtopost);

					break;
				}
				catch { }
			}
			return searchResult;
		}

		public static async Task<SearchResult[]> GetLatest(string[] tags)
		{
			SearchResult[] e = null;

			try
			{
				e = await boorus[0].GetLastPostsAsync(tags);
			}
			catch { }
			return e;
		}

		static List<string> GetTags(string[] tags)
		{
			List<string> tagz = new List<string>();
			foreach (string item in tags)
			{
				tagz.Add(" " + item);
			}

			foreach (string item in blacklist)
			{
				tagz.Add($" -{item}");
			}
			return tagz;
		}
	}
}