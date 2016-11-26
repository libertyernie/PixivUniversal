using Pixeez;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APITest
{
    public static class SensitiveData
    {
        public static async Task PixivDemo()
        {
            var tokens = await Auth.AuthorizeAsync("", "");
            Console.WriteLine(tokens.AccessToken);
            Console.ReadKey();
        }
    }
}
