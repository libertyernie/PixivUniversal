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
using PixivUWP.ViewModels;
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
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using System.Threading.Tasks;
using PixivUWP.Pages.DetailPage;
using PixivUWP.Views;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace PixivUWP.Pages
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public partial class MasterDetailControl : UserControl, DetailPage.IRefreshable, IBackable
    {
        public MasterDetailControl()
        {
            this.InitializeComponent();
            DetailContentPresenter.Navigate(typeof(BlankPage));
        }

        struct backitem
        {
            public Type sender;
            public object e;
            public int selectedindex;
        }

        List<backitem> backstack = new List<backitem>();

        public ListView MasterListView
        {
            get { return (ListView)GetValue(MasterListViewProperty); }
            set { SetValue(MasterListViewProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MasterListView.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MasterListViewProperty =
            DependencyProperty.Register("MasterListView", typeof(ListView), typeof(MasterDetailControl), new PropertyMetadata(null));



        public UIElement MasterContent
        {
            get { return (UIElement)GetValue(MasterContentProperty); }
            set { SetValue(MasterContentProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MasterContent.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MasterContentProperty =
            DependencyProperty.Register("MasterContent", typeof(UIElement), typeof(MasterDetailControl), new PropertyMetadata(null));


        private void AdaptiveStates_CurrentStateChanged(object sender, VisualStateChangedEventArgs e)
        {
            UpdateForVisualState(e.NewState, e.OldState);
        }

        private void UpdateForVisualState(VisualState newState, VisualState oldState = null)
        {
            var isNarrow = newState == NarrowState;

            if (isNarrow)
            {
                // Resize down to the detail item. Don't play a transition.
                //Frame.Navigate(typeof(DetailPage.WorkDetailPage), _lastSelectedItem, new SuppressNavigationTransitionInfo());
                Grid.SetColumn(DetailContentPresenter, 0);
                Grid.SetRow(DetailContentPresenter, 1);
                DetailContentPresenter.BorderThickness = new Windows.UI.Xaml.Thickness(0, 0, 0, 0);
                if (DetailContentPresenter.Content != null)
                    DetailContentPresenter.Margin = new Windows.UI.Xaml.Thickness(0, 0 - LayoutRoot.ActualHeight, 0, 0);
                //MasterListView.SelectionMode = ListViewSelectionMode.None;
            }
            else
            {
                Grid.SetColumn(DetailContentPresenter, 1);
                Grid.SetRow(DetailContentPresenter, 0);
                DetailContentPresenter.BorderThickness = new Windows.UI.Xaml.Thickness(1, 0, 0, 0);
                DetailContentPresenter.Margin = new Windows.UI.Xaml.Thickness(0, 0, 0, 0);
                //MasterListView.SelectionMode = ListViewSelectionMode.Single;
            }
        }
        object _lastSelectedItem;
        public void MasterListView_ItemClick(Type sender, object e)
        {
            _lastSelectedItem = e;
            DetailContentPresenter.Navigate(sender, e);
            backitem tmpitem = new Pages.MasterDetailControl.backitem();
            tmpitem.sender = sender;
            tmpitem.e = e;
            tmpitem.selectedindex = MasterListView.SelectedIndex;
            backstack.Add(tmpitem);
            if (AdaptiveStates.CurrentState == NarrowState)
            {
                // Use "drill in" transition for navigating from master list to detail view
                //Frame.Navigate(typeof(DetailPage.WorkDetailPage), clickedItem, new DrillInNavigationTransitionInfo());
                Grid.SetColumn(DetailContentPresenter, 0);
                Grid.SetRow(DetailContentPresenter, 1);
                DetailContentPresenter.BorderThickness = new Windows.UI.Xaml.Thickness(0, 0, 0, 0);
                BindableMargin margin = new BindableMargin(DetailContentPresenter);
                margin.Top = 0;
                DoubleAnimationUsingKeyFrames animation = new DoubleAnimationUsingKeyFrames();
                animation.EnableDependentAnimation = true;
                EasingDoubleKeyFrame f1 = new EasingDoubleKeyFrame();
                f1.Value = 0;
                f1.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0.2));
                animation.KeyFrames.Add(f1);
                EasingDoubleKeyFrame f2 = new EasingDoubleKeyFrame();
                f2.EasingFunction = new PowerEase() { EasingMode = EasingMode.EaseInOut, Power = 4 };
                f2.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0.6));
                f2.Value = 0 - LayoutRoot.ActualHeight;
                animation.KeyFrames.Add(f2);
                Storyboard.SetTarget(animation, margin);
                Storyboard.SetTargetProperty(animation, "Top");
                Storyboard storyboard = new Storyboard();
                storyboard.Children.Add(animation);
                storyboard.Completed += delegate
                  {
                      margin.Top = 0 - LayoutRoot.ActualHeight;
                  };
                storyboard.Begin();
            }
            else
            {
                Grid.SetColumn(DetailContentPresenter, 1);
                Grid.SetRow(DetailContentPresenter, 0);
                DetailContentPresenter.BorderThickness = new Windows.UI.Xaml.Thickness(1, 0, 0, 0);
                // Play a refresh animation when the user switches detail items.
                EnableContentTransitions();
            }

        }

        //private void LayoutRoot_Loaded(object sender, RoutedEventArgs e)
        //{
        //    // Assure we are displaying the correct item. This is necessary in certain adaptive cases.
        //    MasterListView.SelectedItem = _lastSelectedItem;
        //}

        private void EnableContentTransitions()
        {
            DetailContentPresenter.ContentTransitions.Clear();
            DetailContentPresenter.ContentTransitions.Add(new EntranceThemeTransition());
        }

        private void DisableContentTransitions()
        {
            if (DetailContentPresenter != null)
            {
                DetailContentPresenter.ContentTransitions.Clear();
            }
        }

        private async void Image_DataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
        {
            try
            {
                var img = sender as Image;
                if (img.DataContext != null)
                {
                    using (var stream = await Data.TmpData.CurrentAuth.Tokens.SendRequestAsync(Pixeez.MethodType.GET, (img.DataContext as Work).ImageUrls.Small))
                    {
                        var bitmap = new Windows.UI.Xaml.Media.Imaging.BitmapImage();
                        await bitmap.SetSourceAsync((await stream.GetResponseStreamAsync()).AsRandomAccessStream());
                        img.Source = bitmap;
                    }
                }
            }
            catch
            {

            }

        }

        public async virtual Task RefreshAsync()
        {
            var ir=DetailContentPresenter.Content as IRefreshable;
            if(ir!=null)
            {
                await ir.RefreshAsync();
            }
        }

        public bool GoBack()
        {
            if (backstack.Count == 0)
                return false;
            if(backstack.Count==1)
            {
                if (AdaptiveStates.CurrentState == NarrowState)
                {
                    BindableMargin margin = new BindableMargin(DetailContentPresenter);
                    margin.Top = 0 - LayoutRoot.ActualHeight;
                    DoubleAnimationUsingKeyFrames animation = new DoubleAnimationUsingKeyFrames();
                    animation.EnableDependentAnimation = true;
                    EasingDoubleKeyFrame f1 = new EasingDoubleKeyFrame();
                    f1.Value = 0 - LayoutRoot.ActualHeight;
                    f1.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0.1));
                    animation.KeyFrames.Add(f1);
                    EasingDoubleKeyFrame f2 = new EasingDoubleKeyFrame();
                    f2.EasingFunction = new PowerEase() { EasingMode = EasingMode.EaseInOut, Power = 4 };
                    f2.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0.4));
                    f2.Value = 0;
                    animation.KeyFrames.Add(f2);
                    Storyboard.SetTarget(animation, margin);
                    Storyboard.SetTargetProperty(animation, "Top");
                    Storyboard storyboard = new Storyboard();
                    storyboard.Children.Add(animation);
                    storyboard.Completed += delegate
                      {
                          backstack.Clear();
                          DetailContentPresenter.Content = null;
                          margin.Top = 0;
                      };
                    storyboard.Begin();
                }
                else
                {
                    backstack.Clear();
                    DetailContentPresenter.Navigate(typeof(BlankPage));
                }
                MasterListView.SelectedItem = null;
                return true;
            }
            else if(backstack.Count>1)
            {
                MasterListView.SelectedIndex = backstack[backstack.Count - 1].selectedindex;
                backstack.RemoveAt(backstack.Count - 1);
                DetailContentPresenter.Navigate(backstack[backstack.Count - 1].sender, backstack[backstack.Count - 1].e);
                return true;
            }
            return false;
        }

        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if(AdaptiveStates.CurrentState == NarrowState&& DetailContentPresenter.Content != null)
                DetailContentPresenter.Margin = new Windows.UI.Xaml.Thickness(0, 0 - LayoutRoot.ActualHeight, 0, 0);
        }
    }
}
