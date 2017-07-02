using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage.Streams;

namespace PixivUWP.Controls
{
    public static class ShareHelper
    {
        public enum ShareType
        {
            Link,
            Protocol,
            WorkID,
            Bitmap
        }

        static DataPackage package;

        public static DataPackage GenPackage(ShareType shareType, object data)
        {
            package = new DataPackage();
            switch(shareType)
            {
                case ShareType.Link:
                    package.SetWebLink(data as Uri);
                    break;
                case ShareType.Protocol:
                    package.SetApplicationLink(data as Uri);
                    break;
                case ShareType.WorkID:
                    package.SetText(data as string);
                    break;
                case ShareType.Bitmap:
                    package.SetBitmap(data as RandomAccessStreamReference);
                    break;
                default:
                    throw new ShareTypeException();
            }
            return package;
        }
    }

    public class ShareTypeException : Exception { }
}
