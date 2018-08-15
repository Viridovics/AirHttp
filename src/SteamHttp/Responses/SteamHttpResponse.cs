using System;
using System.Net;

namespace SteamHttp.Responses
{
    public class SteamHttpResponse
    {
        protected SteamHttpResponse()
        {}

        internal static SteamHttpResponse CreateFaultedResponse(Exception e)
        {
            return new SteamHttpResponse
            {
                RequestFaulted = true,
                InnerException = e
            };
        }

        internal static SteamHttpResponse CreateSuccessResponse(HttpStatusCode statusCode)
        {
            return new SteamHttpResponse
            {
                RequestFaulted = false,
                StatusCode = statusCode,
            };
        }

        public HttpStatusCode StatusCode{ get; protected set; }
        public bool RequestFaulted{ get; protected set; }
        public Exception InnerException{ get; protected set; }
    }
}