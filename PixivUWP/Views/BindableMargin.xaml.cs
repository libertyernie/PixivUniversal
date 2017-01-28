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

namespace PixivUWP.Views
{
    public partial class BindableMargin
    {
        public static readonly DependencyProperty BottomProperty = DependencyProperty.Register(nameof(Bottom), typeof(double), typeof(BindableMargin), new PropertyMetadata(default(double), BottomChanged));

        public static readonly DependencyProperty LeftProperty = DependencyProperty.Register(nameof(Left), typeof(double), typeof(BindableMargin), new PropertyMetadata(default(double), LeftChanged));

        public static readonly DependencyProperty RightProperty = DependencyProperty.Register(nameof(Right), typeof(double), typeof(BindableMargin), new PropertyMetadata(default(double), RightChanged));

        public static readonly DependencyProperty TopProperty = DependencyProperty.Register(nameof(Top), typeof(double), typeof(BindableMargin), new PropertyMetadata(default(double), TopChanged));

        private readonly FrameworkElement _owner;

        public BindableMargin(FrameworkElement owner)
        {
            if (owner == null)
            {
                throw new ArgumentNullException(nameof(owner));
            }

            _owner = owner;
        }

        public double Bottom
        {
            get
            {
                var ownerBottom = _owner.Margin.Bottom;
                var bottom = (double)GetValue(BottomProperty);
                if (ownerBottom.Equals(bottom) == false)
                {
                    SetValue(BottomProperty, ownerBottom);
                }
                return ownerBottom;
            }
            set
            {
                SetValue(BottomProperty, value);
            }
        }

        public double Left
        {
            get
            {
                var ownerLeft = _owner.Margin.Left;
                var left = (double)GetValue(LeftProperty);
                if (ownerLeft.Equals(left) == false)
                {
                    SetValue(LeftProperty, ownerLeft);
                }
                return ownerLeft;
            }
            set
            {
                SetValue(LeftProperty, value);
            }
        }

        public double Right
        {
            get
            {
                var ownerRight = _owner.Margin.Right;
                var right = (double)GetValue(RightProperty);
                if (ownerRight.Equals(right) == false)
                {
                    SetValue(RightProperty, ownerRight);
                }
                return ownerRight;
            }
            set
            {
                SetValue(RightProperty, value);
            }
        }

        public double Top
        {
            get
            {
                var ownerTop = _owner.Margin.Top;
                var top = (double)GetValue(TopProperty);
                if (ownerTop.Equals(top) == false)
                {
                    SetValue(TopProperty, ownerTop);
                }
                return ownerTop;
            }
            set
            {
                SetValue(TopProperty, value);
            }
        }

        private static void BottomChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var obj = (BindableMargin)d;
            var value = (double)e.NewValue;

            var owner = obj._owner;
            var margin = owner.Margin;
            margin.Bottom = value;
            owner.Margin = margin;
        }

        private static void LeftChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var obj = (BindableMargin)d;
            var value = (double)e.NewValue;

            var owner = obj._owner;
            var margin = owner.Margin;
            margin.Left = value;
            owner.Margin = margin;
        }

        private static void RightChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var obj = (BindableMargin)d;
            var value = (double)e.NewValue;

            var owner = obj._owner;
            var margin = owner.Margin;
            margin.Right = value;
            owner.Margin = margin;
        }

        private static void TopChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var obj = (BindableMargin)d;
            var value = (double)e.NewValue;

            var owner = obj._owner;
            var margin = owner.Margin;
            margin.Top = value;
            owner.Margin = margin;
        }
    }
}
