using System;
using System.Net;

namespace AirHttp.Responses
{
    public class AirHttpResponse
    {
        protected AirHttpResponse()
        { }

        internal static AirHttpResponse CreateFaultedResponse(Exception e)
        {
            return new AirHttpResponse
            {
                Failed = true,
                FaultException = e
            };
        }

        internal static AirHttpResponse CreateSuccessResponse(HttpWebResponse httpWebResponse)
        {
            return new AirHttpResponse
            {
                Failed = false,
                ServerResponse = httpWebResponse
            };
        }

        public HttpStatusCode StatusCode
        {
            get
            {
                return ServerResponse.StatusCode;
            }
        }

        public long ContentLength
        {
            get
            {
                return ServerResponse.ContentLength;
            }
        }

        public DateTime LastModified
        {
            get
            {
                return ServerResponse.LastModified;
            }
        }

        public bool Failed { get; protected set; }
        public Exception FaultException { get; protected set; }

        protected HttpWebResponse ServerResponse { get; set; }
    }
}