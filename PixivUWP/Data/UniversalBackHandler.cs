using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixivUWP.Data
{
    static internal class UniversalBackHandler
    {
        static Stack<BackHandle> backStack = new Stack<BackHandle>();

        public static void AddPage(Type page, BackInfo info)
            => backStack.Push(new BackHandle { info = info, page = page });

        public static BackHandle Back()
            => (backStack.Count == 0) ? null : backStack.Pop();
    }

    class BackHandle
    {
        public BackInfo info
        { get; set; }

        public Type page
        { get; set; }
    }

    internal interface IBackHandlable
    {
        BackInfo GenerateBackInfo();
    }
}
