using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace PixivUWP.Data
{
    static class DownloadManager
    {
        static StorageFolder pictureFolder;
        public static void AddTask(string url, string filename)
        {
            var task = AddTaskAsync(url, filename);
        }
        public static async Task AddTaskAsync(string url,string filename)
        {
            if (pictureFolder == null)
                pictureFolder = await Windows.Storage.KnownFolders.PicturesLibrary.CreateFolderAsync("Pixiv", CreationCollisionOption.OpenIfExists);
            string ex;
            try
            {
                ex=Path.GetExtension(url);
            }
            catch
            {
                ex = ".jpg";
            }
            Uri result;
            if(Uri.TryCreate(url,UriKind.Absolute,out result))
            {
                int policy;
                try
                {
                    policy = (int)AppDataHelper.GetValue("BackgroundTransferCostPolicy");
                }
                catch
                {
                    policy = 0;
                }
                try
                {
                    var file = await pictureFolder.CreateFileAsync(filename + ex, CreationCollisionOption.FailIfExists);
                    var downloader = new Windows.Networking.BackgroundTransfer.BackgroundDownloader();
                    downloader.SetRequestHeader("Referer", "https://app-api.pixiv.net/");
                    switch(policy)
                    {
                        default:
                        case 0:
                            downloader.CostPolicy = Windows.Networking.BackgroundTransfer.BackgroundTransferCostPolicy.UnrestrictedOnly;
                            break;
                        case 1:
                            downloader.CostPolicy = Windows.Networking.BackgroundTransfer.BackgroundTransferCostPolicy.Default;
                            break;
                        case 2:
                            downloader.CostPolicy = Windows.Networking.BackgroundTransfer.BackgroundTransferCostPolicy.Always;
                            break;
                    }
                    var op=downloader.CreateDownload(result, file);
                    await op.StartAsync();
                }
                catch { }
            }

        }
    }
}
