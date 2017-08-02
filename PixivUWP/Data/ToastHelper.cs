using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Notifications;
using Microsoft.Toolkit.Uwp.Notifications;

namespace PixivUWP.Data
{
    public static class ToastHelper
    {
        public static void SendToast(string title = "Pixiv UWP", string content = "", string image = null, string logo = null, ToastGenericAppLogoCrop hintcrop = ToastGenericAppLogoCrop.Circle)
        {
            ToastVisual visual = new ToastVisual();
            ToastBindingGeneric generic = new ToastBindingGeneric();
            generic.Children.Add(new AdaptiveText() { Text = title });
            generic.Children.Add(new AdaptiveText() { Text = content });
            if (image != null) generic.Children.Add(new AdaptiveImage { Source = image });
            if (logo != null)
            {
                generic.AppLogoOverride.Source = logo;
                generic.AppLogoOverride.HintCrop = hintcrop;
            }
            ToastContent tcontent = new ToastContent()
            {
                Visual=visual
            };
            var toast = new ToastNotification(tcontent.GetXml());
            ToastNotificationManager.CreateToastNotifier().Show(toast);
        }
    }
}
