using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;

namespace BingWallpaper
{
    internal class Program
    {
        private const string Domain = "https://www.bing.com";
        private const string RequestUri = Domain + "/HPImageArchive.aspx?format=js&idx=0&n=1";

        private const string UserAgent =
            "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36";

        private const string SavePath = @".\pic";
        private static readonly StringBuilder StringBuilder = new StringBuilder();

        private static void Main(string[] args)
        {
            //Application.Run(new Form1());

            var systemTray = new SystemTray("BingWallpaper");
            systemTray.ShowBalloon("Working on it");

            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("User-Agent", UserAgent);

                var response = httpClient.GetStringAsync(RequestUri).Result;
                var imageUrl = Domain + JObject.Parse(response)["images"][0]["url"].Value<string>();
                var imageResponse = httpClient.GetAsync(imageUrl).Result;

                var fn = Path.GetFileName(imageUrl);
                if (!Directory.Exists(SavePath))
                    Directory.CreateDirectory(SavePath);
                var responseStream = imageResponse.Content.ReadAsStreamAsync().Result;
                var file = Path.GetFullPath(Path.Combine(SavePath, fn));
                if (!File.Exists(file))
                {
                    using (var fileStream = File.Create(file))
                    {
                        responseStream.CopyTo(fileStream);
                    }
                }

                StringBuilder.Append($"{DateTime.Now} Current: {Wallpaper.GetBackgroud()}\r\n");
                Wallpaper.SetBackgroud(file);
                StringBuilder.Append($"{DateTime.Now} New: {imageUrl}\r\n");

                File.AppendAllText("log.txt", StringBuilder.ToString());
                StringBuilder.Clear();

                if (systemTray.TrayIcon != null)
                {
                    systemTray.TrayIcon.Visible = false;
                    systemTray.TrayIcon.Dispose();
                }

                Application.Exit();
            }
        }
    }
}