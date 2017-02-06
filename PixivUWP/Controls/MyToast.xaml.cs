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

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace PixivUWP.Controls
{
    public sealed partial class MyToast : UserControl
    {
        private Popup m_Popup;

        private string m_TextBlockContent;
        private TimeSpan m_ShowTime;

        private MyToast()
        {
            this.InitializeComponent();
            m_Popup = new Popup();
            MeasurePopupSize();
            m_Popup.Child = this;
            this.Loaded += NotifyPopup_Loaded;
            this.Unloaded += NotifyPopup_Unloaded;
        }

        public MyToast(string content, TimeSpan showTime) : this()
        {
            this.m_TextBlockContent = content;
            this.m_ShowTime = showTime;
        }

        public MyToast(string content) : this(content, TimeSpan.FromSeconds(2))
        {
        }

        public void Show()
        {
            this.m_Popup.IsOpen = true;
        }

        private void MeasurePopupSize()
        {
            this.Width = Windows.UI.ViewManagement.ApplicationView.GetForCurrentView().VisibleBounds.Width;

            double marginTop = 0;
            if (Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.UI.ViewManagement.StatusBar"))
                marginTop = Windows.UI.ViewManagement.StatusBar.GetForCurrentView().OccludedRect.Height;
            this.Height = Windows.UI.ViewManagement.ApplicationView.GetForCurrentView().VisibleBounds.Height;
            this.Margin = new Thickness(0, marginTop, 0, 0);
        }

        private void NotifyPopup_Loaded(object sender, RoutedEventArgs e)
        {
            this.tbNotify.Text = m_TextBlockContent;
            this.sbOut.BeginTime = this.m_ShowTime;
            this.sbOut.Begin();
            this.sbOut.Completed += SbOut_Completed;
            Windows.UI.ViewManagement.ApplicationView.GetForCurrentView().VisibleBoundsChanged += NotifyPopup_VisibleBoundsChanged;
        }

        private void NotifyPopup_VisibleBoundsChanged(Windows.UI.ViewManagement.ApplicationView sender, object args)
        {
            MeasurePopupSize();
        }

        private void SbOut_Completed(object sender, object e)
        {
            this.m_Popup.IsOpen = false;
        }


        private void NotifyPopup_Unloaded(object sender, RoutedEventArgs e)
        {
            Windows.UI.ViewManagement.ApplicationView.GetForCurrentView().VisibleBoundsChanged -= NotifyPopup_VisibleBoundsChanged;
        }
    }
}
