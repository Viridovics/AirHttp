using System;
using System.IO;
using System.Net;
using AirHttp.Configuration;
using AirHttp.Protocols;
using AirHttp.Responses;

namespace AirHttp.Client
{
    public class AirHttpClient
    {
        private IAirHttpContentConfiguration _configuration;
        public AirHttpClient(IAirHttpContentConfiguration configuration)
        {
            _configuration = configuration;
        }

        public AirHttpResponse<TResult> Get<TResult>(string url)
        {
            return QueryUrl<TResult>(url, HttpMethods.Get);
        }

        public AirHttpResponse<TResult> Post<TPostBody, TResult>(string url, TPostBody obj)
        {
            return QueryUrl<TResult>(url, HttpMethods.Post, _configuration.SerializeObject(obj));
        }

        public AirHttpResponse Post<TPostBody>(string url, TPostBody obj)
        {
            return QueryUrl(url, HttpMethods.Post, _configuration.SerializeObject(obj));
        }

        public AirHttpResponse Head(string url)
        {
            return QueryUrl(url, HttpMethods.Head);
        }

        private AirHttpResponse<T> QueryUrl<T>(string url, string method, string body = null)
        {
            try
            {
                var (httpResponse, content) = InnerQueryUrl(url, method, body);
                return AirHttpResponse<T>.CreateSuccessResponseWithValue(httpResponse,
                                                                    _configuration.DeserializeObject<T>(content));
            }
            catch (Exception e)
            {
                return AirHttpResponse<T>.CreateFaultedResponseWithValue(e);
            }
        }

        private AirHttpResponse QueryUrl(string url, string method, string body = null)
        {
            try
            {
                var (httpResponse, _) = InnerQueryUrl(url, method, body);
                return AirHttpResponse.CreateSuccessResponse(httpResponse);
            }
            catch (Exception e)
            {
                return AirHttpResponse.CreateFaultedResponse(e);
            }
        }

        private (HttpWebResponse, string) InnerQueryUrl(string url, string method, string body = null)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.ContentType = _configuration.ContentType;
            httpWebRequest.Method = method;

            if (body != null)
            {
                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    streamWriter.Write(body);
                    streamWriter.Flush();
                    streamWriter.Close();
                }
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                return (httpResponse, streamReader.ReadToEnd());
            }
        }
    }
}