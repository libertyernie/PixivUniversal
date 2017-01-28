//PixivUniversal
//Copyright(C) 2017 Pixeez Plus Project

//This program is free software; you can redistribute it and/or
//modify it under the terms of the GNU General Public License
//as published by the Free Software Foundation; either version 2
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
                var res = awaiter.GetResult();
                BitmapImage img = new BitmapImage();
                await img.SetSourceAsync((await res.GetResponseStreamAsync()).AsInputStream() as IRandomAccessStream);
                img_BAvatar.Visibility = Visibility.Collapsed;
                img_Avatar.ImageSource = img;
            });
        }

        private async void MainPage_BackRequested(object sender, Windows.UI.Core.BackRequestedEventArgs e)
        {
            e.Handled = true;
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                btn_Back_Click(null, null);
            });
        }

        public ObservableCollection<MenuItem> menuItems = new ObservableCollection<MenuItem>()
        {
            new MenuItem() {Symbol="",Label="作品发现" },
            new MenuItem() {Symbol="",Label="最新动态" },
            new MenuItem() {Symbol="",Label="个人作品" },
            new MenuItem() {Symbol="",Label="我的收藏" },
            new MenuItem() {Symbol="",Label="下载任务" }
        };
        public ObservableCollection<MenuItem> menuBottomItems = new ObservableCollection<MenuItem>()
        {
            new MenuItem() {Symbol="",Label="设置" },
            new MenuItem() {Symbol="",Label="反馈" },
        };

        private void MenuToggle_Click(object sender, RoutedEventArgs e)
        {
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
            if (MenuItemList.SelectedIndex == -1)
            {
                return;
            }
            MenuBottomItemList.SelectedIndex = -1;
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
                contentroot.Visibility = Visibility.Visible;
            }
            else
            {
                contentroot.Visibility = Visibility.Collapsed;
                btn_Lock.IsChecked = true;
            }
        }

        private void MenuBottomItemList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MenuBottomItemList.SelectedIndex == -1)
            {
                return;
            }
            MenuItemList.SelectedIndex = -1;
            switch (MenuBottomItemList.SelectedIndex)
            {
                case 0:
                    MainFrame.Navigate(typeof(Pages.pg_Settings));
                    break;
            }
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            //Current = null;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            //Current = this;
        }

        private void btn_Back_Click(object sender, RoutedEventArgs e)
        {
            if(MainFrame.Content!=null)
            {
                MainFrame.Content = null;
            }
            else
            {
                Frame.BackStack.Clear();
                if (MenuItemList.SelectedIndex != 0)
                {
                    MenuItemList.SelectedIndex = 0;
                }
                else
                {
                    Frame.GoBack();
                }
            }
        }

        private async void btn_Refresh_Click(object sender, RoutedEventArgs e)
        {
            btn_Refresh.IsEnabled = false;
            var obj = MainFrame.Content as Pages.DetailPage.IRefreshable;
            try
            {
                await obj.RefreshAsync();
            }
            finally
            {
                btn_Refresh.IsEnabled = true;
            }
        }
    }
}
