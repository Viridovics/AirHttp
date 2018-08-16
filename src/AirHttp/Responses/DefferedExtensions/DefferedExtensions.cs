using System;
using System.Net;
using AirHttp.Responses.Interfaces;

namespace AirHttp.Responses.DefferedExtensions
{
    public static class DefferedExtensions
    {
        public static TAirHttpResponse Fail<TAirHttpResponse>(this TAirHttpResponse AirHttpResponse,
                                                                        Action<Exception> callback) 
                                                                        where TAirHttpResponse : IAirHttpResponse
        {
            if(AirHttpResponse.Failed)
            {
                callback(AirHttpResponse.FaultException);
            }
            return AirHttpResponse;
        }

        public static IAirHttpResponse Success(this IAirHttpResponse AirHttpResponse,
                                                    Action<IAirHttpResponse> callback)
        {
            if(!AirHttpResponse.Failed)
            {
                callback(AirHttpResponse);
            }
            return AirHttpResponse;
        }

        public static IAirHttpResponse<TValue> Success<TValue>(this IAirHttpResponse<TValue> AirHttpResponse,
                                                                        Action<IAirHttpResponse<TValue>> callback)
        {
            if(!AirHttpResponse.Failed)
            {
                callback(AirHttpResponse);
            }
            return AirHttpResponse;
        }

        public static IAirHttpResponse<TValue> Success<TValue>(this IAirHttpResponse<TValue> AirHttpResponse,
                                                                        Action<TValue> callback)
        {
            return AirHttpResponse.Success(r => callback(r.Value));
        }

        public static TAirHttpResponse Always<TAirHttpResponse>(this TAirHttpResponse AirHttpResponse,
                                                                        Action<IAirHttpResponse> callback) 
                                                                        where TAirHttpResponse : IAirHttpResponse
        {
            callback(AirHttpResponse);
            return AirHttpResponse;
        }
    }
}