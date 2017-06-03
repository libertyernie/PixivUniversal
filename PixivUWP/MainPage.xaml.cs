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
using Pixeez;
using Pixeez.Objects;
using PixivUWP.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

//“空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409 上有介绍

namespace PixivUWP
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Windows.UI.Xaml.Controls.Page
    {
        private Tokens token;
        //public static MainPage Current { get; private set; }

        //public Button RefrechButton
        //{
        //    get
        //    {
        //        return btn_Refresh;
        //    }
        //}

        public MainPage()
        {
            this.InitializeComponent();
            var loader = new Windows.ApplicationModel.Resources.ResourceLoader();
            menuItems[0].Label = loader.GetString("Main");
            menuItems[1].Label = loader.GetString("Feeds");
            menuItems[2].Label = loader.GetString("Mywork");
            menuItems[3].Label = loader.GetString("Collection");
            menuItems[4].Label = loader.GetString("Download");
            menuBottomItems[0].Label = loader.GetString("Settings");
            menuBottomItems[1].Label = loader.GetString("Feedback");
            version.Text = "v" + Data.VersionHelper.GetThisAppVersionString().ToString() + "β";
            Windows.UI.Core.SystemNavigationManager.GetForCurrentView().BackRequested += MainPage_BackRequested;
            var coreTitleBar = CoreApplication.GetCurrentView().TitleBar;
            coreTitleBar.ExtendViewIntoTitleBar = true;
            Window.Current.SetTitleBar(Title);
            MenuItemList.ItemsSource = menuItems;
            MenuBottomItemList.ItemsSource = menuBottomItems;
            MenuItemList.SelectedIndex = 0;
            token = Data.TmpData.CurrentAuth.Tokens;
            if (DeviceTypeHelper.GetDeviceFormFactorType() == DeviceFormFactorType.Phone)
            {
                TitlebarGrid.Visibility = Visibility.Collapsed;
            }
            else if (DeviceTypeHelper.GetDeviceFormFactorType() == DeviceFormFactorType.Tablet)
            {
                pad1.Width = new Windows.UI.Xaml.GridLength(0);
            }
            tb_Username.Text = Data.TmpData.CurrentAuth.Authorize.User.Name;
            tb_Email.Text = Data.TmpData.CurrentAuth.Authorize.User.Email;
            var asyncres = token.SendRequestAsync(MethodType.GET,
                Data.TmpData.CurrentAuth.Authorize.User.ProfileImageUrls.Px170x170, null);
            var awaiter = asyncres.GetAwaiter();
            awaiter.OnCompleted(async () =>
            {
                try
                {
                    var res = awaiter.GetResult();
                    BitmapImage img = new BitmapImage();
                    await img.SetSourceAsync((await res.GetResponseStreamAsync()).AsInputStream() as IRandomAccessStream);
                    img_BAvatar.Visibility = Visibility.Collapsed;
                    img_Avatar.ImageSource = img;
                }
                catch { }

            });
        }

        private bool checkVersion()
        {
            if ((string)Data.AppDataHelper.GetValue("last_version") == Data.VersionHelper.GetThisAppVersionString().ToString()) return false;
            Data.AppDataHelper.SetValue("last_version", Data.VersionHelper.GetThisAppVersionString().ToString());
            return true;
        }

        private void MainPage_BackRequested(object sender, Windows.UI.Core.BackRequestedEventArgs e)
        {
            if (!(MainFrame.Content is Pages.DetailPage.BlankPage))
            {
                var ib = MainFrame.Content as Pages.IBackable;
                if (ib != null && ib.GoBack() == false)
                {
                    //MainFrame.Navigate(typeof(Pages.DetailPage.BlankPage));
                    if (e != null) e.Handled = Goback();
                }
                else
                {
                    if (e != null) e.Handled = true;
                }
            }
            else
            {
                if (e != null) e.Handled = Goback();
            }
        }

        public ObservableCollection<MenuItem> menuItems = new ObservableCollection<MenuItem>()
        {
            new MenuItem() {Symbol=""},
            new MenuItem() {Symbol=""},
            new MenuItem() {Symbol=""},
            new MenuItem() {Symbol=""},
            new MenuItem() {Symbol=""}
        };
        public ObservableCollection<MenuItem> menuBottomItems = new ObservableCollection<MenuItem>()
        {
            new MenuItem() {Symbol=""},
            new MenuItem() {Symbol=""},
        };

        private void MenuToggle_Click(object sender, RoutedEventArgs e)
        {
            if(!contentroot.IsPaneOpen)
            {
                Storyboard storyboard = new Storyboard();
                DoubleAnimationUsingKeyFrames animation1 = new DoubleAnimationUsingKeyFrames()
                {
                    EnableDependentAnimation = true
                };
                EasingDoubleKeyFrame f1 = new EasingDoubleKeyFrame();
                f1.Value = 0;
                f1.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0));
                animation1.KeyFrames.Add(f1);
                EasingDoubleKeyFrame f2 = new EasingDoubleKeyFrame();
                f2.Value = 1;
                f2.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0.2));
                animation1.KeyFrames.Add(f2);
                DoubleAnimationUsingKeyFrames animation2 = new DoubleAnimationUsingKeyFrames();
                animation2.EnableDependentAnimation = true;
                EasingDoubleKeyFrame f3 = new EasingDoubleKeyFrame();
                f3.Value = 1;
                f3.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0));
                animation2.KeyFrames.Add(f3);
                EasingDoubleKeyFrame f4 = new EasingDoubleKeyFrame();
                f4.Value = 0;
                f4.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0.2));
                animation2.KeyFrames.Add(f4);
                Storyboard.SetTarget(animation1, Searchbox);
                Storyboard.SetTarget(animation2, SearchBtn);
                Storyboard.SetTargetProperty(animation1, "Opacity");
                Storyboard.SetTargetProperty(animation2, "Opacity");
                storyboard.Children.Add(animation1);
                storyboard.Children.Add(animation2);
                storyboard.Completed += delegate
                  {
                      Searchbox.Opacity = 1;
                      Searchbox.Visibility = Visibility.Visible;
                      SearchBtn.Opacity = 0;
                      SearchBtn.Visibility = Visibility.Collapsed;
                  };
                Searchbox.Visibility = Visibility.Visible;
                storyboard.Begin();
            }
            contentroot.IsPaneOpen = !contentroot.IsPaneOpen;
            //var _sender = sender as ToggleButton;
            //Storyboard storyboard = new Storyboard();
            //if(_sender.IsChecked==true)
            //{
            //    DoubleAnimationUsingKeyFrames animation = new DoubleAnimationUsingKeyFrames();
            //    animation.EnableDependentAnimation = true;
            //    EasingDoubleKeyFrame f1 = new EasingDoubleKeyFrame();
            //    f1.Value = 48;
            //    f1.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0));
            //    animation.KeyFrames.Add(f1);
            //    EasingDoubleKeyFrame f2 = new EasingDoubleKeyFrame();
            //    f2.EasingFunction = new PowerEase() { EasingMode = EasingMode.EaseOut, Power = 4 };
            //    f2.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0.3));
            //    f2.Value = 256;
            //    animation.KeyFrames.Add(f2);
            //    Storyboard.SetTarget(animation, MainMenu);
            //    Storyboard.SetTargetProperty(animation, "Width");
            //    storyboard.Children.Add(animation);
            //    storyboard.Begin();
            //}
            //else
            //{
            //    DoubleAnimationUsingKeyFrames animation = new DoubleAnimationUsingKeyFrames();
            //    animation.EnableDependentAnimation = true;
            //    EasingDoubleKeyFrame f1 = new EasingDoubleKeyFrame();
            //    f1.Value = 256;
            //    f1.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0));
            //    animation.KeyFrames.Add(f1);
            //    EasingDoubleKeyFrame f2 = new EasingDoubleKeyFrame();
            //    f2.EasingFunction = new PowerEase() { EasingMode = EasingMode.EaseOut, Power = 4 };
            //    f2.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0.3));
            //    f2.Value = 48;
            //    animation.KeyFrames.Add(f2);
            //    Storyboard.SetTarget(animation, MainMenu);
            //    Storyboard.SetTargetProperty(animation, "Width");
            //    storyboard.Children.Add(animation);
            //    storyboard.Begin();
            //}
        }

        private void MenuItemList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Data.TmpData.isBackTrigger)
            {
                Data.TmpData.isBackTrigger = false;
                return;
            }
            if (MenuItemList.SelectedIndex == -1)
            {
                return;
            }
            MenuBottomItemList.SelectedIndex = -1;
            var tmpInfo = (MainFrame.Content as Data.IBackHandlable)?.GenerateBackInfo();
            if (tmpInfo != null)
                Data.UniversalBackHandler.AddPage(MainFrame.Content.GetType(), tmpInfo);
            Data.TmpData.StopLoading();
            switch (MenuItemList.SelectedIndex)
            {
                case 0:
                    MainFrame.Navigate(typeof(Pages.pg_Main));
                    break;
                case 1:
                    MainFrame.Navigate(typeof(Pages.pg_Feeds));
                    break;
                case 2:
                    MainFrame.Navigate(typeof(Pages.pg_Mywork));
                    break;
                case 3:
                    MainFrame.Navigate(typeof(Pages.pg_Collection));
                    break;
                case 4:
                    MainFrame.Navigate(typeof(Pages.pg_Download));
                    break;
            }
            contentroot.IsPaneOpen = false;
        }

        private async void btn_Lock_Click(object sender, RoutedEventArgs e)
        {
            if (contentroot.Visibility == Visibility.Collapsed)
            {
                switch (await Windows.Security.Credentials.UI.UserConsentVerifier.RequestVerificationAsync("验证您的身份"))
                {
                    case Windows.Security.Credentials.UI.UserConsentVerificationResult.Verified:
                        break;
                    case Windows.Security.Credentials.UI.UserConsentVerificationResult.DeviceNotPresent:
                    case Windows.Security.Credentials.UI.UserConsentVerificationResult.NotConfiguredForUser:
                    case Windows.Security.Credentials.UI.UserConsentVerificationResult.DisabledByPolicy:
                        await new Windows.UI.Popups.MessageDialog("当前识别设备未配置或被系统策略禁用，将默认通过验证").ShowAsync();
                        break;
                    case Windows.Security.Credentials.UI.UserConsentVerificationResult.DeviceBusy:
                    case Windows.Security.Credentials.UI.UserConsentVerificationResult.RetriesExhausted:
                    case Windows.Security.Credentials.UI.UserConsentVerificationResult.Canceled:
                    default:
                        await new Windows.UI.Popups.MessageDialog("当前识别设备不可用").ShowAsync();
                        btn_Lock.IsChecked = true;
                        return;
                }
                btn_Lock.IsChecked = false;
                await setvis(Visibility.Visible);
            }
            else
            {
                await setvis(Visibility.Collapsed);
                btn_Lock.IsChecked = true;
            }
        }

        private async System.Threading.Tasks.Task setvis(Visibility vis)
        {
            contentroot.Visibility = vis;
            foreach (var one in CoreApplication.Views)
            {
                if (one != CoreApplication.MainView)
                {
                    await one.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                    {
                        Window.Current.Content.Visibility = vis;
                    });
                }
            }
        }

        private async void MenuBottomItemList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Data.TmpData.isBackTrigger)
            {
                Data.TmpData.isBackTrigger = false;
                return;
            }
            if (MenuBottomItemList.SelectedIndex == -1)
            {
                return;
            }
            var tmpInfo = (MainFrame.Content as Data.IBackHandlable).GenerateBackInfo();
            Data.UniversalBackHandler.AddPage(MainFrame.Content.GetType(), tmpInfo);
            Data.TmpData.StopLoading();
            MenuItemList.SelectedIndex = -1;
            switch (MenuBottomItemList.SelectedIndex)
            {
                case 0:
                    MainFrame.Navigate(typeof(Pages.pg_Settings));
                    break;
                case 1:
                    if(Microsoft.Services.Store.Engagement.StoreServicesFeedbackLauncher.IsSupported())
                    {
                        await Microsoft.Services.Store.Engagement.StoreServicesFeedbackLauncher.GetDefault().LaunchAsync();
                    }
                    else
                    {
                        await Windows.System.Launcher.LaunchUriAsync(new Uri("https://git.oschina.net/ThomasWFan/PixivUniversal/issues", UriKind.Absolute));
                    }
                    goto case 0;
            }
            contentroot.IsPaneOpen = false;

        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            //Current = null;
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            var loader = new Windows.ApplicationModel.Resources.ResourceLoader();
            if (checkVersion())
                await new Windows.UI.Popups.MessageDialog(loader.GetString("PopupContent"), loader.GetString("PopupTitle")).ShowAsync();
        }

        private void btn_Back_Click(object sender, RoutedEventArgs e)
        {
            MainPage_BackRequested(null, null);
        }

        private bool Goback()
        {
            var backInfo = Data.UniversalBackHandler.Back();
            if (backInfo == null) return false;

            return true;
        }

        private void btn_Refresh_Click(object sender, RoutedEventArgs e)
        {
            //btn_Refresh.IsEnabled = false;
            //var obj = MainFrame.Content as Pages.DetailPage.IRefreshable;
            //try
            //{
            //    await obj.RefreshAsync();
            //}
            //finally
            //{
            //    btn_Refresh.IsEnabled = true;
            //}
            Data.TmpData.StopLoading();
            if (MainFrame.Content is Pages.pg_Search)
            {
                MainFrame.Navigate(typeof(Pages.pg_Search), currentQueryString);
                return;
            }
            MainFrame.Navigate(MainFrame.Content.GetType());
        }

        private void contentroot_PaneClosing(SplitView sender, SplitViewPaneClosingEventArgs args)
        {
            Storyboard storyboard = new Storyboard();
            DoubleAnimationUsingKeyFrames animation1 = new DoubleAnimationUsingKeyFrames()
            {
                EnableDependentAnimation = true
            };
            EasingDoubleKeyFrame f1 = new EasingDoubleKeyFrame();
            f1.Value = 0;
            f1.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0));
            animation1.KeyFrames.Add(f1);
            EasingDoubleKeyFrame f2 = new EasingDoubleKeyFrame();
            f2.Value = 1;
            f2.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0.2));
            animation1.KeyFrames.Add(f2);
            DoubleAnimationUsingKeyFrames animation2 = new DoubleAnimationUsingKeyFrames();
            animation2.EnableDependentAnimation = true;
            EasingDoubleKeyFrame f3 = new EasingDoubleKeyFrame();
            f3.Value = 1;
            f3.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0));
            animation2.KeyFrames.Add(f3);
            EasingDoubleKeyFrame f4 = new EasingDoubleKeyFrame();
            f4.Value = 0;
            f4.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0.2));
            animation2.KeyFrames.Add(f4);
            Storyboard.SetTarget(animation2, Searchbox);
            Storyboard.SetTarget(animation1, SearchBtn);
            Storyboard.SetTargetProperty(animation1, "Opacity");
            Storyboard.SetTargetProperty(animation2, "Opacity");
            storyboard.Children.Add(animation1);
            storyboard.Children.Add(animation2);
            storyboard.Completed += delegate
            {
                Searchbox.Opacity = 0;
                Searchbox.Visibility = Visibility.Collapsed;
                SearchBtn.Opacity = 1;
                SearchBtn.Visibility = Visibility.Visible;
            };
            SearchBtn.Visibility = Visibility.Visible;
            storyboard.Begin();
        }

        private void SearchBtn_Click(object sender, RoutedEventArgs e)
        {
            MenuToggle.IsChecked = true;
            MenuToggle_Click(null, null);
        }

        string currentQueryString;

        private void Searchbox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            if (args.QueryText == "") return;
            MenuToggle.IsChecked = false;
            MenuToggle_Click(null, null);
            MenuItemList.SelectedIndex = -1;
            var tmpInfo = (MainFrame.Content as Data.IBackHandlable).GenerateBackInfo();
            Data.UniversalBackHandler.AddPage(MainFrame.Content.GetType(), tmpInfo);
            Data.TmpData.StopLoading();
            MainFrame.Navigate(typeof(Pages.pg_Search), args.QueryText);
            currentQueryString = args.QueryText;
        }
    }
}
