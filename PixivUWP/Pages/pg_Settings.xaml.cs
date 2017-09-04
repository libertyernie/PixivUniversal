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
using PixivUWP.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace PixivUWP.Pages
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class pg_Settings : Page,IBackHandlable,IBackable
    {
        string str_con= "";
        public pg_Settings()
        {
            this.InitializeComponent();
            Yinyue200.OperationDeferral.OperationDeferral od = new Yinyue200.OperationDeferral.OperationDeferral();
            od.Start();
            Task.Run(() =>
            {
                try
                {
                    str_con = FileIO.ReadTextAsync(Windows.Storage.StorageFile.GetFileFromApplicationUriAsync(new Uri(@"ms-appx:///Contributors.txt")).AsTask().Result).AsTask().Result;
                }
                catch { }
                od.Complete();
            });
            od.WaitOne();
            contributors.Text = str_con;
            //下面这部分代码写得真暴力
            //设置以后假如多了这里迟早成瓶颈
            try
            {
                backpolicy.SelectedIndex = (int)Data.AppDataHelper.GetValue("BackgroundTransferCostPolicy");
            }
            catch
            {
                backpolicy.SelectedIndex = 0;
            }
            try
            {
                loadpolicy.SelectedIndex = (int)Data.AppDataHelper.GetValue("LoadPolicy");
            }
            catch
            {
                loadpolicy.SelectedIndex = 1;
            }
            try
            {
                imagepreviewsizepolicy.SelectedIndex = (int)Data.AppDataHelper.GetValue("PreviewImageSize");
            }
            catch
            {
                imagepreviewsizepolicy.SelectedIndex = 0;
            }
            try
            {
                软件主题.SelectedIndex = (int)Data.AppDataHelper.GetValue(nameof(软件主题));
            }
            catch
            {
                软件主题.SelectedIndex = 0;
            }
            leftwidth.Text = Data.TmpData.waterflowwidth.ToString();
            leftcolum.Text = Data.TmpData.waterflowcolumnum.ToString();
        }

        public BackInfo GenerateBackInfo() => null;

        private async void Hyperlink_Click(Windows.UI.Xaml.Documents.Hyperlink sender, Windows.UI.Xaml.Documents.HyperlinkClickEventArgs args)
        {
            await Windows.System.Launcher.LaunchUriAsync(new Uri("ms-windows-store://pdp/?productid=9nblggh1xvd2", UriKind.Absolute));
        }

        private async void Hyperlink_Click2(Windows.UI.Xaml.Documents.Hyperlink sender, Windows.UI.Xaml.Documents.HyperlinkClickEventArgs args)
        {
            await Windows.System.Launcher.LaunchUriAsync(new Uri("https://github.com/moespirit", UriKind.Absolute));
        }

        private void backpolicy_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Data.AppDataHelper.SetValue("BackgroundTransferCostPolicy", backpolicy.SelectedIndex);
        }

        private void loadpolicy_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Data.AppDataHelper.SetValue("LoadPolicy", loadpolicy.SelectedIndex);
        }

        private void imagepreviewsizepolicy_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Data.AppDataHelper.SetValue("PreviewImageSize", imagepreviewsizepolicy.SelectedIndex);
        }


        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            try
            {
                if ((bool)((object[])e.Parameter)[0])
                {
                    Data.TmpData.isBackTrigger = true;
                    Data.TmpData.menuItem.SelectedIndex = -1;
                    Data.TmpData.menuBottomItem.SelectedIndex = 0;
                }
            }
            catch { }
            var res = BandHelper.HasTile();
            var awaiter = res.GetAwaiter();
            awaiter.OnCompleted(() =>
            {
                band_Connect.Content = awaiter.GetResult() ? "- Pixiv Tile" : "+ Pixiv Tile";
                band_Connect.IsEnabled = true;
            });
        }

        public bool GoBack() => false;

        private void band_Connect_Click(object sender, RoutedEventArgs e)
        {
            if (band_Connect.Content.ToString() == "+ Pixiv Tile")
            {
                band_Connect.IsEnabled = false;
                var res = BandHelper.CreateTileAsync();
                var awaiter = res.GetAwaiter();
                awaiter.OnCompleted(() =>
                {
                    if (awaiter.GetResult())
                    {
                        band_Connect.Content = "- Pixiv Tile";
                        band_Connect.IsEnabled = true;
                    }
                    else
                    {
                        new Controls.MyToast("Error").Show();
                        band_Connect.IsEnabled = true;
                    }
                });
            }
            else
            {
                band_Connect.IsEnabled = false;
                var res = BandHelper.RemoveTileAsync();
                var awaiter = res.GetAwaiter();
                awaiter.OnCompleted(() =>
                {
                    if (awaiter.GetResult())
                    {
                        band_Connect.Content = "+ Pixiv Tile";
                        band_Connect.IsEnabled = true;
                    }
                    else
                    {
                        new Controls.MyToast("Error").Show();
                        band_Connect.IsEnabled = true;
                    }
                });
            }
        }

        private void 软件主题_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Data.AppDataHelper.SetValue(nameof(软件主题), 软件主题.SelectedIndex);
        }

        private void logout_Click(object sender, RoutedEventArgs e)
        {
            Data.AppDataHelper.SetValue("isrem", false);
            Data.AppDataHelper.SetValue("isauto", false);
            Data.AppDataHelper.SetValue("upasswd", null);
            (Window.Current.Content as Frame).Navigate(typeof(LoginPage));
        }

        private void updateleft_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Convert.ToInt32(leftwidth.Text) <= 100 || Convert.ToInt32(leftcolum.Text) <= 0)
                {
                    ((Button)sender).Content = "值不合法";
                    return;
                }
                Data.TmpData.waterflowwidth = Convert.ToInt32(leftwidth.Text);
                Data.TmpData.waterflowcolumnum = Convert.ToInt32(leftcolum.Text);
                Data.AppDataHelper.SetValue("leftwidth", Data.TmpData.waterflowwidth);
                Data.AppDataHelper.SetValue("leftcolum", Data.TmpData.waterflowcolumnum);
                ((Button)sender).Content = "更新成功";
            }
            catch
            {
                ((Button)sender).Content = "值不合法";
            }
        }

        private void resetleft_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Data.TmpData.waterflowwidth = 380;
                Data.TmpData.waterflowcolumnum = 2;
                Data.AppDataHelper.SetValue("leftwidth", Data.TmpData.waterflowwidth);
                Data.AppDataHelper.SetValue("leftcolum", Data.TmpData.waterflowcolumnum);
                leftwidth.Text = Data.TmpData.waterflowwidth.ToString();
                leftcolum.Text = Data.TmpData.waterflowcolumnum.ToString();
                ((Button)sender).Content = "重置成功";
            }
            catch
            {
                ((Button)sender).Content = "重置失败";
            }
        }
    }
}
