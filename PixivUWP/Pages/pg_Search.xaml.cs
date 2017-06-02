using Pixeez.Objects;
using PixivUWP.Data;
using PixivUWP.Pages.DetailPage;
using PixivUWP.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace PixivUWP.Pages
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class pg_Search : Windows.UI.Xaml.Controls.Page, DetailPage.IRefreshable, IBackable,IBackHandlable
    {
        ItemViewList<Work> list = new ItemViewList<Work>();
        string _query;
        bool _bypopular;

        public pg_Search()
        {
            this.InitializeComponent();
            //list.LoadingMoreItems += List_LoadingMoreItems;
            //list.HasMoreItemsEvent += List_HasMoreItemsEvent;
            MasterListView.ItemsSource = list;
            mdc.MasterListView = MasterListView;
        }

        public BackInfo GenerateBackInfo()
            => new BackInfo { list = this.list, param = new object[] { this._query, this.nowpage } };

        private async Task firstLoadAsync()
        {
            while (scrollRoot.ScrollableHeight - 500 <= 10)
                if (await loadAsync() == false)
                    return;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            _query = e.Parameter as string;
            qText.Text = _query;
            MasterListView.ItemsSource = list;
            var result = firstLoadAsync();
        }

        bool _isLoading = false;
        private async Task<bool> loadAsync()
        {
            if (_isLoading) return true;
            Debug.WriteLine("loadAsync() called.");
            _isLoading = true;
            try
            {
                foreach (var one in await Data.TmpData.CurrentAuth.Tokens.SearchWorksAsync(_query, nowpage, 30, "text", "all", "desc", _bypopular ? "popular" : "date"))
                {
                    if (!list.Contains(one, Data.WorkEqualityComparer.Default))
                        list.Add(one);
                }
                nowpage++;
                _isLoading = false;
                return true;
            }
            catch
            {
                _isLoading = false;
                return false;
            }
        }

        //private void List_HasMoreItemsEvent(ItemViewList<Work> sender, PackageTuple.WriteableTuple<bool> args)
        //{
        //    args.Item1 = !isfinish;
        //}

        int nowpage = 1;
        //bool isfinish = false;
        //private async void List_LoadingMoreItems(ItemViewList<Work> sender, Tuple<Yinyue200.OperationDeferral.OperationDeferral<uint>, uint> args)
        //{
        //    //var list1=await Data.TmpData.CurrentAuth.Tokens.GetRecommendedWorks();
        //    var nowcount = list.Count;
        //    try
        //    {
        //        foreach (var one in await Data.TmpData.CurrentAuth.Tokens.SearchWorksAsync(_query, nowpage, 30, "text", "all", "desc", _bypopular ? "popular" : "date"))
        //        {
        //            if (!list.Contains(one, Data.WorkEqualityComparer.Default))
        //                list.Add(one);
        //        }
        //        nowpage++;
        //    }
        //    catch
        //    {
        //        isfinish = true;
        //    }
        //    finally
        //    {
        //        args.Item1.Complete((uint)(list.Count - nowcount));
        //    }
        //}

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void MasterListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            mdc.MasterListView_ItemClick(typeof(DetailPage.WorkDetailPage), e.ClickedItem);
        }


        private async void Image_DataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
        {
            try
            {
                var img = sender as Image;
                img.Source = null;
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

        private void byPopularity_Checked(object sender, RoutedEventArgs e)
        {
            Data.TmpData.StopLoading();
            _bypopular = true;
            list.Clear();
            MasterListView.ItemsSource = list;
        }

        private void byPopularity_Unchecked(object sender, RoutedEventArgs e)
        {
            Data.TmpData.StopLoading();
            _bypopular = false;
            list.Clear();
            MasterListView.ItemsSource = list;
        }

        double _originHeight = 0;
        private void scrollRoot_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            if (scrollRoot.VerticalOffset == _originHeight) return;
            _originHeight = scrollRoot.VerticalOffset;
            if (scrollRoot.VerticalOffset <= scrollRoot.ScrollableHeight - 500) return;
            var result = loadAsync();
        }
    }
}
