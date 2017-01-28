//PixivUniversal
//Copyright(C) 2017 Pixeez Plus Project

//This program is free software; you can redistribute it and/or
//modify it under the terms of the GNU General Public License
//as published by the Free Software Foundation; either version 2
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
using Windows.UI.Xaml.Controls;

namespace PixivUWP
{
    /// <summary>
    /// 从越飞阅读里copy过来
    /// </summary>
    static class ProgressBarVisualHelper
    {
        public static bool? GetYFHelperVisibility(FrameworkElement obj)
        {
            return (bool?)obj.GetValue(YFHelperVisibilityProperty);
        }

        public static void SetYFHelperVisibility(FrameworkElement obj, bool? value)
        {
            obj.SetValue(YFHelperVisibilityProperty, value);
        }

        public static readonly DependencyProperty YFHelperVisibilityProperty =
            DependencyProperty.RegisterAttached("YFHelperVisibility", typeof(bool?), typeof(FrameworkElement), new PropertyMetadata(null, OnYFHelperVisibilityChanged));
        private static void OnYFHelperVisibilityChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            var value = e.NewValue as bool?;
            if (value != null)
            {
                bool v2 = value.Value;
                var element = obj as ProgressBar;
                var element2 = obj as ProgressRing;
                if (element != null)
                {
                    element.Visibility = v2 ? Visibility.Visible : Visibility.Collapsed;
                    element.IsIndeterminate = v2;
                }
                else if (element2 != null)
                {
                    element2.Visibility = v2 ? Visibility.Visible : Visibility.Collapsed;
                    element2.IsActive = v2;
                }
            }
        }

    }
}
