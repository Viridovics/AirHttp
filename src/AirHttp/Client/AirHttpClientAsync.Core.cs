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

        internal Task<IAirHttpResponse<T>> QueryUrl<T>(string url, string method, Lazy<string> body, CancellationToken cancellationToken = default(CancellationToken))
        {
            return QueryUrl(url, method, body,
                            responseContent => AirHttpResponse<T>.CreateSuccessfulResponseWithValue(responseContent.Item1,
                                                                        _contentProcessor.DeserializeObject<T>(responseContent.Item2)),
                            e => AirHttpResponse<T>.CreateFaultedResponseWithValue(e), cancellationToken);
        }

        internal Task<IAirHttpResponse> QueryUrl(string url, string method, Lazy<string> body, CancellationToken cancellationToken = default(CancellationToken))
        {
            return QueryUrl(url, method, body,
                responseContent => AirHttpResponse.CreateSuccessfulResponse(responseContent.Item1),
                e => AirHttpResponse.CreateFaultedResponse(e), cancellationToken);
        }

        internal async Task<TResponse> QueryUrl<TResponse>(string url, string method, Lazy<string> body,
                                                          Func<Tuple<HttpWebResponse, string>, TResponse> createSuccessResponse,
                                                          Func<Exception, TResponse> createFaultedResponse,
                                                          CancellationToken cancellationToken = default(CancellationToken)) where TResponse : IAirHttpResponse
        {
            uint attempt = 0;
            while (true)
            {
                attempt++;
                TResponse result;
                try
                {
                    var responseContent = await InnerQueryUrl(url, method, body, cancellationToken).ConfigureAwait(false);
                    result = createSuccessResponse(responseContent);
                }
                catch (Exception e)
                {
                    result = createFaultedResponse(e);
                }
                var retryPolicyVerdict = GetRetryPolicyVerdict(attempt, result);
                if (retryPolicyVerdict == RetryPolicyVerdict.Continue)
                {
                    await Task.Delay(_parameters.RetryPolicy.RetryTimeout);
                    continue;
                }
                return result;
            }
        }

        private enum RetryPolicyVerdict { Return, Continue }
        private RetryPolicyVerdict GetRetryPolicyVerdict(uint attempt, IAirHttpResponse response)
        {
            if (_parameters.RetryPolicy == null)
            {
                return RetryPolicyVerdict.Return;
            }
            var retryPolicy = _parameters.RetryPolicy;
            if (attempt >= retryPolicy.AttemptsCount)
            {
                return RetryPolicyVerdict.Return;
            }
            try
            {
                if (retryPolicy.GoodCondition != null && retryPolicy.GoodCondition(response))
                {
                    return RetryPolicyVerdict.Return;
                }
                if (retryPolicy.BadCondition != null && retryPolicy.BadCondition(response))
                {
                    return RetryPolicyVerdict.Continue;
                }
                if (response.Failed)
                {
                    return RetryPolicyVerdict.Continue;
                }
            }
            catch (Exception)
            {
                return RetryPolicyVerdict.Continue;
            }
            return RetryPolicyVerdict.Return;
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
            if (_parameters.Proxy != null)
            {
                httpWebRequest.Proxy = _parameters.Proxy;
            }
            if (_parameters.ConfigureRequest != null)
            {
                _parameters.ConfigureRequest(httpWebRequest);
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