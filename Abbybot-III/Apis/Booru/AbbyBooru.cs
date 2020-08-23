using BooruSharp.Search.Post;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Abbybot_III.Apis.Booru
{
    class AbbyBooru
    {
        static BooruSharp.Booru.Gelbooru gel = new BooruSharp.Booru.Gelbooru();
        static BooruSharp.Booru.SankakuComplex sanku = new BooruSharp.Booru.SankakuComplex();
        static BooruSharp.Booru.Safebooru safe = new BooruSharp.Booru.Safebooru();

        public static string[] blacklist = { "beastiality", "furry", "vore", "blood", "sofra" };
        public static async Task<SearchResult> Execute(string[] tags)
        {
            List<string> tagz = GetTags(tags);
            SearchResult searchResult;
            try
            {
                searchResult = await gel.GetRandomPostAsync(tagz.ToArray());
            }
            catch
            {
                try
                {
                    searchResult = await sanku.GetRandomPostAsync(tagz.ToArray());
                }
                catch
                {
                    try
                    {

                        searchResult = await safe.GetRandomPostAsync(tagz.ToArray());
                    }
                    catch
                    {
                        throw new Exception("no picture found");

                        searchResult = new SearchResult(new Uri("https://img2.gelbooru.com/samples/ee/e2/sample_eee286783bfa37e088d1ffbcf8f098ba.jpg"), new Uri("https://img2.gelbooru.com/samples/ee/e2/sample_eee286783bfa37e088d1ffbcf8f098ba.jpg"), new Uri("https://gelbooru.com/index.php?page=post&s=view&id=4325788&tags=crying+abigail_williams_%28fate%2Fgrand_order%29"), Rating.Safe, new string[] { "abigail_williams_(fate/grand_order)" }, 0, null, 1000, 1000, null, null, null, "", 1000000, "47");
                    }
                }
            }
            return searchResult;
        }

        private static List<string> GetTags(string[] tags)
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
