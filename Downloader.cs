using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Animation;

namespace ModificaWPF
{
    internal static class Downloader
    {
        public static async Task<Version> GetLatestVersion()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    return Version.Parse(await client.GetStringAsync("https://raw.githubusercontent.com/Sacracia/ModificaWPF/master/version"));
                }
            }
            catch
            {
                return null;
            }
        }

        public static async Task<byte[]> DownloadFile(string url)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    return await client.GetByteArrayAsync(url);
                }
            }
            catch
            {
                return null;
            }
        }
    }
}
