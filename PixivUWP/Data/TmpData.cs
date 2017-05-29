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


        public static bool GetEnableAutoLoadWorkImg(Image obj)
        {
            return (bool)obj.GetValue(EnableAutoLoadWorkImgProperty);
        }

        public static void SetEnableAutoLoadWorkImg(Image obj, bool value)
        {
            obj.SetValue(EnableAutoLoadWorkImgProperty, value);
        }

        private static void OnEnableAutoLoadWorkImgChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is bool vl&&obj is Image img)
            {
                if(vl)
                    img.DataContextChanged += Img_DataContextChanged;
                else
                    img.DataContextChanged -= Img_DataContextChanged;
            }

        }

        static Queue<FrameworkElement> loadQueue = new Queue<FrameworkElement>();

        static bool isQueuedLoading = false;

        public static void ClearQueue()
            => loadQueue.Clear();

        private async static void QueuedLoad()
        {
            if (isQueuedLoading) return;
            isQueuedLoading = true;
            while(loadQueue.Count>0)
            {
                var tmpSender = loadQueue.Dequeue();
                await LoadPictureAsync(tmpSender);
            }
            isQueuedLoading = false;
        }

        private async static void Img_DataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
        {
            var img = sender as Image;
            img.Source = new Windows.UI.Xaml.Media.Imaging.BitmapImage(new Uri("ms-appx:///Assets/BlankHolder.png"));
            if (((int?)Data.AppDataHelper.GetValue("LoadPolicy")) == 1)
            {
                loadQueue.Enqueue(sender);
                QueuedLoad();
            }
            else
                await LoadPictureAsync(sender);
        }

        // Using a DependencyProperty as the backing store for EnableAutoLoadWorkImg.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EnableAutoLoadWorkImgProperty =
            DependencyProperty.RegisterAttached("EnableAutoLoadWorkImg", typeof(bool), typeof(Image), new PropertyMetadata(false, OnEnableAutoLoadWorkImgChanged));


        public static async Task LoadPictureAsync(FrameworkElement sender)
        {
            try
            {
                if (sender.Parent is Panel pl)
                {
                    if (pl.FindName("pro") is TextBlock ring)
                    {
                        ring.Visibility = Visibility.Visible;
                        try
                        {
                            var img = sender as Image;
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
                            ring.Visibility = Visibility.Collapsed;
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
