using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Pixeez;
using Windows.UI.Notifications;
using Windows.UI.Xaml.Media.Imaging;
using System.IO;
using Windows.Storage;
using Microsoft.Toolkit.Uwp.Notifications;

namespace TileBackground
{
    public sealed class TileBackground : IBackgroundTask
    {
        public static void PassAuth(string Username,string Password)
        {
            AppDataHelper.SetValue("uname", Username);
            AppDataHelper.SetValue("upasswd", Password);
        }

        private static (bool isAuthed, string username, string password) getAuth()
            => (AppDataHelper.GetValue("uname") == null || AppDataHelper.GetValue("upasswd") == null) ?
               (false, "", "") :
               (true, (string)AppDataHelper.GetValue("uname"), (string)AppDataHelper.GetValue("upasswd"));

        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            BackgroundTaskDeferral deferral = taskInstance?.GetDeferral();
            object numToday = AppDataHelper.GetValue("numtoday");
            int numtoday;
            if (numToday == null)
                numtoday = 0;
            else
                numtoday = (int)numToday;
            object dateToday = AppDataHelper.GetValue("dateToday");
            bool datetoday;
            if (dateToday == null)
                datetoday = false;
            else if ((int)dateToday == DateTime.Now.Year * 10000 + DateTime.Now.Month * 100 + DateTime.Now.Day)
                datetoday = true;
            else
                datetoday = false;
            AppDataHelper.SetValue("datetoday", DateTime.Now.Year * 10000 + DateTime.Now.Month * 100 + DateTime.Now.Day);
            if (!datetoday)
                numtoday = 1;
            else
                numtoday++;
            if (numtoday > 20)
                numtoday = 1;
            AppDataHelper.SetValue("numtoday", numtoday);
            (bool isAuthed, string username, string password) = getAuth();
            if (!isAuthed)
            {
                deferral.Complete();
                return;
            }
            var token = await Auth.AuthorizeAsync(username, password, null);
            var ranks = await token.Tokens.GetRankingAllAsync("daily", 1, 20);
            //更新磁贴
            var updater = TileUpdateManager.CreateTileUpdaterForApplication();
            updater.EnableNotificationQueue(false);
            //updater.Clear();
            using (var res = await token.Tokens.SendRequestToGetImageAsync(MethodType.GET, ranks[0].Works[numtoday - 1].Work.ImageUrls.Medium))
            {
                StorageFolder applicationdatafolder = ApplicationData.Current.TemporaryFolder;
                using (var stream = await res.GetResponseStreamAsync())
                {
                    var file = await applicationdatafolder.CreateFileAsync("tmp" + numtoday.ToString() + ".jpg", CreationCollisionOption.ReplaceExisting);
                    using (var filestream = await file.OpenStreamForWriteAsync())
                    {
                        await stream.CopyToAsync(filestream);
                    }
                }
            }
            TileBindingContentAdaptive content = new TileBindingContentAdaptive
            {
                BackgroundImage = new TileBackgroundImage
                {
                    Source = ApplicationData.Current.TemporaryFolder.Path + "\\tmp" + numtoday.ToString() + ".jpg"
                }
            };
            TileBindingContentAdaptive content_m = new TileBindingContentAdaptive
            {
                BackgroundImage = new TileBackgroundImage
                {
                    Source = ApplicationData.Current.TemporaryFolder.Path + "\\tmp" + numtoday.ToString() + ".jpg"
                }
            };
            AdaptiveSubgroup subgroupa = new AdaptiveSubgroup();
            subgroupa.Children.Add(new AdaptiveText() { Text = "Pixiv #" + numtoday, HintWrap = true });
            subgroupa.HintWeight = 3;
            AdaptiveSubgroup subgroupb = new AdaptiveSubgroup();
            subgroupb.Children.Add(new AdaptiveText() { Text = "By: " + ranks[0].Works[numtoday - 1].Work.User.Account, HintStyle = AdaptiveTextStyle.CaptionSubtle, HintWrap = true, HintAlign = AdaptiveTextAlign.Right });
            subgroupb.HintWeight = 7;
            AdaptiveGroup maingroup = new AdaptiveGroup();
            maingroup.Children.Add(subgroupa);
            maingroup.Children.Add(subgroupb);
            AdaptiveGroup maingroup_m = new AdaptiveGroup();
            maingroup_m.Children.Add(subgroupa);
            content.Children.Add(maingroup);
            content.Children.Add(new AdaptiveText() { Text = "★" + ranks[0].Works[numtoday - 1].Work.Stats.Score.ToString(), HintStyle = AdaptiveTextStyle.CaptionSubtle, HintWrap = true, HintAlign = AdaptiveTextAlign.Right });
            content_m.Children.Add(maingroup_m);
            TileBinding binding = new TileBinding
            {
                Branding = TileBranding.Name,
                DisplayName = ranks[0].Works[numtoday - 1].Work.Title,
                Content = content
            };
            TileBinding binding_m = new TileBinding
            {
                Branding = TileBranding.Name,
                DisplayName = ranks[0].Works[numtoday - 1].Work.Title,
                Content = content_m
            };
            TileContent tile = new TileContent
            {
                Visual = new TileVisual
                {
                    TileMedium = binding_m,
                    TileWide = binding,
                    TileLarge = binding
                }
            };
            var notification = new TileNotification(tile.GetXml());
            updater.Update(notification);
            deferral?.Complete();
        }
    }
}
