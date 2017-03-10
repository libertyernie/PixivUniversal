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
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Yinyue200.OperationDeferral;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace PixivUWP.Pages
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class Win_UserInfo : Windows.UI.Xaml.Controls.Page, DetailPage.IRefreshable,IBackable
    {
        public Win_UserInfo()
        {
            this.InitializeComponent();
            list.LoadingMoreItems += List_LoadingMoreItems;
            list.HasMoreItemsEvent += List_HasMoreItemsEvent;
            list_fav.LoadingMoreItems += List_fav_LoadingMoreItems;
            list_fav.HasMoreItemsEvent += List_fav_HasMoreItemsEvent;
            mdc.MasterListView = MasterListView;
            mdc_fav.MasterListView = MasterListView_fav;
            MasterListView.ItemsSource = list;
            MasterListView_fav.ItemsSource = list_fav;
        }
        string nexturl = null;
        string nexturl_fav = null;

        private void List_fav_HasMoreItemsEvent(ItemViewList<Work> sender, ValuePackage<bool> args)
        {
            args.Value = nexturl_fav != string.Empty;
        }

        private async void List_fav_LoadingMoreItems(ItemViewList<Work> sender, Tuple<OperationDeferral<uint>, uint> args)
        {
            var nowcount = list.Count;
            try
            {
                var root = nexturl_fav == null ? await Data.TmpData.CurrentAuth.Tokens.GetUserFavoriteWorksAsync(pix_user.Id.Value) : await Data.TmpData.CurrentAuth.Tokens.AccessNewApiAsync<Illusts>(nexturl_fav);
                nexturl = root.next_url ?? string.Empty;
                foreach (var one in root.illusts)
                {
                    if (!list.Contains(one, Data.WorkEqualityComparer.Default))
                        list.Add(one);
                }
            }
            catch
            {
                nexturl = string.Empty;
            }
            finally
            {
                args.Item1.Complete((uint)(list.Count - nowcount));
            }
        }

        private async void List_LoadingMoreItems(ItemViewList<Work> sender, Tuple<OperationDeferral<uint>, uint> args)
        {
            var nowcount = list.Count;
            try
            {
                var root = nexturl == null ? await Data.TmpData.CurrentAuth.Tokens.GetUserWorksAsync(pix_user.Id.Value) : await Data.TmpData.CurrentAuth.Tokens.AccessNewApiAsync<Illusts>(nexturl);
                nexturl = root.next_url ?? string.Empty;
                foreach (var one in root.illusts)
                {
                    if (!list.Contains(one, Data.WorkEqualityComparer.Default))
                        list.Add(one);
                }
            }
            catch
            {
                nexturl = string.Empty;
            }
            finally
            {
                args.Item1.Complete((uint)(list.Count - nowcount));
            }
        }

        private void List_HasMoreItemsEvent(ItemViewList<Work> sender, ValuePackage<bool> args)
        {
            args.Value = nexturl != string.Empty;
        }

        private void MasterListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            (sender == MasterListView?mdc:mdc_fav).MasterListView_ItemClick(typeof(DetailPage.WorkDetailPage), e.ClickedItem);
        }

        private async void Image_DataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
        {
            await Data.TmpData.LoadPictureAsync(sender);
        }

        User pix_user;
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            pix_user = e.Parameter as User;
            await RefreshAsync();
        }

        public async Task RefreshAsync()
        {
            try
            {
                pivot.Title = pix_user.Name + (!string.IsNullOrEmpty(pix_user.Email) ? "(" + pix_user.Email + ")" : string.Empty);
                string imgurl = pix_user.ProfileImageUrls.Px170x170 ?? pix_user.ProfileImageUrls.Px50x50 ?? pix_user.ProfileImageUrls.Px16x16;
                if(imgurl==null)
                {
                    pix_user = (await Data.TmpData.CurrentAuth.Tokens.GetUsersAsync(pix_user.Id.Value)).Single();
                    imgurl = pix_user.ProfileImageUrls.Px170x170;
                }
                using (var res = await PixivUWP.Data.TmpData.CurrentAuth.Tokens.SendRequestToGetImageAsync(Pixeez.MethodType.GET, imgurl))
                {
                    var bitmap = new Windows.UI.Xaml.Media.Imaging.BitmapImage();
                    await bitmap.SetSourceAsync((await res.GetResponseStreamAsync()).AsRandomAccessStream());
                    userpro.Source = bitmap;
                }
            }
            catch { }
        }
        ItemViewList<Work> list = new ItemViewList<Work>();
        ItemViewList<Work> list_fav = new ItemViewList<Work>();
        private void PivotItem_Loaded(object sender, RoutedEventArgs e)
        {
            //MasterListView.ItemsSource = await Data.TmpData.CurrentAuth.Tokens.GetUsersWorksAsync(pix_user.Id.Value);
        }

        private void PivotItem_Loaded_1(object sender, RoutedEventArgs e)
        {
            //MasterListView_fav.ItemsSource = await Data.TmpData.CurrentAuth.Tokens.GetUserFavoriteWorksAsync(pix_user.Id.Value);
        }

        public bool GoBack()
        {
            switch(pivot.SelectedIndex)
            {
                default:
                case 0:
                    return false;
                case 1:
                    return mdc.GoBack();
                case 2:
                    return mdc_fav.GoBack();
            }
        }
    }
}
