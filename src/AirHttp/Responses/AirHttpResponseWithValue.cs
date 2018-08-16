using System;
using System.Net;
using AirHttp.Responses.Interfaces;

namespace AirHttp.Responses
{
    internal class AirHttpResponse<T> : AirHttpResponse, IAirHttpResponse<T>
    {
        private T _value;

        internal static AirHttpResponse<T> CreateFaultedResponseWithValue(Exception e)
        {
            return new AirHttpResponse<T>
            {
                Failed = true,
                FaultException = e
            };
        }

        internal static AirHttpResponse<T> CreateSuccessResponseWithValue(HttpWebResponse httpWebRespons, T responseObject)
        {
            return new AirHttpResponse<T>
            {
                Failed = false,
                Value = responseObject,
                ServerResponse = httpWebRespons
            };
        }

        public T Value
        {
            get
            {
                if (Failed)
                {
                    throw FaultException;
                }
                else
                {
                    return _value;
                }
            }
            private set
            {
                _value = value;
            }
        }
    }
}