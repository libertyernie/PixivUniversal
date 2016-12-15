using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixivUWP.ViewModels
{
    public class MainPageMaster
    {
        public string ImageURL { get; set; }
        public string Title { get; set; }
        public string Caption { get; set; }
        public string Tags { get; private set; }
        public bool IsLiked { get; set; }
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
