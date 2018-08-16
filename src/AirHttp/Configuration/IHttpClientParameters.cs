using System;

namespace AirHttp.Configuration
{
    public interface IHttpClientParameters
    {
        int TimeoutInMilliseconds { get; }
        bool SaveCookie { get; }
    }
}