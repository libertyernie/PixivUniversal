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
using Windows.System.Profile;

namespace PixivUWP.Data
{
    class VersionHelper
    {
        public static Version GetThisAppVersionString()
        {
            var a = Windows.ApplicationModel.Package.Current.Id.Version;
            return new Version(a.Major,a.Minor, a.Build,a.Revision);
        }
        /// <summary>
        /// 获取当前操作系统版本
        /// </summary>
        /// <returns></returns>
        public static string GetOperatingSystemVersion()
        {
            string sv = AnalyticsInfo.VersionInfo.DeviceFamilyVersion;
            ulong v = ulong.Parse(sv);
            ulong v1 = (v & 0xFFFF000000000000L) >> 48;
            ulong v2 = (v & 0x0000FFFF00000000L) >> 32;
            ulong v3 = (v & 0x00000000FFFF0000L) >> 16;
            ulong v4 = v & 0x000000000000FFFFL;
            string version = $"{v1}.{v2}.{v3}.{v4}";

            return version;
        }

    }
}
