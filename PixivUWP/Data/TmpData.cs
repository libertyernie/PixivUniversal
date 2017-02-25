//PixivUniversal
//Copyright(C) 2017 Pixeez Plus Project

//This program is free software; you can redistribute it and/or
//modify it under the terms of the GNU General Public License
//as published by the Free Software Foundation; version 2
//of the License.

//This program is distributed in the hope that it will be useful,
//but WITHOUT ANY WARRANTY; without even the implied warranty of
//MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the
//GNU General Public License for more details.

//You should have received a copy of the GNU General Public License
//along with this program; if not, write to the Free Software
//Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.
using Pixeez.Objects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace PixivUWP.Data
{
    //偷懒用的方法。。
    public static class TmpData
    {
        public static string Username;
        public static string Password;
        public static Pixeez.AuthResult CurrentAuth;
        public static Dictionary<object, int> OpenedWindows = new Dictionary<object, int>();
        public static async Task LoadPictureAsync(FrameworkElement sender)
        {
            try
            {
                if (sender.Parent is Panel pl)
                {
                    if (pl.FindName("pro") is ProgressRing ring)
                    {
                        ProgressBarVisualHelper.SetYFHelperVisibility(ring, true);
                        try
                        {
                            var img = sender as Image;
                            img.Source = null;
                            if (img.DataContext != null)
                            {
                                var work = (img.DataContext as Work);
                                using (var stream = await Data.TmpData.CurrentAuth.Tokens.SendRequestToGetImageAsync(Pixeez.MethodType.GET, work.ImageUrls.Medium))
                                {
                                    var bitmap = new Windows.UI.Xaml.Media.Imaging.BitmapImage();
                                    await bitmap.SetSourceAsync((await stream.GetResponseStreamAsync()).AsRandomAccessStream());
                                    img.Source = bitmap;
                                }
                            }
                        }
                        finally
                        {
                            ProgressBarVisualHelper.SetYFHelperVisibility(ring, false);
                        }
                    }
                }
            }
            catch
            {

            }
        }
    }
}
