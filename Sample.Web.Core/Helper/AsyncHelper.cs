using System;
using System.Threading.Tasks;

namespace Sample.Web.Core.Helper
{
    public static class AsyncHelper
    {
        public static void Sync(Func<Task> func) => Task.Run(func).Wait();

        public static T Sync<T>(Func<Task<T>> func) => Task.Run(func).Result;

    }
}