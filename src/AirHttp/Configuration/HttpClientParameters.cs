using System;

namespace AirHttp.Configuration
{
    public class HttpClientParameters : IHttpClientParameters
    {
        public int TimeoutInMilliseconds { get; set; }
        public bool SaveCookie { get; set; }
    }
}