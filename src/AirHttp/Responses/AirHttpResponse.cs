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

        internal static AirHttpResponse CreateSuccessfulResponse(HttpWebResponse httpWebResponse)
        {
            return new AirHttpResponse
            {
                Failed = false,
                OriginalResponse = httpWebResponse
            };
        }

        public HttpStatusCode StatusCode
        {
            get
            {
                return OriginalResponse.StatusCode;
            }
        }

        public long ContentLength
        {
            get
            {
                return OriginalResponse.ContentLength;
            }
        }

        public DateTime LastModified
        {
            get
            {
                return OriginalResponse.LastModified;
            }
        }

        public bool Failed { get; protected set; }
        public Exception FaultException { get; protected set; }

        private HttpWebResponse _originalResponse;
        public HttpWebResponse OriginalResponse 
        { 
            get
            {
                if (Failed)
                {
                    throw new InvalidOperationException("Response is failed. See details in FaultException", FaultException);
                }
                return _originalResponse;
            } 
            set
            {
                _originalResponse = value;
            } 
        }
    }
}