using System;
using System.IO;
using System.Net;
using AirHttp.Configuration;
using AirHttp.Protocols;
using AirHttp.Responses;
using AirHttp.Responses.Interfaces;

namespace AirHttp.Client
{
    public class AirHttpClient
    {
        private CookieCollection _cookies;
        private IAirHttpContentConfiguration _configuration;
        private IHttpClientParameters _parameters;

        public AirHttpClient(IAirHttpContentConfiguration configuration) : this(configuration, new DefaultHttpClientParameters())
        { }

        public AirHttpClient(IAirHttpContentConfiguration configuration, IHttpClientParameters parameters)
        {
            _configuration = configuration;
            _parameters = parameters;
        }

        public IAirHttpResponse<TResult> Get<TResult>(string url)
        {
            return QueryUrl<TResult>(url, HttpMethods.Get);
        }

        public IAirHttpResponse<TResult> Post<TPostBody, TResult>(string url, TPostBody obj)
        {
            return QueryUrl<TResult>(url, HttpMethods.Post, new Lazy<string>(() => _configuration.SerializeObject(obj)));
        }

        public IAirHttpResponse Post<TPostBody>(string url, TPostBody obj)
        {
            return QueryUrl(url, HttpMethods.Post, new Lazy<string>(() => _configuration.SerializeObject(obj)));
        }

        public IAirHttpResponse<TResult> Put<TPostBody, TResult>(string url, TPostBody obj)
        {
            return QueryUrl<TResult>(url, HttpMethods.Put, new Lazy<string>(() => _configuration.SerializeObject(obj)));
        }

        public IAirHttpResponse Put<TPostBody>(string url, TPostBody obj)
        {
            return QueryUrl(url, HttpMethods.Put, new Lazy<string>(() => _configuration.SerializeObject(obj)));
        }

        public IAirHttpResponse Head(string url)
        {
            return QueryUrl(url, HttpMethods.Head);
        }

        public IAirHttpResponse Delete(string url)
        {
            return QueryUrl(url, HttpMethods.Delete);
        }

        private IAirHttpResponse<T> QueryUrl<T>(string url, string method, Lazy<string> body = null)
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

        private IAirHttpResponse QueryUrl(string url, string method, Lazy<string> body = null)
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

        private (HttpWebResponse, string) InnerQueryUrl(string url, string method, Lazy<string> body = null)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.ContentType = _configuration.ContentType;
            httpWebRequest.Method = method;
            httpWebRequest.Timeout = _parameters.TimeoutInMilliseconds;
            FillCookie(httpWebRequest);
            if (body != null)
            {
                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    streamWriter.Write(body.Value);
                    streamWriter.Flush();
                    streamWriter.Close();
                }
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            SaveCookie(httpResponse);
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                return (httpResponse, streamReader.ReadToEnd());
            }
        }

        private void FillCookie(HttpWebRequest request)
        {
            if (_cookies != null && _parameters.SaveCookie)
            {
                request.CookieContainer = new CookieContainer();
                foreach(Cookie cookie in _cookies)
                {
                    request.CookieContainer.Add(cookie);
                }
            }
        }

        private void SaveCookie(HttpWebResponse response)
        {
            if (_parameters.SaveCookie)
            {
                _cookies = response.Cookies;
            }
        }
    }
}