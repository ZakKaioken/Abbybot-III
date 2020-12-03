using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Abbybot_III.Apis.Twitter.Core
{
    class ImageDownloader
    {
        public static async Task<string> DownloadImage(string u)
        {
            WebClient client = new WebClient();

            string dir = $@"{Directory.GetCurrentDirectory()}\Temp\";


            if (Directory.Exists(dir))
                Directory.Delete(dir);            
            Directory.CreateDirectory(dir);
            

            string name = Path.GetFileName(u.ToString());
            string location = $"{dir}{name}";
            client.DownloadFileAsync(new Uri(u), location);

            while (client.IsBusy)
            {
                await Task.Delay(0);
            }
            client.Dispose();


            return location;

        }
    }
}
