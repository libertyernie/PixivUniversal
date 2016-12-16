using PixivUWP.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

//“空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409 上有介绍

namespace PixivUWP
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            MenuItemList.ItemsSource = menuItems;
        }

        public ObservableCollection<MenuItem> menuItems = new ObservableCollection<MenuItem>()
        {
            new MenuItem() {Symbol="",Label="作品发现" },
            new MenuItem() {Symbol="",Label="最新动态" },
            new MenuItem() {Symbol="",Label="个人作品" },
            new MenuItem() {Symbol="",Label="我的收藏" },
            new MenuItem() {Symbol="",Label="下载任务" }
        };

        private void MenuToggle_Click(object sender, RoutedEventArgs e)
        {
            var _sender = sender as ToggleButton;
            Storyboard storyboard = new Storyboard();
            if(_sender.IsChecked==true)
            {
                DoubleAnimationUsingKeyFrames animation = new DoubleAnimationUsingKeyFrames();
                animation.EnableDependentAnimation = true;
                EasingDoubleKeyFrame f1 = new EasingDoubleKeyFrame();
                f1.Value = 48;
                f1.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0));
                animation.KeyFrames.Add(f1);
                EasingDoubleKeyFrame f2 = new EasingDoubleKeyFrame();
                f2.EasingFunction = new PowerEase() { EasingMode = EasingMode.EaseOut, Power = 4 };
                f2.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0.3));
                f2.Value = 256;
                animation.KeyFrames.Add(f2);
                Storyboard.SetTarget(animation, MainMenu);
                Storyboard.SetTargetProperty(animation, "Width");
                storyboard.Children.Add(animation);
                storyboard.Begin();
            }
            else
            {
                DoubleAnimationUsingKeyFrames animation = new DoubleAnimationUsingKeyFrames();
                animation.EnableDependentAnimation = true;
                EasingDoubleKeyFrame f1 = new EasingDoubleKeyFrame();
                f1.Value = 256;
                f1.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0));
                animation.KeyFrames.Add(f1);
                EasingDoubleKeyFrame f2 = new EasingDoubleKeyFrame();
                f2.EasingFunction = new PowerEase() { EasingMode = EasingMode.EaseOut, Power = 4 };
                f2.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0.3));
                f2.Value = 48;
                animation.KeyFrames.Add(f2);
                Storyboard.SetTarget(animation, MainMenu);
                Storyboard.SetTargetProperty(animation, "Width");
                storyboard.Children.Add(animation);
                storyboard.Begin();
            }
        }
    }
}
