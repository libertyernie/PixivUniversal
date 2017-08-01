using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pixeez.Objects;
using Windows.UI.Xaml.Media.Imaging;

namespace PixivUWP.ViewModels
{
    public class CommentListItem
    {
        public Comment Comment { get; set; }
        public int LeftMargin { get; set; }
        public BitmapImage Avatar { get; set; }
    }
}
