/*using System;
using System.IO;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using AirHttp.Configuration;
using AirHttp.Protocols;
using AirHttp.Responses;

namespace AirHttp.Client
{
    public class AirHttpClientAsync
    {
        private IAirHttpContentConfiguration _configuration;
        public AirHttpClientAsync(IAirHttpContentConfiguration configuration)
        {
            _configuration = configuration;
        }

        public AirHttpResponse<TResult> Get<TResult>(string url)
        {
            return QueryUrl<TResult>(url, HttpMethods.GetMethod);
        }

        public AirHttpResponse<TResult> Post<TPostBody, TResult>(string url, TPostBody obj)
        {
            return QueryUrl<TResult>(url, HttpMethods.PostMethod, _configuration.SerializeObject(obj));
        }

        public AirHttpResponse Post<TPostBody>(string url, TPostBody obj)
        {
            return QueryUrl(url, HttpMethods.PostMethod, _configuration.SerializeObject(obj));
        }

        private AirHttpResponse<T> QueryUrl<T>(string url, string method, string body = null)
        {
            try
            {
                var (httpResponse, content) = InnerQueryUrl(url,  method, body);
                return AirHttpResponse<T>.CreateSuccessResponseWithValue(httpResponse.StatusCode,
                                                                    _configuration.DeserializeObject<T>(content));
            }
            catch(Exception e)
            {
                return AirHttpResponse<T>.CreateFaultedResponseWithValue(e);
            }
        }

        private AirHttpResponse QueryUrl(string url, string method, string body = null)
        {
            try
            {
                var (httpResponse, _) = InnerQueryUrl(url,  method, body);
                return AirHttpResponse.CreateSuccessResponse(httpResponse.StatusCode);
            }
            catch(Exception e)
            {
                return AirHttpResponse.CreateFaultedResponse(e);
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