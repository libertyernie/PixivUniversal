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
using Windows.UI.Xaml.Navigation;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace PixivUWP.Pages
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class pg_Settings : Page,IBackHandlable
    {
        string strithome="";
        string strqqgroup="";
        public pg_Settings()
        {
            this.InitializeComponent();
            var fileo = Windows.Storage.StorageFile.GetFileFromApplicationUriAsync(new Uri(@"ms-appx:///Contributors_ithome.txt"));
            fileo.Completed += delegate
              {
                  var file = fileo.GetResults();
                  var readstreamo = file.OpenReadAsync();
                  readstreamo.Completed += delegate
                    {
                        var readstream = readstreamo.GetResults();
                        StreamReader reader = new StreamReader(readstream.AsStream());
                        strithome = reader.ReadToEnd();
                        var fileo2 = Windows.Storage.StorageFile.GetFileFromApplicationUriAsync(new Uri(@"ms-appx:///Contributors_qqgroup.txt"));
                        fileo2.Completed += delegate
                        {
                            var file2 = fileo2.GetResults();
                            var readstreamo2 = file2.OpenReadAsync();
                            readstreamo2.Completed += delegate
                            {
                                var readstream2 = readstreamo2.GetResults();
                                StreamReader reader2 = new StreamReader(readstream2.AsStream());
                                strqqgroup = reader2.ReadToEnd();
                            };
                        };
                    };
              };
            while (strqqgroup == "") { }
            con_ithome.Text = strithome;
            con_qqgroup.Text = strqqgroup;
            try
            {
                backpolicy.SelectedIndex = (int)Data.AppDataHelper.GetValue("BackgroundTransferCostPolicy");
            }
            catch
            {
                backpolicy.SelectedIndex = 0;
            }
            try
            {
                loadpolicy.SelectedIndex = (int)Data.AppDataHelper.GetValue("LoadPolicy");
            }
            catch
            {
                loadpolicy.SelectedIndex = 1;
            }
        }

        public BackInfo GenerateBackInfo() => null;

        private async void Hyperlink_Click(Windows.UI.Xaml.Documents.Hyperlink sender, Windows.UI.Xaml.Documents.HyperlinkClickEventArgs args)
        {
            await Windows.System.Launcher.LaunchUriAsync(new Uri("ms-windows-store://pdp/?productid=9nblggh1xvd2", UriKind.Absolute));
        }

        private async void Hyperlink_Click2(Windows.UI.Xaml.Documents.Hyperlink sender, Windows.UI.Xaml.Documents.HyperlinkClickEventArgs args)
        {
            await Windows.System.Launcher.LaunchUriAsync(new Uri("https://github.com/moespirit", UriKind.Absolute));
        }

        private void backpolicy_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Data.AppDataHelper.SetValue("BackgroundTransferCostPolicy", backpolicy.SelectedIndex);
        }

        private void loadpolicy_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Data.AppDataHelper.SetValue("LoadPolicy", loadpolicy.SelectedIndex);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            try
            {
                if ((bool)((object[])e.Parameter)[0])
                {
                    Data.TmpData.isBackTrigger = true;
                    Data.TmpData.menuItem.SelectedIndex = -1;
                    Data.TmpData.menuBottomItem.SelectedIndex = 0;
                }
            }
            catch { }
        }
    }
}
