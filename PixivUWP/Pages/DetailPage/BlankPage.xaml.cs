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

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace PixivUWP.Pages.DetailPage
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class BlankPage : Page
    {
        public BlankPage()
        {
            this.InitializeComponent();
            changeLog.Text = "\n版本：v0.0.10.0β\n" +
                "本版相较上版作出的改动：\n" +
                "1、启用多窗口功能；\n" +
                "2、可以查看多图作品了；\n" +
                "3、应用锁定将能够锁定所有窗口；\n" +
                "3、可以查看“作者信息”和“相关作品”了；\n\n" +
                "已知BUG：\n"+
                "1、作者头像的大小未被固定\n"+
                "2、多图查看时可能会出现顺序混乱；\n" +
                "3、在部分移动设备（Win10M）上查看多图会出现闪退；\n\n" +
                "请志愿测试人员重点测试：\n" +
                "1、多图查看中会出现的乱序和闪退问题；\n" +
                "2、按热门度的搜索排序（需要P站高级会员）；\n\n" +
                "注意：搜索功能仅在左侧搜索框内可用";
        }
    }
}
