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
    public partial class AirHttpClientAsync
    {
        private CookieCollection _cookies;
        private IAirContentProcessor _contentProcessor;
        private IHttpClientParameters _parameters;
        private IWebRequestProcessor _webRequestProcessor;

        internal AirHttpClientAsync(IAirContentProcessor contentProcessor, IHttpClientParameters parameters, IWebRequestProcessor webRequestProcessor)
        {
            _contentProcessor = contentProcessor;
            _parameters = parameters;
            _webRequestProcessor = webRequestProcessor;
        }

        internal async Task<IAirHttpResponse<T>> QueryUrl<T>(string url, string method, Lazy<string> body, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                var responseContent = await InnerQueryUrl(url, method, body, cancellationToken).ConfigureAwait(false);
                return AirHttpResponse<T>.CreateSuccessfulResponseWithValue(responseContent.Item1,
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
                return AirHttpResponse.CreateSuccessfulResponse(responseContent.Item1);
            }
            catch (Exception e)
            {
                return AirHttpResponse.CreateFaultedResponse(e);
            }
        }

        private async Task<Tuple<HttpWebResponse, string>> InnerQueryUrl(string url, string method, Lazy<string> body, CancellationToken cancellationToken)
        {
            var httpWebRequest = CreateWebRequest(url, method);
            FillCookie(httpWebRequest);
            var responseContent = await _webRequestProcessor.Process(httpWebRequest, body, _parameters.Encoding, cancellationToken).ConfigureAwait(false);
            SaveCookie(responseContent.Item1);
            return responseContent;
        }

        private HttpWebRequest CreateWebRequest(string url, string method)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.ContentType = _contentProcessor.ContentType;
            httpWebRequest.Method = method;
            httpWebRequest.Timeout = _parameters.TimeoutInMilliseconds;
            if(_parameters.Proxy != null)
            {
                httpWebRequest.Proxy = _parameters.Proxy;
            }
            return httpWebRequest;
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