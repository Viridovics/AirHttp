using System;
using System.Net;
using System.Threading.Tasks;
using AirHttp.Responses.Interfaces;

namespace AirHttp.Responses.DefferedExtensions
{
    public static class AsyncDefferedExtensions
    {
        public static Task<TAirHttpResponse> Fail<TAirHttpResponse>(this Task<TAirHttpResponse> airHttpResponse,
                                                                        Action<Exception> callback)
                                                                        where TAirHttpResponse : IAirHttpResponse
        {
            return airHttpResponse.ContinueWith(r =>
            {
                var response = r.Result;
                return response.Fail(callback);
            });
        }

        public static Task<TAirHttpResponse> Success<TAirHttpResponse>(this Task<TAirHttpResponse> airHttpResponse,
                                                                        Action<TAirHttpResponse> callback)
                                                                        where TAirHttpResponse : IAirHttpResponse
        {
            return airHttpResponse.ContinueWith(r =>
            {
                var response = r.Result;
                return response.Success(callback);
            });
        }

        public static Task<IAirHttpResponse<TValue>> Success<TValue>(this Task<IAirHttpResponse<TValue>> airHttpResponse,
                                                                        Action<TValue> callback)
        {
            return airHttpResponse.Success(r => callback(r.Value));
        }

        public static Task<TAirHttpResponse> Always<TAirHttpResponse>(this Task<TAirHttpResponse> airHttpResponse,
                                                                        Action<TAirHttpResponse> callback)
                                                                        where TAirHttpResponse : IAirHttpResponse
        {
            return airHttpResponse.ContinueWith(r =>
            {
                var response = r.Result;
                return response.Always(callback);
            });
        }
    }
}