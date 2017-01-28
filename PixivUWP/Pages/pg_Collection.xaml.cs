﻿//PixivUniversal
//Copyright(C) 2017 Pixeez Plus Project

//This program is free software; you can redistribute it and/or
//modify it under the terms of the GNU General Public License
//as published by the Free Software Foundation; either version 2
//of the License, or(at your option) any later version.

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

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace PixivUWP.Pages
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class pg_Collection : Windows.UI.Xaml.Controls.Page
    {
        ItemViewList<UsersFavoriteWork> list = new ItemViewList<UsersFavoriteWork>();
        public pg_Collection()
        {
            this.InitializeComponent();
            list.LoadingMoreItems += List_LoadingMoreItems;
            list.HasMoreItemsEvent += List_HasMoreItemsEvent;
            MasterListView.ItemsSource = list;
        }

        private void List_HasMoreItemsEvent(ItemViewList<UsersFavoriteWork> sender, Yinyue200.OperationDeferral.ValuePackage<bool> args)
        {
            args.Value = !isfinish;
        }

        int nowpage = 1;
        bool isfinish = false;
        private async void List_LoadingMoreItems(ItemViewList<UsersFavoriteWork> sender, Tuple<Yinyue200.OperationDeferral.OperationDeferral<uint>, uint> args)
        {
            var nowcount = list.Count;
            try
            {
                foreach (var one in await Data.TmpData.CurrentAuth.Tokens.GetMyFavoriteWorksAsync(nowpage))
                {
                    list.Add(one);
                }
                nowpage++;
            }
            catch
            {
                isfinish = true;
            }
            finally
            {
                args.Item1.Complete((uint)(list.Count - nowcount));
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            MasterListView.ItemsSource = list;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private UsersFavoriteWork _lastSelectedItem;

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
            }
            else
            {
                Grid.SetColumn(DetailContentPresenter, 1);
            }

            EntranceNavigationTransitionInfo.SetIsTargetElement(MasterListView, isNarrow);
            if (DetailContentPresenter != null)
            {
                EntranceNavigationTransitionInfo.SetIsTargetElement(DetailContentPresenter, !isNarrow);
            }
        }

        private void MasterListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var clickedItem = (UsersFavoriteWork)e.ClickedItem;
            _lastSelectedItem = clickedItem;

            if (AdaptiveStates.CurrentState == NarrowState)
            {
                // Use "drill in" transition for navigating from master list to detail view
                //Frame.Navigate(typeof(DetailPage.WorkDetailPage), clickedItem, new DrillInNavigationTransitionInfo());
                DetailContentPresenter.Navigate(typeof(DetailPage.WorkDetailPage), _lastSelectedItem.Work);

                Grid.SetColumn(DetailContentPresenter, 0);

            }
            else
            {

                Grid.SetColumn(DetailContentPresenter, 1);
                // Play a refresh animation when the user switches detail items.
                EnableContentTransitions();
            }
        }

        private void LayoutRoot_Loaded(object sender, RoutedEventArgs e)
        {
            // Assure we are displaying the correct item. This is necessary in certain adaptive cases.
            MasterListView.SelectedItem = _lastSelectedItem;
        }

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
            var img = sender as Image;
            if (img.DataContext != null)
            {
                using (var stream = await Data.TmpData.CurrentAuth.Tokens.SendRequestAsync(Pixeez.MethodType.GET, (img.DataContext as UsersFavoriteWork).Work.ImageUrls.Small))
                {
                    var bitmap = new Windows.UI.Xaml.Media.Imaging.BitmapImage();
                    await bitmap.SetSourceAsync((await stream.GetResponseStreamAsync()).AsRandomAccessStream());
                    img.Source = bitmap;
                }
            }
        }

        private void MasterListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (AdaptiveStates.CurrentState != NarrowState)
            {
                //DetailContentPresenter.;
                DetailContentPresenter.Navigate(typeof(DetailPage.WorkDetailPage), ((UsersFavoriteWork)MasterListView.SelectedValue).Work);
            }
        }
    }
}