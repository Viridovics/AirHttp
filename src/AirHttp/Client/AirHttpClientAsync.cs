using System;
using System.IO;
using System.Net;
using System.Text;
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
        private const int _defaultTimeoutLag = 5000;
        private CookieCollection _cookies;
        private IAirContentProcessor _configuration;
        private IHttpClientParameters _parameters;

        public AirHttpClientAsync(IAirContentProcessor configuration) : this(configuration, new DefaultHttpClientParameters())
        { }

        public AirHttpClientAsync(IAirContentProcessor configuration, IHttpClientParameters parameters)
        {
            _configuration = configuration;
            _parameters = parameters;
        }

        public async Task<IAirHttpResponse<TResult>> Get<TResult>(string url, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await QueryUrl<TResult>(url, HttpMethods.Get, null, cancellationToken);
        }
        /*
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

        
        public IAirHttpResponse<TResult> Patch<TPostBody, TResult>(string url, TPostBody obj)
        {
            return QueryUrl<TResult>(url, HttpMethods.Patch, new Lazy<string>(() => _configuration.SerializeObject(obj)));
        }

        public IAirHttpResponse Patch<TPostBody>(string url, TPostBody obj)
        {
            return QueryUrl(url, HttpMethods.Patch, new Lazy<string>(() => _configuration.SerializeObject(obj)));
        }

        public IAirHttpResponse Head(string url)
        {
            return QueryUrl(url, HttpMethods.Head);
        }

        public IAirHttpResponse Delete(string url)
        {
            return QueryUrl(url, HttpMethods.Delete);
        }*/



        private async Task<IAirHttpResponse<T>> QueryUrl<T>(string url, string method, Lazy<string> body, CancellationToken cancellationToken)
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

        private async Task<IAirHttpResponse> QueryUrl(string url, string method, Lazy<string> body, CancellationToken cancellationToken)
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
            if (body != null)
            {
                using (var requestStream = await httpWebRequest.GetRequestStreamAsync())
                {
                    var bodyBytes = Encoding.UTF8.GetBytes(body.Value);
                    await requestStream.WriteAsync(bodyBytes, 0, bodyBytes.Length, cancellationToken);
                    await requestStream.FlushAsync();
                }
            }
            var requestState = new RequestState();
            requestState.Request = httpWebRequest;

            var result = httpWebRequest.BeginGetResponse(new AsyncCallback(ResponseCallback), requestState);

            var waitSuccess = await WaitOneAsync(requestState.Done,
                                                    httpWebRequest.Timeout == ConfigurationConstants.InfiniteTimeout ?
                                                    ConfigurationConstants.InfiniteTimeout :
                                                    httpWebRequest.Timeout + _defaultTimeoutLag,
                                                    cancellationToken);

            if (!waitSuccess)
            {
                httpWebRequest.Abort();
                throw new Exception($"WebRequest was not processed for {httpWebRequest.Timeout + _defaultTimeoutLag} milliseconds");
            }

            if (requestState.ResponseException != null)
            {
                throw requestState.ResponseException;
            }

            var httpResponse = requestState.Response;
            SaveCookie(httpResponse);
            return new Tuple<HttpWebResponse, string>(httpResponse, requestState.requestData.ToString());
        }

        private static async Task<bool> WaitOneAsync(WaitHandle handle, int millisecondsTimeout, CancellationToken cancellationToken)
        {
            RegisteredWaitHandle registeredHandle = null;
            var tokenRegistration = default(CancellationTokenRegistration);
            try
            {
                var tcs = new TaskCompletionSource<bool>();
                registeredHandle = ThreadPool.RegisterWaitForSingleObject(
                    handle,
                    (state, timedOut) => ((TaskCompletionSource<bool>)state).TrySetResult(!timedOut),
                    tcs,
                    millisecondsTimeout,
                    true);
                tokenRegistration = cancellationToken.Register(
                    state => ((TaskCompletionSource<bool>)state).TrySetCanceled(),
                    tcs);
                return await tcs.Task;
            }
            finally
            {
                if (registeredHandle != null)
                    registeredHandle.Unregister(null);
                tokenRegistration.Dispose();
            }
        }

        private void ResponseCallback(IAsyncResult asynchronousResult)
        {
            var requestState = (RequestState)asynchronousResult.AsyncState;
            try
            {
                var httpWebRequest = requestState.Request;
                requestState.Response = (HttpWebResponse)httpWebRequest.EndGetResponse(asynchronousResult);
                var responseStream = requestState.Response.GetResponseStream();
                requestState.StreamResponse = responseStream;
                var asynchronousInputRead = responseStream.BeginRead(requestState.BufferRead, 0, RequestState.BUFFER_SIZE, new AsyncCallback(ReadCallback), requestState);
                return;
            }
            catch (Exception e)
            {
                requestState.ResponseException = e;
            }
            requestState.Done.Set();
        }

        private void ReadCallback(IAsyncResult asyncResult)
        {
            var requestState = (RequestState)asyncResult.AsyncState;
            try
            {
                var responseStream = requestState.StreamResponse;
                int read = responseStream.EndRead(asyncResult);
                if (read > 0)
                {
                    requestState.requestData.Append(Encoding.ASCII.GetString(requestState.BufferRead, 0, read));
                    IAsyncResult asynchronousResult = responseStream.BeginRead(requestState.BufferRead, 0, RequestState.BUFFER_SIZE, new AsyncCallback(ReadCallback), requestState);
                    return;
                }
                else
                {
                    responseStream.Close();
                    requestState.Done.Set();
                }
            }
            catch (Exception e)
            {
                requestState.ResponseException = e;
                requestState.Done.Set();
            }
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

        private class RequestState
        {
            public const int BUFFER_SIZE = 1024;
            public StringBuilder requestData { get; set; } = new StringBuilder();
            public byte[] BufferRead { get; set; } = new byte[BUFFER_SIZE];
            public ManualResetEvent Done { get; set; } = new ManualResetEvent(false);

            public HttpWebRequest Request { get; set; }
            public HttpWebResponse Response { get; set; }
            public Exception ResponseException { get; set; }
            public Stream StreamResponse { get; set; }
        }
    }
}