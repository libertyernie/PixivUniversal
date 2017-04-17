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
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml.Controls;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace PixivUWP.Pages.DetailPage
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class WorkDetailPage : Windows.UI.Xaml.Controls.Page, IRefreshable
    {
        string err1 = "";
        string err2 = "";

        public WorkDetailPage()
        {
            this.InitializeComponent();
            var loader = new Windows.ApplicationModel.Resources.ResourceLoader();
            err1 = loader.GetString("ErrorLoading");
            err2 = loader.GetString("Error");
        }
        Work Work;
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            Work = e.Parameter as Work;
            await RefreshAsync();
            if (Work is IllustWork iw)
            {
                if(iw.meta_pages != null && iw.meta_pages.Length > 1)
                    watchbigimg.Visibility = Visibility.Visible;
            }
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
                await new Windows.UI.Popups.MessageDialog(err1).ShowAsync();
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

        private async void Hyperlink_Click(Windows.UI.Xaml.Documents.Hyperlink sender, Windows.UI.Xaml.Documents.HyperlinkClickEventArgs args)
        {
            //弹出该作者的信息
            await CreateNewWindowAsync(Work.User.Id.Value.ToString(), typeof(Win_UserInfo), Work.User);

        }

        private async void watchbigimg_Click(object sender, RoutedEventArgs e)
        {
            await CreateNewWindowAsync("work" + Work.Id, typeof(DetailPage.Win_WorkImgs), Work);
        }

        private static async Task CreateNewWindowAsync(string id, Type page, object par)
        {
            await CoreApplication.MainView.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
            {
                //if (Data.TmpData.OpenedWindows.TryGetValue(id, out int value))
                //{
                //    await ApplicationViewSwitcher.TryShowAsStandaloneAsync(value);
                //}
                //else
                {
                    int newViewId = -1;

                    CoreApplicationView newView = CoreApplication.CreateNewView();

                    await newView.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                    {
                        var newWindow = Window.Current;
                        var newAppView = ApplicationView.GetForCurrentView();
                        var sysnm = Windows.UI.Core.SystemNavigationManager.GetForCurrentView();
                        var frame = new Frame();
                        frame.Navigated += (s, e) =>
                        {
                            if(frame.Content is IBackable)
                                sysnm.AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
                            else
                                sysnm.AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
                        };
                        sysnm.BackRequested += (s, e) =>
                        {
                            if(frame.Content is IBackable iba)
                            {
                                if (!iba.GoBack())
                                {
                                    var a=Window.Current.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                                    {
                                        Window.Current.Close();
                                    });
                                }
                                else
                                {
                                    e.Handled = true;
                                }
                            }
                        };
                        newWindow.Closed += async(s, e) =>
                        {
                            Window.Current.Content = null;
                            await CoreApplication.MainView.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                            {
                                Data.TmpData.OpenedWindows.Remove(id);
                            });
                        };
                        frame.Navigate(page, par);
                        newWindow.Content = frame;
                        newWindow.Activate();
                        newViewId = newAppView.Id;
                    });
                    Data.TmpData.OpenedWindows[id] = newViewId;
                    var viewShown = await ApplicationViewSwitcher.TryShowAsStandaloneAsync(newViewId);

                }
            });
        }

        private async void relate_Click(object sender, RoutedEventArgs e)
        {
            await CreateNewWindowAsync("related" + Work.Id, typeof(Win_Related), Work);
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var tags = inputbox.Text.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            fs.IsEnabled = false;
            try
            {
                await PixivUWP.Data.TmpData.CurrentAuth.Tokens.AddMyFavoriteWorksAsync(Work.Id.Value, tags);
                fs.IsChecked = true;
                Work.SetBookMarkedValue(fs.IsChecked == true);
            }
            catch
            {
                new Controls.MyToast(err2).Show();
            }
            finally
            {
                fs.IsEnabled = true;
            }
        }

        private void fs_Loaded(object sender, RoutedEventArgs e)
        {
            bool allowFocusOnInteractionAvailable =
    Windows.Foundation.Metadata.ApiInformation.IsPropertyPresent(
        "Windows.UI.Xaml.FrameworkElement",
        "AllowFocusOnInteraction");

            if (allowFocusOnInteractionAvailable)
            {
                if (sender is FrameworkElement s)
                {
                    s.AllowFocusOnInteraction = true;
                }
            }
        }

        private async void inputbox_Loaded(object sender, RoutedEventArgs e)
        {
            if(sender is TextBox tb)
            {
                var sb = new System.Text.StringBuilder();
                foreach (var one in (await Data.TmpData.CurrentAuth.Tokens.GetBookMarkedDetailAsync(Work.Id.Value)).bookmark_detail.tags)
                {
                    if(one.is_registered==true)
                    {
                        if (sb.Length != 0)
                        {
                            sb.Append(',');
                        }
                        sb.Append(one.Name);
                    }
                }
                tb.Text = sb.ToString();
            }
        }
    }
}
