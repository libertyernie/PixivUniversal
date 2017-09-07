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
        public static string  VoteUID = "0";
        public static string Name = "TestVote";
        public static string Title = "Test";
        public static string Message = "This is a test vote";
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
