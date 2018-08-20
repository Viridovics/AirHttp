using System;
using System.Text;

namespace AirHttp.Configuration
{
    public class HttpClientParameters : IHttpClientParameters
    {
        public int TimeoutInMilliseconds { get; set; } = 100000;
        public bool SaveCookie { get; set; } = true;
        public Encoding Encoding { get; set; } = Encoding.UTF8;
    }
}