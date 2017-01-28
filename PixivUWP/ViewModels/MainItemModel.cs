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

namespace PixivUWP.ViewModels
{
    public class MainItemModel
    {
        public string SmallImageURL { get; set; }
        public string ImageURL { get; set; }
        public string Title { get; set; }
        public string Tags { get; private set; }
        public bool IsLiked { get; set; }
        public string Caption { get; set; }
        public string Size
        {
            get
            {
                return width.ToString() + "x" + height.ToString();
            }
        }

        private int height;
        private int width;

        public int WorkWidth
        {
            set
            {
                width = value;
            }
        }

        public int WorkHeight
        {
            set
            {
                height = value;
            }
        }

        public bool R18 { get; set; }
    }
}
