using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FakePoll
{
    public static class BaseAddress
    {
        private static readonly object Lock = new object();
        public static string[] AvailableAddresses =
        {
            "http://google.pl",
            "http://twitch.tv",
            "http://wykop.pl",
            "http://mpcforum.pl",
            "http://github.com",
        };

        public static string GetAddress()
        {
            lock (Lock)
            {
                var random = new Random().Next(0, AvailableAddresses.Length);
                return AvailableAddresses[random];
            }
        }
    }
}
