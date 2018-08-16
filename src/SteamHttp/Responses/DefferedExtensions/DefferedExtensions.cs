using System;
using System.Net;

namespace AirHttp.Responses.DefferedExtensions
{
    public static class DefferedExtensions
    {
        public static TAirHttpResponse IfFaulted<TAirHttpResponse>(this TAirHttpResponse AirHttpResponse,
                                                                        Action<Exception> callback) 
                                                                        where TAirHttpResponse : AirHttpResponse
        {
            if(AirHttpResponse.Failed)
            {
                callback(AirHttpResponse.FaultException);
            }
            return AirHttpResponse;
        }

        public static AirHttpResponse Success(this AirHttpResponse AirHttpResponse,
                                                    Action<AirHttpResponse> callback)
        {
            if(!AirHttpResponse.Failed)
            {
                callback(AirHttpResponse);
            }
            return AirHttpResponse;
        }

        public static AirHttpResponse<TValue> Success<TValue>(this AirHttpResponse<TValue> AirHttpResponse,
                                                                        Action<AirHttpResponse<TValue>> callback)
        {
            if(!AirHttpResponse.Failed)
            {
                callback(AirHttpResponse);
            }
            return AirHttpResponse;
        }

        public static AirHttpResponse<TValue> Success<TValue>(this AirHttpResponse<TValue> AirHttpResponse,
                                                                        Action<TValue> callback)
        {
            return AirHttpResponse.Success(r => callback(r.Value));
        }

        public static TAirHttpResponse Always<TAirHttpResponse>(this TAirHttpResponse AirHttpResponse,
                                                                        Action<TAirHttpResponse> callback) 
                                                                        where TAirHttpResponse : AirHttpResponse
        {
            callback(AirHttpResponse);
            return AirHttpResponse;
        }
    }
}