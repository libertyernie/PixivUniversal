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
using PixivUWP.Pages.DetailPage;
using System.Threading.Tasks;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace PixivUWP.Pages
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class pg_Main : Windows.UI.Xaml.Controls.Page, DetailPage.IRefreshable, IBackable
    {
        ItemViewList<Work> list = new ItemViewList<Work>();
        public pg_Main()
        {
            this.InitializeComponent();
            list.LoadingMoreItems += List_LoadingMoreItems;
            list.HasMoreItemsEvent += List_HasMoreItemsEvent;
            MasterListView.ItemsSource = list;
            mdc.MasterListView = MasterListView;

        }

        private void List_HasMoreItemsEvent(ItemViewList<Work> sender, PackageTuple.WriteableTuple<bool> args)
        {
            args.Item1 = nexturl!=string.Empty;
        }

        string nexturl = null;
        private async void List_LoadingMoreItems(ItemViewList<Work> sender, Tuple<Yinyue200.OperationDeferral.OperationDeferral<uint>, uint> args)
        {
            var nowcount = list.Count;
            try
            {
                var root = nexturl==null? await Data.TmpData.CurrentAuth.Tokens.GetRecommendedWorks(): await Data.TmpData.CurrentAuth.Tokens.AccessNewApiAsync<RecommendedRootobject>(nexturl);
                nexturl = root.next_url ?? string.Empty;
                foreach (var one in root.illusts)
                {
                    if(!list.Contains(one,Data.WorkEqualityComparer.Default))
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

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            MasterListView.ItemsSource = list;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void MasterListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            mdc.MasterListView_ItemClick(typeof(DetailPage.WorkDetailPage), e.ClickedItem);
        }


        public Task RefreshAsync()
        {
            list.Clear();
            MasterListView.ItemsSource = list;
            return ((IRefreshable)mdc).RefreshAsync();
        }

        public bool GoBack()
        {
            return ((IBackable)mdc).GoBack();
        }
    }
}
