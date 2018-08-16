using System;
using System.Net;

namespace AirHttp.Responses.Interfaces
{
    public interface IAirHttpResponse
    {
        HttpStatusCode StatusCode { get; }

        long ContentLength { get; }

        DateTime LastModified { get; }

        bool Failed { get; }

        Exception FaultException { get; }
    }
}