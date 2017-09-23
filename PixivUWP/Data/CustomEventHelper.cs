using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Services.Store.Engagement;

namespace PixivUWP.Data
{
    internal static class Vote
    {
        public static bool NeedVote = true;
        public static string VoteUID = "1";
        public static string Name = "通知栏缩略图";
        public static string Title = "下载图片的缩略图调查";
        public static string Message = "下载完成的通知中，是否需要显示下载图片的缩略图。";
    }

    internal static class CustomEventHelper
    {
        public enum EventType
        {
            Vote,
            Log,
            Other
        }

        public static void LogEvent(string log,
            EventType eventType = EventType.Log)
        {
            string logmessage = eventType.ToString() + "_" + log;
            StoreServicesCustomEventLogger.GetDefault().Log(logmessage);
        }
    }
}
