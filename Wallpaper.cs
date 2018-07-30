using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace BingWallpaper
{
    internal class Wallpaper
    {
        public enum UAction
        {
            /// <summary>
            ///     set the desktop background image
            /// </summary>
            SpiSetdeskwallpaper = 0x0014,

            /// <summary>
            ///     get the desktop background image
            /// </summary>
            SpiGetdeskwallpaper = 0x0073
        }

        [DllImport("user32.dll", EntryPoint = "SystemParametersInfo")]
        public static extern int
            SystemParametersInfo(UAction uAction, int uParam, StringBuilder lpvParam, int fuWinIni);

        public static string GetBackgroud()
        {
            var s = new StringBuilder(300);
            SystemParametersInfo(UAction.SpiGetdeskwallpaper, 300, s, 0);
            return s.ToString();
        }

        /// <summary>
        ///     set the desktop background image
        /// </summary>
        /// <param name="fileName">the path of image</param>
        /// <returns></returns>
        public static int SetBackgroud(string fileName)
        {
            var result = 0;
            if (File.Exists(fileName))
            {
                var s = new StringBuilder(fileName);
                result = SystemParametersInfo(UAction.SpiSetdeskwallpaper, 0, s, 0x2);
            }

            return result;
        }
    }
}