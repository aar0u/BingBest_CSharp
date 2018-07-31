using System;
using System.IO;
using System.Net.Http;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;

namespace BingWallpaper
{
    internal class Program
    {
        private const string Domain = "https://www.bing.com";

        //8 is the max the api returns
        private const string RequestUrl = Domain + "/HPImageArchive.aspx?format=js&idx=0&n=8";
        private const string UserAgent = "Mozilla/5.0 (Windows NT 6.1; rv:28.0) Gecko/20100101 Firefox/28.0";
        private const string SavePath = "pic";
        private static readonly Random Rnd = new Random();
        private static readonly Logger Logger = new Logger("log.txt");

        private static void Main(string[] args)
        {
            var systemTray = new SystemTray("BingWallpaper");

            var index = Rnd.Next(0, 8);
            systemTray.ShowBalloon($"Set {index} of 8");
            var file = GetImage(index);

            Logger.Info($"Current: {Wallpaper.GetBackgroud()}");
            Wallpaper.SetBackgroud(file);

            if (systemTray.TrayIcon != null)
            {
                systemTray.TrayIcon.Visible = false;
                systemTray.TrayIcon.Dispose();
            }

            Application.Exit();
        }

        private static string GetImage(int index)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("User-Agent", UserAgent);

                var response = httpClient.GetStringAsync(RequestUrl).Result;
                var image = JObject.Parse(response)["images"][index];
                var imageUrl = Domain + image["url"].Value<string>();

                if (!Directory.Exists(SavePath))
                    Directory.CreateDirectory(SavePath);

                var fileName = Path.GetFileName(imageUrl);
                Logger.Info($"New: {fileName} | {image["copyright"].Value<string>()}");

                var file = Path.GetFullPath(Path.Combine(SavePath, fileName));
                if (File.Exists(file))
                    return file;
                using (var fileStream = File.Create(file))
                {
                    var imageResponse = httpClient.GetStreamAsync(imageUrl).Result;
                    imageResponse.CopyTo(fileStream);
                }

                return file;
            }
        }
    }
}