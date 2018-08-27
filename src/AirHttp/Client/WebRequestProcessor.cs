using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AirHttp.Configuration;

namespace AirHttp.Client
{
    internal sealed class WebRequestProcessor : IWebRequestProcessor
    {
        private const int _defaultTimeoutLag = 5000;
        public async Task<Tuple<HttpWebResponse, string>> Process(HttpWebRequest httpWebRequest, Lazy<string> body, Encoding encoding, CancellationToken cancellationToken)
        {
            if (body != null)
            {
                using (var requestStream = await httpWebRequest.GetRequestStreamAsync().ConfigureAwait(false))
                {
                    var bodyBytes = encoding.GetBytes(body.Value);
                    await requestStream.WriteAsync(bodyBytes, 0, bodyBytes.Length, cancellationToken).ConfigureAwait(false);
                    await requestStream.FlushAsync(cancellationToken).ConfigureAwait(false);
                }
            }
            var requestState = new RequestState
            {
                Request = httpWebRequest,
                Encoding = encoding
            };

            var result = httpWebRequest.BeginGetResponse(new AsyncCallback(ResponseCallback), requestState);

            var waitSuccess = await WaitOneAsync(requestState.Done,
                                                    httpWebRequest.Timeout == ConfigurationConstants.InfiniteTimeout ?
                                                    ConfigurationConstants.InfiniteTimeout :
                                                    httpWebRequest.Timeout + _defaultTimeoutLag,
                                                    cancellationToken).ConfigureAwait(false);
            requestState.StreamResponse?.Close();
            if (!waitSuccess)
            {
                httpWebRequest.Abort();
                throw new Exception($"WebRequest was not processed for {httpWebRequest.Timeout + _defaultTimeoutLag} milliseconds");
            }

            if (requestState.ResponseException != null)
            {
                throw requestState.ResponseException;
            }

            return new Tuple<HttpWebResponse, string>(requestState.Response, requestState.requestData.ToString());
        }
        
        private async Task<bool> WaitOneAsync(WaitHandle handle, int millisecondsTimeout, CancellationToken cancellationToken)
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
                return await tcs.Task.ConfigureAwait(false);
            }
            finally
            {
                registeredHandle?.Unregister(null);
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
                    requestState.requestData.Append(requestState.Encoding.GetString(requestState.BufferRead, 0, read));
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
            public Encoding Encoding { get; set; }
        }
    }
}