using System;
using System.Net;
using System.Text;

namespace AirHttp.Configuration
{
    public interface IHttpClientParameters
    {
        int TimeoutInMilliseconds { get; }
        bool SaveCookie { get; }
        Encoding Encoding { get; }
        IWebProxy Proxy { get; }
    }
}