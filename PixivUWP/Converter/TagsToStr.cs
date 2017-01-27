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
