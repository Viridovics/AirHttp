using System;
using System.IO;
using System.Net;
using SteamHttp.Configuration;
using SteamHttp.Protocols;
using SteamHttp.Responses;

namespace SteamHttp.Client
{
    public class SteamHttpClient
    {
        private ISteamHttpContentConfiguration _configuration;
        public SteamHttpClient(ISteamHttpContentConfiguration configuration)
        {
            _configuration = configuration;
        }

        public SteamHttpResponse<TResult> Get<TResult>(string url)
        {
            return QueryUrl<TResult>(url, HttpMethods.Get);
        }

        public SteamHttpResponse<TResult> Post<TPostBody, TResult>(string url, TPostBody obj)
        {
            return QueryUrl<TResult>(url, HttpMethods.Post, _configuration.SerializeObject(obj));
        }

        public SteamHttpResponse Post<TPostBody>(string url, TPostBody obj)
        {
            return QueryUrl(url, HttpMethods.Post, _configuration.SerializeObject(obj));
        }

        public SteamHttpResponse Head(string url)
        {
            return QueryUrl(url, HttpMethods.Head);
        }

        private SteamHttpResponse<T> QueryUrl<T>(string url, string method, string body = null)
        {
            try
            {
                var (httpResponse, content) = InnerQueryUrl(url, method, body);
                return SteamHttpResponse<T>.CreateSuccessResponseWithValue(httpResponse,
                                                                    _configuration.DeserializeObject<T>(content));
            }
            catch (Exception e)
            {
                return SteamHttpResponse<T>.CreateFaultedResponseWithValue(e);
            }
        }

        private SteamHttpResponse QueryUrl(string url, string method, string body = null)
        {
            try
            {
                var (httpResponse, _) = InnerQueryUrl(url, method, body);
                return SteamHttpResponse.CreateSuccessResponse(httpResponse);
            }
            catch (Exception e)
            {
                return SteamHttpResponse.CreateFaultedResponse(e);
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