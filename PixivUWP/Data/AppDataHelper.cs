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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixivUWP.Data
{
    //引用自:http://www.cnblogs.com/tonge/p/4760217.html
    public static class AppDataHelper
    {
        #region 字段
        /// <summary>
        /// 获取应用的设置容器
        /// </summary>
        private static Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;

        /// <summary>
        /// 获取独立存储文件
        /// </summary>
        private static Windows.Storage.StorageFolder localFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
        #endregion

        #region Set应用设置(简单设置，容器中的设置)
        /// <summary>
        /// 简单设置
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void SetValue(string key, string value)
            => localSettings.Values[key] = value;

        /// <summary>
        /// 创建设置容器
        /// </summary>
        /// <param name="containerName"></param>
        /// <returns></returns>
        private static Windows.Storage.ApplicationDataContainer CreateContainer(string containerName)
            => localSettings.CreateContainer(containerName, Windows.Storage.ApplicationDataCreateDisposition.Always);

        /// <summary>
        /// 讲设置保存到设置容器
        /// </summary>
        /// <param name="containerName"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void SetContainerValue(string containerName, string key, string value)
        {
            if (!localSettings.Containers.ContainsKey(containerName))
                CreateContainer(containerName);
            localSettings.Containers[containerName].Values[key] = value;
        }
        #endregion

        #region Get应用设置(简单设置，容器中的设置)
        /// <summary>
        /// 获取应用设置
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static object GetValue(string key)
            => localSettings.Values[key];

        /// <summary>
        /// 从设置容器中获取应用设置
        /// </summary>
        /// <returns></returns>
        public static object GetValueByContainer(string containerName, string key)
        {
            bool hasContainer = localSettings.Containers.ContainsKey(containerName);

            if (hasContainer)
            {
                return localSettings.Containers[containerName].Values.ContainsKey(key);
            }
            return null;
        }
        #endregion

        #region Remove已完成的设置
        /// <summary>
        /// 删除简单设置或复合设置
        /// </summary>
        /// <param name="key"></param>
        public static void Remove(string key)
            => localSettings.Values.Remove(key);

        /// <summary>
        /// 删除设置容器
        /// </summary>
        public static void RemoveContainer(string containerName)
            => localSettings.DeleteContainer(containerName);

        #endregion
    }
}
