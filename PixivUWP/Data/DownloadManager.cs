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
        public static StorageFolder pictureFolder;
        public static void AddTask(string url, string filename)
        {
            var task = AddTaskAsync(url, filename);
        }
        public static async Task AddTaskAsync(string url,string filename)
        {
            await getpicfolder();
            string ex;
            try
            {
                ex = Path.GetExtension(url);
            }
            catch
            {
                ex = ".jpg";
            }
            if (Uri.TryCreate(url, UriKind.Absolute, out Uri result))
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
                    Windows.Networking.BackgroundTransfer.BackgroundTransferCostPolicy costpolicy;
                    switch (policy)
                    {
                        default:
                        case 0:
                            costpolicy = Windows.Networking.BackgroundTransfer.BackgroundTransferCostPolicy.UnrestrictedOnly;
                            break;
                        case 1:
                            costpolicy = Windows.Networking.BackgroundTransfer.BackgroundTransferCostPolicy.Default;
                            break;
                        case 2:
                            costpolicy = Windows.Networking.BackgroundTransfer.BackgroundTransferCostPolicy.Always;
                            break;
                        case 3:
                            using (var res = await Data.TmpData.CurrentAuth.Tokens.SendRequestToGetImageAsync(Pixeez.MethodType.GET, url))
                            {
                                using (var stream = await res.GetResponseStreamAsync())
                                {
                                    using (var filestream = await file.OpenStreamForWriteAsync())
                                    {
                                        await stream.CopyToAsync(filestream);
                                    }
                                }
                            }
                            return;
                    }
                    var downloader = new Windows.Networking.BackgroundTransfer.BackgroundDownloader() { CostPolicy = costpolicy };
                    downloader.SetRequestHeader("Referer", "https://app-api.pixiv.net/");
                    var op = downloader.CreateDownload(result, file);
                    var a = op.StartAsync();
                }
                catch { }
            }

        }

        public static async Task getpicfolder()
        {
            if (pictureFolder == null)
                pictureFolder = await Windows.Storage.KnownFolders.PicturesLibrary.CreateFolderAsync("Pixiv", CreationCollisionOption.OpenIfExists);
        }
    }
}
