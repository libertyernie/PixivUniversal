//PixivUniversal
//Copyright(C) 2017 Pixiv Plus Project

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
using Windows.UI.Xaml.Data;

namespace PixivUWP.Converter
{
    public class TagsToStr : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if(value==null)
            {
                return null;
            }
            var array = (IList<string>)value;
            var sb = new StringBuilder();
            foreach(var one in array)
            {
                sb.Append(one);
                sb.Append(" ");
            }
            return sb.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
