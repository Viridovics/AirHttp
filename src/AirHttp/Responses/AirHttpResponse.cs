using System;
using System.Net;
using AirHttp.Responses.Interfaces;

namespace AirHttp.Responses
{
    internal class AirHttpResponse : IAirHttpResponse
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

        private HttpWebResponse _serverResponse;
        protected HttpWebResponse ServerResponse 
        { 
            get
            {
                if (Failed)
                {
                    throw FaultException;
                }
                return _serverResponse;
            } 
            set
            {
                _serverResponse = value;
            } 
        }
    }
}