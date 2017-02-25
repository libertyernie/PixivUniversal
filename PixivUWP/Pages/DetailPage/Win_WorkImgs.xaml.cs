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
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace PixivUWP.Pages.DetailPage
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class Win_WorkImgs : Windows.UI.Xaml.Controls.Page
    {
        public Win_WorkImgs()
        {
            this.InitializeComponent();
        }
        IllustWork work;
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            work = e.Parameter as IllustWork;
            flipview.ItemsSource = work.meta_pages;
        }

        private async void Image_DataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
        {
            var page=args.NewValue as MetaPages;
            var img = sender as Image;
            using (var stream = await Data.TmpData.CurrentAuth.Tokens.SendRequestAsync(Pixeez.MethodType.GET, page.ImageUrls.Original?? page.ImageUrls.Large?? page.ImageUrls.Medium))
            {
                var bitmap = new Windows.UI.Xaml.Media.Imaging.BitmapImage();
                await bitmap.SetSourceAsync((await stream.GetResponseStreamAsync()).AsRandomAccessStream());
                img.Source = bitmap;
            }
        }

        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            if((sender as AppBarToggleButton).IsChecked==false)
            {
                Windows.UI.ViewManagement.ApplicationView.GetForCurrentView().ExitFullScreenMode();
            }
            else
            {
                if (!Windows.UI.ViewManagement.ApplicationView.GetForCurrentView().TryEnterFullScreenMode())
                {
                    (sender as AppBarToggleButton).IsChecked = false;
                }
            }

        }
    }
}
