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
using PixivUWP.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Networking.BackgroundTransfer;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace PixivUWP.Pages
{
    class DownloadTask:DependencyObject
    {

        public string Name
        {
            get { return (string)GetValue(NameProperty); }
            set { SetValue(NameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Name.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NameProperty =
            DependencyProperty.Register("Name", typeof(string), typeof(DownloadTask), new PropertyMetadata(string.Empty));



        public int MaxValue
        {
            get { return (int)GetValue(MaxValueProperty); }
            set { SetValue(MaxValueProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MaxValue.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MaxValueProperty =
            DependencyProperty.Register("MaxValue", typeof(int), typeof(DownloadTask), new PropertyMetadata(100));

        public int Value
        {
            get { return (int)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Value.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(int), typeof(DownloadTask), new PropertyMetadata(0));

    }
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class pg_Download : Page,IBackHandlable
    {
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            try
            {
                if ((bool)((object[])e.Parameter)[0])
                {
                    Data.TmpData.isBackTrigger = true;
                    Data.TmpData.menuItem.SelectedIndex = 4;
                    Data.TmpData.menuBottomItem.SelectedIndex = -1;
                }
            }
            catch { }
        }

        public pg_Download()
        {
            this.InitializeComponent();
            listview.ItemsSource = tasks;
        }
        ObservableCollection<DownloadTask> tasks = new ObservableCollection<DownloadTask>();
        List<IAsyncOperationWithProgress<DownloadOperation, DownloadOperation>> list = new List<IAsyncOperationWithProgress<DownloadOperation, DownloadOperation>>();
        Dictionary<Guid, DownloadTask> dic = new Dictionary<Guid, DownloadTask>();
        Dictionary<uint, DownloadOperation> dic2 = new Dictionary<uint, DownloadOperation>();
        public async void load()
        {
            var downloads=await BackgroundDownloader.GetCurrentDownloadsAsync();
            list.Clear();
            dic.Clear();
            tasks.Clear();
            dic2.Clear();
            foreach(var one in downloads )
            {
                var two = one.AttachAsync();
                list.Add(two);
                var dt = new DownloadTask() { Name = one.ResultFile.Name };
                tasks.Add(dt);
                dic.Add(one.Guid,dt);
                dic2.Add(two.Id, one);
                two.Progress = progresschange;
                two.Completed = progresschange;
            }
        }

        public BackInfo GenerateBackInfo() => null;

        public void progresschange(IAsyncOperationWithProgress<DownloadOperation, DownloadOperation> a, DownloadOperation b)
        {
            var task = Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
              {
                  dic[b.Guid].Value = (int)(b.Progress.BytesReceived / b.Progress.TotalBytesToReceive * 100);
              });
        }
        public void progresschange(IAsyncOperationWithProgress<DownloadOperation, DownloadOperation> a, AsyncStatus b)
        {
            var task = Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                switch (b)
                {
                    default:
                    case AsyncStatus.Started:
                    case AsyncStatus.Canceled:
                    case AsyncStatus.Completed:
                        break;
                    case AsyncStatus.Error:
                        new Controls.MyToast(dic2[a.Id].ResultFile.Name +"下载失败").Show();
                        break;
                }
                try
                {
                    tasks.Remove(dic[a.AsTask().Result.Guid]);
                }
                catch(OperationCanceledException)
                {

                }
                catch
                {
                    try
                    {
                        tasks.Remove(dic[dic2[a.Id].Guid]);
                    }
                    catch
                    {
                        new Controls.MyToast("出现未知错误" + a.Id).Show();
                    }
                }
            });
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            foreach (var one in list)
            {
                try
                {
                    one.Progress = null;
                    one.Completed = null;
                }
                catch
                {

                }
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            load();
        }

        private async void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            await Data.DownloadManager.getpicfolder();
            await Launcher.LaunchFolderAsync(Data.DownloadManager.pictureFolder);
        }

        private void cancelall_Click(object sender, RoutedEventArgs e)
        {
            foreach(var one in list)
            {
                one.Cancel();
            }
            list.Clear();
            dic.Clear();
            tasks.Clear();
            dic2.Clear();
        }
    }
}
