using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static APITest.SensitiveData;

namespace APITest
{
    class Program
    {
        static void Main(string[] args)
        {
            Task.Run(async () => await PixivDemo()).Wait();
        }

    }
}
