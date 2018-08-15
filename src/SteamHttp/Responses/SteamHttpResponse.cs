using System;
using System.Net;

namespace SteamHttp.Responses
{
    public class SteamHttpResponse
    {
        protected SteamHttpResponse()
        { }

        internal static SteamHttpResponse CreateFaultedResponse(Exception e)
        {
            return new SteamHttpResponse
            {
                RequestFaulted = true,
                InnerException = e
            };
        }

        internal static SteamHttpResponse CreateSuccessResponse(HttpWebResponse httpWebResponse)
        {
            return new SteamHttpResponse
            {
                RequestFaulted = false,
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

        public bool RequestFaulted { get; protected set; }
        public Exception InnerException { get; protected set; }

        protected HttpWebResponse ServerResponse { get; set; }
    }
}