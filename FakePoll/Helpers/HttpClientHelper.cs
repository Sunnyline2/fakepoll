using System;
using System.Net;
using System.Net.Http;

namespace FakePoll.Helpers
{
    public static class HttpClientHelper
    {
        public static HttpClient CreateHttpClient(WebProxy proxy, int timeout = 30) => new HttpClient(CreateHttpClientHandler(proxy), true)
        {
            BaseAddress = new Uri(BaseAddress.GetAddress()),
            DefaultRequestHeaders =
            {
                {"Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8"},
                {"Accept-Encoding", "gzip, deflate"},
                {"Accept-Language", "pl-PL,pl;q=0.8,en-US;q=0.6,en;q=0.4"},
                {"User-Agent", UserAgents.GetUserAgent()},
            },
            Timeout = TimeSpan.FromSeconds(timeout),
        };

        public static HttpClientHandler CreateHttpClientHandler(WebProxy proxy) => new HttpClientHandler
        {
            AllowAutoRedirect = true,
            AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip,
            CookieContainer = new CookieContainer(),
            UseCookies = true,
            Proxy = proxy,
            UseProxy = true,
        };
    }
}