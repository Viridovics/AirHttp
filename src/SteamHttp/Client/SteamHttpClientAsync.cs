/*using System;
using System.IO;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using SteamHttp.Configuration;
using SteamHttp.Protocols;
using SteamHttp.Responses;

namespace SteamHttp.Client
{
    public class SteamHttpClientAsync
    {
        private ISteamHttpContentConfiguration _configuration;
        public SteamHttpClientAsync(ISteamHttpContentConfiguration configuration)
        {
            _configuration = configuration;
        }

        public SteamHttpResponse<TResult> Get<TResult>(string url)
        {
            return QueryUrl<TResult>(url, HttpMethods.GetMethod);
        }

        public SteamHttpResponse<TResult> Post<TPostBody, TResult>(string url, TPostBody obj)
        {
            return QueryUrl<TResult>(url, HttpMethods.PostMethod, _configuration.SerializeObject(obj));
        }

        public SteamHttpResponse Post<TPostBody>(string url, TPostBody obj)
        {
            return QueryUrl(url, HttpMethods.PostMethod, _configuration.SerializeObject(obj));
        }

        private SteamHttpResponse<T> QueryUrl<T>(string url, string method, string body = null)
        {
            try
            {
                var (httpResponse, content) = InnerQueryUrl(url,  method, body);
                return SteamHttpResponse<T>.CreateSuccessResponseWithValue(httpResponse.StatusCode,
                                                                    _configuration.DeserializeObject<T>(content));
            }
            catch(Exception e)
            {
                return SteamHttpResponse<T>.CreateFaultedResponseWithValue(e);
            }
        }

        private SteamHttpResponse QueryUrl(string url, string method, string body = null)
        {
            try
            {
                var (httpResponse, _) = InnerQueryUrl(url,  method, body);
                return SteamHttpResponse.CreateSuccessResponse(httpResponse.StatusCode);
            }
            catch(Exception e)
            {
                return SteamHttpResponse.CreateFaultedResponse(e);
            }
        }

        private async Task<(HttpWebResponse, string)> InnerQueryUrl(string url, string method, string body = null)
        {
            HttpClient client = new HttpClient();
            client.PostAsync()
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

            var httpResponse = await httpWebRequest.GetResponseAsync();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                return (httpResponse, streamReader.ReadToEnd());
            }
        }
    }
}*/