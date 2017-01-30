using Pixeez.Objects;
using PixivUWP.Pages.DetailPage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Threading.Tasks;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace PixivUWP.Pages
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class UserPage : Windows.UI.Xaml.Controls.Page,IRefreshable
    {
        public UserPage()
        {
            this.InitializeComponent();
        }
        User pix_user;
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            pix_user = e.Parameter as User;
            await RefreshAsync();
        }

        public async Task RefreshAsync()
        {
            try
            {
                username.Text = pix_user.Name + "(" + pix_user.Email + ")";
                using (var res = await PixivUWP.Data.TmpData.CurrentAuth.Tokens.SendRequestToGetImageAsync(Pixeez.MethodType.GET, pix_user.ProfileImageUrls.Px170x170))
                {
                    var bitmap = new Windows.UI.Xaml.Media.Imaging.BitmapImage();
                    await bitmap.SetSourceAsync((await res.GetResponseStreamAsync()).AsRandomAccessStream());
                    userpro.Source = bitmap;
                }
            }
            catch { }
        }
    }
}
