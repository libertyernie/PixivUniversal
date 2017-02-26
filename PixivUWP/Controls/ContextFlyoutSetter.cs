//PixivUniversal
//Copyright(C) 2017 Pixeez Plus Project

//This program is free software; you can redistribute it and/or
//modify it under the terms of the GNU General Public License
//as published by the Free Software Foundation; version 2s
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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace PixivUWP.Controls
{
    /// <summary>
    /// 从越飞阅读里copy过来
    /// </summary>
    public static class ContextFlyoutSetter
    {
        public static Windows.UI.Xaml.Controls.Primitives.FlyoutBase GetCompatibleContextFlyout(FrameworkElement obj)
        {
            return (Windows.UI.Xaml.Controls.Primitives.FlyoutBase)obj.GetValue(CompatibleContextFlyoutProperty);
        }

        public static void SetCompatibleContextFlyout(FrameworkElement obj, Windows.UI.Xaml.Controls.Primitives.FlyoutBase value)
        {
            obj.SetValue(CompatibleContextFlyoutProperty, value);
        }
        static readonly bool issupport = Windows.Foundation.Metadata.ApiInformation.IsPropertyPresent("Windows.UI.Xaml.UIElement", "ContextFlyout");
        // Using a DependencyProperty as the backing store for ContextFlyout.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CompatibleContextFlyoutProperty =
            DependencyProperty.RegisterAttached("CompatibleContextFlyout", typeof(Windows.UI.Xaml.Controls.Primitives.FlyoutBase), typeof(FrameworkElement), new PropertyMetadata(null, CompatibleContextFlyoutChanged));
        private static void CompatibleContextFlyoutChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (obj is FrameworkElement uie)
            {
                if (issupport)
                {
                    uie.ContextFlyout = e.NewValue as Windows.UI.Xaml.Controls.Primitives.FlyoutBase;
                }
                else
                {
                    Windows.UI.Xaml.Controls.Primitives.FlyoutBase.SetAttachedFlyout(uie, e.NewValue as Windows.UI.Xaml.Controls.Primitives.FlyoutBase);
                    if (e.NewValue != null)
                    {
                        uie.Holding += Uie_Holding;
                        uie.RightTapped += Uie_RightTapped;
                    }
                    else
                    {
                        uie.Holding -= Uie_Holding;
                        uie.RightTapped -= Uie_RightTapped;
                    }
                }
            }
        }

        private static void Uie_Holding(object sender, Windows.UI.Xaml.Input.HoldingRoutedEventArgs e)
        {
            Windows.UI.Xaml.Controls.Primitives.FlyoutBase.ShowAttachedFlyout(sender as FrameworkElement);
        }

        private static void Uie_RightTapped(object sender, Windows.UI.Xaml.Input.RightTappedRoutedEventArgs e)
        {
            Windows.UI.Xaml.Controls.Primitives.FlyoutBase.ShowAttachedFlyout(sender as FrameworkElement);
        }
    }
}
