using System;

namespace AirHttp.Configuration
{
    public class DefaultHttpClientParameters : IHttpClientParameters
    {
        public int TimeoutInMilliseconds { get; set; } = 100000;

        public bool SaveCookie { get; set; } = true;
    }
}