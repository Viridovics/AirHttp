using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AirHttp.Configuration;
using AirHttp.Protocols;
using AirHttp.Responses;
using AirHttp.Responses.Interfaces;

namespace AirHttp.Client
{
    public class AirHttpClientAsync
    {
        private CookieCollection _cookies;
        private IAirContentProcessor _configuration;
        private IHttpClientParameters _parameters;

        private WebRequestProcessor _webRequestProcessor;

        public AirHttpClientAsync(IAirContentProcessor configuration) : this(configuration, new DefaultHttpClientParameters())
        { }

        public AirHttpClientAsync(IAirContentProcessor configuration, IHttpClientParameters parameters) : this(configuration, parameters, new WebRequestProcessor())
        { }

        internal AirHttpClientAsync(IAirContentProcessor configuration, IHttpClientParameters parameters, WebRequestProcessor webRequestProcessor)
        {
            _configuration = configuration;
            _parameters = parameters;
            _webRequestProcessor = webRequestProcessor;
        }

        public async Task<IAirHttpResponse<TResult>> Get<TResult>(string url, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await QueryUrl<TResult>(url, HttpMethods.Get, null, cancellationToken);
        }
        
        public async Task<IAirHttpResponse<TResult>> Post<TPostBody, TResult>(string url, TPostBody obj, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await QueryUrl<TResult>(url, HttpMethods.Post, new Lazy<string>(() => _configuration.SerializeObject(obj)), cancellationToken);
        }

        public async Task<IAirHttpResponse> Post<TPostBody>(string url, TPostBody obj, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await QueryUrl(url, HttpMethods.Post, new Lazy<string>(() => _configuration.SerializeObject(obj)), cancellationToken);
        }

        public async Task<IAirHttpResponse<TResult>> Put<TPostBody, TResult>(string url, TPostBody obj, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await  QueryUrl<TResult>(url, HttpMethods.Put, new Lazy<string>(() => _configuration.SerializeObject(obj)), cancellationToken);
        }

        public async Task<IAirHttpResponse> Put<TPostBody>(string url, TPostBody obj, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await QueryUrl(url, HttpMethods.Put, new Lazy<string>(() => _configuration.SerializeObject(obj)), cancellationToken);
        }

        
        public async Task<IAirHttpResponse<TResult>> Patch<TPostBody, TResult>(string url, TPostBody obj, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await QueryUrl<TResult>(url, HttpMethods.Patch, new Lazy<string>(() => _configuration.SerializeObject(obj)), cancellationToken);
        }

        public async Task<IAirHttpResponse> Patch<TPostBody>(string url, TPostBody obj, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await QueryUrl(url, HttpMethods.Patch, new Lazy<string>(() => _configuration.SerializeObject(obj)), cancellationToken);
        }

        public async Task<IAirHttpResponse> Head(string url, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await QueryUrl(url, HttpMethods.Head, null, cancellationToken);
        }

        public async Task<IAirHttpResponse> Delete(string url, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await QueryUrl(url, HttpMethods.Delete, null, cancellationToken);
        }

        private async Task<IAirHttpResponse<T>> QueryUrl<T>(string url, string method, Lazy<string> body, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                var responseContent = await InnerQueryUrl(url, method, body, cancellationToken);
                return AirHttpResponse<T>.CreateSuccessResponseWithValue(responseContent.Item1,
                                                                    _configuration.DeserializeObject<T>(responseContent.Item2));
            }
            catch (Exception e)
            {
                return AirHttpResponse<T>.CreateFaultedResponseWithValue(e);
            }
        }

        private async Task<IAirHttpResponse> QueryUrl(string url, string method, Lazy<string> body, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                var responseContent = await InnerQueryUrl(url, method, body, cancellationToken);
                return AirHttpResponse.CreateSuccessResponse(responseContent.Item1);
            }
            catch (Exception e)
            {
                return AirHttpResponse.CreateFaultedResponse(e);
            }
        }

        private async Task<Tuple<HttpWebResponse, string>> InnerQueryUrl(string url, string method, Lazy<string> body, CancellationToken cancellationToken)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.ContentType = _configuration.ContentType;
            httpWebRequest.Method = method;
            httpWebRequest.Timeout = _parameters.TimeoutInMilliseconds;
            FillCookie(httpWebRequest);
            var responseContent = await _webRequestProcessor.Process(httpWebRequest, body, cancellationToken);
            SaveCookie(responseContent.Item1);
            return responseContent;
        }

        private void FillCookie(HttpWebRequest request)
        {
            if (_cookies != null && _parameters.SaveCookie)
            {
                request.CookieContainer = new CookieContainer();
                foreach (Cookie cookie in _cookies)
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