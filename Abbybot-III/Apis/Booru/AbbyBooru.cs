using Nano.XML;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Abbybot_III.Apis
{
	class AbbyBooru
	{
		public static string[] badtaglist = { "bondage", "beastiality", "suicide", "injury", "furry", "guro", "sofra", "asian", "photo_(medium)", "scat" };
		//I swear i'm not racist but this is an anime bot for anime girls
		

		public static async Task<int> GetPostCount(string[] tags)
		{		
		try
				{
				var c = await Nano.Booru.GetPictureAsync( tags.ToList(), 0 );
				return c.count;
				}
				catch (Exception e)
				{
					Console.WriteLine(e.ToString());
				}
			
			return 0;
		}

		public static async Task GetPictureById(ulong id, Action<Post> onResult=null, Action<Exception> onFail=null) 
		{
			try {
				var sr = await Nano.Booru.GetPictureWithId(id);
				if (sr.count>0) 
				onResult?.Invoke(sr.posts[0]);
			}catch (Exception e){
				Console.WriteLine( e );
				onFail?.Invoke(e);
			}
		}

		

	public static async Task<List<Post>> GetRandomPost(string[] tags, Action<Exception[]> onFailDeep = null)
	{
			List<string> tagz = GetTags(tags);

			var rs = await Nano.Booru.GetPictureAsync(tagz, 5 , true);
			return rs.posts.Where( p => !p.source.Contains( "sofra" ) ).ToList();
	}

		public static async Task<List<Post>> GetLatest(string[] tags)
		{
			List<string> tagz = GetTags( tags );
			var rs = await Nano.Booru.GetPictureAsync( tagz , 100, false );
			return rs.posts.Where( p => !p.source.Contains( "sofra" ) ).ToList( );
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