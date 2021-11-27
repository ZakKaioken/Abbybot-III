using BooruSharp.Booru;
using BooruSharp.Search.Post;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Abbybot_III.Apis
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

		public static string[] badtaglist = { "bondage", "beastiality", "suicide", "injury", "furry", "guro", "sofra", "asian", "photo_(medium)", "scat" };
		//I swear i'm not racist but this is an anime bot for anime girls
		

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

		public static async Task GetPictureById(int id, Action<SearchResult> onResult=null, Action<Exception> onFail=null) 
		{
			var booru = boorus[0];
			try {
				var sr = await booru.GetPostByIdAsync(id);
				onResult?.Invoke(sr);
			}catch (Exception e){
				onFail?.Invoke(e);
			}
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

	public static async Task<List<GelbooruResult>> ExecuteAsync(string[] tags, Action<Exception[]> onFailDeep = null)
	{
		List<string> tagz = GetTags(tags);
		List<GelbooruResult> results = new();
		List<Exception> deepFails = new();
		int index = 0;
		bool gotposts = false;
		while (!gotposts && index < boorus.Count)
		{
			try
			{
				var rs = await boorus[index].GetRandomPostsAsync(25, tagz.ToArray());
				foreach (var ts in rs.Where(x => x.FileUrl != null && !x.Source.Contains("sofra")))
					results.Add(new GelbooruResult() {tags = ts.Tags.ToArray(),
                        FileUrl = ts.FileUrl,
                        PreviewUrl = ts.PreviewUrl,
                        Source = ts.Source,
                        Nsfw= ts.Rating != Rating.Safe});
				if (results.Count > 0) { break; }
			} catch (Exception e) {
				deepFails.Add(e);
			}
		}
		if (results.Count == 0)
			return null;
		return results;
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

			foreach (string item in badtaglist)
			{
				tagz.Add($" -{item}");
			}
			return tagz;
		}
	}
}