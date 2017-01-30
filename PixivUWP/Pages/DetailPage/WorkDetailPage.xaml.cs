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
using System.IO;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;
using System.Threading.Tasks;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace PixivUWP.Pages.DetailPage
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class WorkDetailPage : Windows.UI.Xaml.Controls.Page, IRefreshable
    {
        public WorkDetailPage()
        {
            this.InitializeComponent();
        }
        Work Work;
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            Work = e.Parameter as Work;
            await RefreshAsync();
        }

        private async void fs_Click(object sender, RoutedEventArgs e)
        {
            fs.IsEnabled = false;
            try
            {
                if (fs.IsChecked != true)
                {
                    await PixivUWP.Data.TmpData.CurrentAuth.Tokens.DeleteMyFavoriteWorksAsync(Work.Id.Value);
                }
                else
                {
                    await PixivUWP.Data.TmpData.CurrentAuth.Tokens.AddMyFavoriteWorksAsync(Work.Id.Value);
                }
            }
            catch
            {
                fs.IsChecked = !fs.IsChecked;
            }
            finally
            {
                fs.IsEnabled = true;
            }
        }

        public async Task RefreshAsync()
        {
            PixivUWP.ProgressBarVisualHelper.SetYFHelperVisibility(pro, true);
            try
            {
                siz.Text = "(" + Work.Height?.ToString() + "x" + Work.Width?.ToString() + ") " + new Converter.TagsToStr().Convert(Work.Tools, null, null, null).ToString();
                fs.IsChecked = Work.IsBookMarked();
                des.Text = Work.Caption ?? string.Empty;
                title.Text = Work.Title;
                user.Text = Work.User.Name+"("+Work.GetCreatedDate().ToString()+")"   /* + "(创建与更新时间：" + Work.CreatedTime.LocalDateTime.ToString() + "," + Work.ReuploadedTime.ToString() + ")"*/;
                tags.Text = new Converter.TagsToStr().Convert(Work.Tags, null, null, null).ToString();
                using (var stream = await Data.TmpData.CurrentAuth.Tokens.SendRequestAsync(Pixeez.MethodType.GET, Work.ImageUrls.Medium))
                {
                    var bitmap = new Windows.UI.Xaml.Media.Imaging.BitmapImage();
                    await bitmap.SetSourceAsync((await stream.GetResponseStreamAsync()).AsRandomAccessStream());
                    bigimg.Source = bitmap;
                }
            }
            catch
            {
                await new Windows.UI.Popups.MessageDialog("加载失败").ShowAsync();
            }
            finally
            {
                PixivUWP.ProgressBarVisualHelper.SetYFHelperVisibility(pro, false);
            }
        }

        private void gz_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
