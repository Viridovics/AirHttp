using System;
using System.Net;

namespace SteamHttp.Responses.DefferedExtensions
{
    public static class DefferedExtensions
    {
        public static TSteamHttpResponse IfFaulted<TSteamHttpResponse>(this TSteamHttpResponse steamHttpResponse,
                                                                        Action<Exception> callback) 
                                                                        where TSteamHttpResponse : SteamHttpResponse
        {
            if(steamHttpResponse.RequestFaulted)
            {
                callback(steamHttpResponse.InnerException);
            }
            return steamHttpResponse;
        }

        public static SteamHttpResponse Success(this SteamHttpResponse steamHttpResponse,
                                                    Action<SteamHttpResponse> callback)
        {
            if(!steamHttpResponse.RequestFaulted)
            {
                callback(steamHttpResponse);
            }
            return steamHttpResponse;
        }

        public static SteamHttpResponse<TValue> Success<TValue>(this SteamHttpResponse<TValue> steamHttpResponse,
                                                                        Action<SteamHttpResponse<TValue>> callback)
        {
            if(!steamHttpResponse.RequestFaulted)
            {
                callback(steamHttpResponse);
            }
            return steamHttpResponse;
        }

        public static SteamHttpResponse<TValue> Success<TValue>(this SteamHttpResponse<TValue> steamHttpResponse,
                                                                        Action<TValue> callback)
        {
            return steamHttpResponse.Success(r => callback(r.Value));
        }

        public static TSteamHttpResponse Always<TSteamHttpResponse>(this TSteamHttpResponse steamHttpResponse,
                                                                        Action<TSteamHttpResponse> callback) 
                                                                        where TSteamHttpResponse : SteamHttpResponse
        {
            callback(steamHttpResponse);
            return steamHttpResponse;
        }
    }
}