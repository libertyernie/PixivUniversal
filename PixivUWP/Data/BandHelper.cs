using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Band;
using Windows.UI.Xaml.Media.Imaging;
using Microsoft.Band.Tiles;
using Windows.Storage;

namespace PixivUWP.Data
{
    public static class BandHelper
    {
        private static string _guid = "b84f5fb2-e643-4b58-bcc3-54ec5a26b850";

        public static string TileGUID
        {
            get => _guid;
        }

        public static async Task<bool> HasTile()
        {
            try
            {
                //连接手环
                IBandInfo[] bands = await BandClientManager.Instance.GetBandsAsync();
                using (var client = await BandClientManager.Instance.ConnectAsync(bands[0]))
                {
                    var tiles = await client.TileManager.GetTilesAsync();
                    foreach(var one in tiles)
                    {
                        if (one.TileId.Equals(new Guid(_guid)))
                            return true;
                    }
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        public static async Task<bool> CreateTileAsync()
        {
            try
            {
                //连接手环
                IBandInfo[] bands = await BandClientManager.Instance.GetBandsAsync();
                using (var client = await BandClientManager.Instance.ConnectAsync(bands[0]))
                {
                    //创建磁贴
                    WriteableBitmap smallIconBitmap = new WriteableBitmap(24, 24);
                    using (var stream = await (await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Assets/bandlogo24.png"))).OpenReadAsync())
                        await smallIconBitmap.SetSourceAsync(stream);
                    BandIcon smallIcon = smallIconBitmap.ToBandIcon();
                    WriteableBitmap tileIconBitmap = new WriteableBitmap(48, 48);
                    using (var stream = await (await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Assets/bandlogo48.png"))).OpenReadAsync())
                        await tileIconBitmap.SetSourceAsync(stream);
                    BandIcon tileIcon = tileIconBitmap.ToBandIcon();
                    BandTile tile = new BandTile(new Guid(_guid))
                    {
                        IsBadgingEnabled = true,
                        Name = "Pixiv",
                        SmallIcon = smallIcon,
                        TileIcon = tileIcon
                    };
                    return await client.TileManager.AddTileAsync(tile);
                }
            }
            catch
            {
                return false;
            }
        }

        public static async Task<bool> RemoveTileAsync()
        {
            try
            {
                //连接手环
                IBandInfo[] bands = await BandClientManager.Instance.GetBandsAsync();
                using (var client = await BandClientManager.Instance.ConnectAsync(bands[0]))
                {
                    return await client.TileManager.RemoveTileAsync(new Guid(_guid));
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
