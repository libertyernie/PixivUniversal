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
                Work.SetBookMarkedValue(fs.IsChecked==true);
            }
            catch(Exception ex)
            {
                if (!(ex is NullReferenceException))
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
            gz.IsEnabled = false;

            try
            {
                siz.Text = "(" + Work.Height?.ToString() + "x" + Work.Width?.ToString() + ") " + new Converter.TagsToStr().Convert(Work.Tools, null, null, null).ToString();
                fs.IsChecked = Work.IsBookMarked();
                string url = Work is IllustWork ? Work.ImageUrls.Large : Work.ImageUrls.Medium;
                des.Text = Work.Caption ?? string.Empty;
                title.Text = Work.Title;
                user.Text = Work.User.Name+"("+Work.GetCreatedDate().ToString()+")"   /* + "(创建与更新时间：" + Work.CreatedTime.LocalDateTime.ToString() + "," + Work.ReuploadedTime.ToString() + ")"*/;
                tags.Text = new Converter.TagsToStr().Convert(Work.Tags, null, null, null).ToString();
                using (var stream = await Data.TmpData.CurrentAuth.Tokens.SendRequestAsync(Pixeez.MethodType.GET, url))
                {
                    var bitmap = new Windows.UI.Xaml.Media.Imaging.BitmapImage();
                    await bitmap.SetSourceAsync((await stream.GetResponseStreamAsync()).AsRandomAccessStream());
                    bigimg.Source = bitmap;
                }
                if (Work.User.is_followed.HasValue)
                    gz.IsChecked = Work.User.is_followed;
                else
                {
                    var user=await Data.TmpData.CurrentAuth.Tokens.GetUsersAsync(Work.User.Id.Value);
                    if (user[0].IsFollowing.HasValue)
                        gz.IsChecked = user[0].IsFollowing;
                    else
                        gz.Visibility = Visibility.Collapsed;
                }
                gz.IsEnabled = true;
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

        private async void gz_Click(object sender, RoutedEventArgs e)
        {
            gz.IsEnabled = false;
            try
            {
                if(gz.IsChecked==true)
                    await Data.TmpData.CurrentAuth.Tokens.AddFavouriteUser(Work.User.Id.Value);            
                else
                    await Data.TmpData.CurrentAuth.Tokens.DeleteFavouriteUser(Work.User.Id.Value.ToString());
                Work.User.is_followed = gz.IsChecked;
            }
            catch
            {
                gz.IsChecked = !gz.IsChecked;
            }
            finally
            {
                gz.IsEnabled = true;
            }
        }

        private async void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            downloadbutton.IsEnabled = false;
            try
            {
                await Data.DownloadManager.AddTaskAsync(Work.ImageUrls.Original ?? (Work as Pixeez.Objects.IllustWork)?.meta_single_page.OriginalImageUrl?? Work.ImageUrls.Large , Work.Id + "_p0");
            }
            finally
            {
                downloadbutton.IsEnabled = true;
            }
        }
    }
}
