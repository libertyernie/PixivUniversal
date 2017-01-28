//PixivUniversal
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
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml.Data;
using Yinyue200.OperationDeferral;

namespace PixivUWP.ViewModels
{
    /// <summary>
    /// 从越飞阅读代码copy过来
    /// </summary>
    /// <typeparam name="T"></typeparam>
    class /*Book*/ItemViewList<T> : ObservableCollection<T>, ISupportIncrementalLoading
    {
        //public List<string> IdList
        //{
        //    get; set;
        //}
        public event TypedEventHandler</*Book*/ItemViewList<T>, ValuePackage<bool>> HasMoreItemsEvent;
        public bool HasMoreItems
        {
            get
            {
                var vp = new ValuePackage<bool>();
                HasMoreItemsEvent.Invoke(this, vp);
                return vp.Value;
                //if (IdList == null)
                //{
                //    return false;
                //}
                //return Count < IdList.Count;
            }
        }

        private bool _isBusy = false;
        public event TypedEventHandler</*Book*/ItemViewList<T>, Tuple<OperationDeferral<uint>, uint>> LoadingMoreItems;
        public async Task<LoadMoreItemsResult> LoadMoreItemsAsync(uint count)
        {
            if (_isBusy)
            {
                return new LoadMoreItemsResult() { Count = uint.MinValue };
            }
            _isBusy = true;
            try
            {
                var op = new OperationDeferral<uint>();
                LoadingMoreItems?.Invoke(this, new Tuple<OperationDeferral<uint>, uint>(op, count));
                return new LoadMoreItemsResult { Count = await op.WaitOneAsync() };
            }
            finally
            {
                _isBusy = false;
            }
        }

        IAsyncOperation<LoadMoreItemsResult> ISupportIncrementalLoading.LoadMoreItemsAsync(uint count)
        {
            return LoadMoreItemsAsync(count).AsAsyncOperation();
        }
    }
}
