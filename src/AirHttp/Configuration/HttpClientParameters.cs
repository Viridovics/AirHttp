using System;
using System.Net;
using System.Text;

namespace AirHttp.Configuration
{
    public class HttpClientParameters : IHttpClientParameters
    {
        public int TimeoutInMilliseconds { get; set; } = 100000;
        public bool SaveCookie { get; set; } = true;
        public Encoding Encoding { get; set; } = Encoding.UTF8;
        public IWebProxy Proxy { get; set; }
        public Action<HttpWebRequest> ConfigureRequest { get; set; }

        public IRetryPolicy RetryPolicy { get; set; }
    }
}