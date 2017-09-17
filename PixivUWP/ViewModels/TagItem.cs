using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixivUWP.ViewModels
{
    public class TagItem
    {
        public string Tag { get; set; }
        public TagItem(string tag)
        {
            Tag = tag;
        }
    }
}
