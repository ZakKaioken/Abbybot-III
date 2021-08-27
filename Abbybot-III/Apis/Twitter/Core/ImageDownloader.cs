using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Abbybot_III.Apis.Twitter.Core
{
    class ImageDownloader
    {
        public static async Task<string> DownloadImage(string u)
        {
			string dir = $@"{Directory.GetCurrentDirectory()}\Temp\";

			if (Directory.Exists(dir))
                Directory.Delete(dir, true);
            Directory.CreateDirectory(dir);

            string name = Path.GetFileName(u.ToString());
            string location = $"{dir}{name}";

            int i = 0;
            HttpClient client = new();
            var response = await client.GetAsync(new Uri(u));
            do
            {
				using var fs = new FileStream(location, FileMode.Create);
				await response.Content.CopyToAsync(fs);
			} while (i++ < 3);
            return location;
        }
    }
}