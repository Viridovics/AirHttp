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
        private IAirContentProcessor _contentProcessor;
        private IHttpClientParameters _parameters;
        private IWebRequestProcessor _webRequestProcessor;

        public AirHttpClientAsync(IAirContentProcessor contentProcessor) : this(contentProcessor, new HttpClientParameters())
        { }

        public AirHttpClientAsync(IAirContentProcessor contentProcessor, IHttpClientParameters parameters) : this(contentProcessor, parameters, new WebRequestProcessor())
        { }

        internal AirHttpClientAsync(IAirContentProcessor contentProcessor, IHttpClientParameters parameters, IWebRequestProcessor webRequestProcessor)
        {
            _contentProcessor = contentProcessor;
            _parameters = parameters;
            _webRequestProcessor = webRequestProcessor;
        }

        public async Task<IAirHttpResponse> Get(string url, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await QueryUrl(url, HttpMethods.Get, null, cancellationToken).ConfigureAwait(false);
        }

        public async Task<IAirHttpResponse<TResult>> Get<TResult>(string url, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await QueryUrl<TResult>(url, HttpMethods.Get, null, cancellationToken).ConfigureAwait(false);
        }

        public async Task<IAirHttpResponse> Post(string url, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await QueryUrl(url, HttpMethods.Post, null, cancellationToken).ConfigureAwait(false);
        }

        public async Task<IAirHttpResponse<TResult>> Post<TBody, TResult>(string url, TBody obj, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await QueryUrl<TResult>(url, HttpMethods.Post, new Lazy<string>(() => _contentProcessor.SerializeObject(obj)), cancellationToken).ConfigureAwait(false);
        }

        public async Task<IAirHttpResponse> Post<TBody>(string url, TBody obj, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await QueryUrl(url, HttpMethods.Post, new Lazy<string>(() => _contentProcessor.SerializeObject(obj)), cancellationToken).ConfigureAwait(false);
        }
        public async Task<IAirHttpResponse> Put(string url, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await QueryUrl(url, HttpMethods.Put, null, cancellationToken).ConfigureAwait(false);
        }

        public async Task<IAirHttpResponse<TResult>> Put<TBody, TResult>(string url, TBody obj, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await QueryUrl<TResult>(url, HttpMethods.Put, new Lazy<string>(() => _contentProcessor.SerializeObject(obj)), cancellationToken).ConfigureAwait(false);
        }

        public async Task<IAirHttpResponse> Put<TBody>(string url, TBody obj, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await QueryUrl(url, HttpMethods.Put, new Lazy<string>(() => _contentProcessor.SerializeObject(obj)), cancellationToken).ConfigureAwait(false);
        }

        public async Task<IAirHttpResponse> Patch(string url, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await QueryUrl(url, HttpMethods.Patch, null, cancellationToken).ConfigureAwait(false);
        }

        public async Task<IAirHttpResponse<TResult>> Patch<TBody, TResult>(string url, TBody obj, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await QueryUrl<TResult>(url, HttpMethods.Patch, new Lazy<string>(() => _contentProcessor.SerializeObject(obj)), cancellationToken).ConfigureAwait(false);
        }

        public async Task<IAirHttpResponse> Patch<TBody>(string url, TBody obj, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await QueryUrl(url, HttpMethods.Patch, new Lazy<string>(() => _contentProcessor.SerializeObject(obj)), cancellationToken).ConfigureAwait(false);
        }

        public async Task<IAirHttpResponse> Head(string url, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await QueryUrl(url, HttpMethods.Head, null, cancellationToken).ConfigureAwait(false);
        }

        public async Task<IAirHttpResponse> Delete(string url, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await QueryUrl(url, HttpMethods.Delete, null, cancellationToken).ConfigureAwait(false);
        }

        internal async Task<IAirHttpResponse<T>> QueryUrl<T>(string url, string method, Lazy<string> body, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                var responseContent = await InnerQueryUrl(url, method, body, cancellationToken).ConfigureAwait(false);
                return AirHttpResponse<T>.CreateSuccessResponseWithValue(responseContent.Item1,
                                                                    _contentProcessor.DeserializeObject<T>(responseContent.Item2));
            }
            catch (Exception e)
            {
                return AirHttpResponse<T>.CreateFaultedResponseWithValue(e);
            }
        }

        internal async Task<IAirHttpResponse> QueryUrl(string url, string method, Lazy<string> body, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                var responseContent = await InnerQueryUrl(url, method, body, cancellationToken).ConfigureAwait(false);
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
            httpWebRequest.ContentType = _contentProcessor.ContentType;
            httpWebRequest.Method = method;
            httpWebRequest.Timeout = _parameters.TimeoutInMilliseconds;
            FillCookie(httpWebRequest);
            var responseContent = await _webRequestProcessor.Process(httpWebRequest, body, _parameters.Encoding, cancellationToken).ConfigureAwait(false);
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
            if (_parameters.SaveCookie && response.Cookies != null)
            {
                if (_cookies == null)
                {
                    _cookies = response.Cookies;
                }
                else
                {
                    foreach (Cookie cookie in response.Cookies)
                    {
                        _cookies.Add(cookie);
                    }
                }
            }
        }
    }
}