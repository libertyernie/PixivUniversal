using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Pixeez;

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
            BackgroundTaskDeferral deferral = taskInstance.GetDeferral();
            (bool isAuthed, string username, string password) = getAuth();
            if (!isAuthed)
            {
                deferral.Complete();
                return;
            }
            var token = await Auth.AuthorizeAsync(username, password, null);

            deferral.Complete();
        }
    }
}
