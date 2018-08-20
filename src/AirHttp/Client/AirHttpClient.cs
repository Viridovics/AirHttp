using System;
using System.Threading;
using System.Threading.Tasks;
using AirHttp.Configuration;
using AirHttp.Protocols;
using AirHttp.Responses.Interfaces;

namespace AirHttp.Client
{
    public class AirHttpClient
    {
        private AirHttpClientAsync _airHttpClientAsync;

        public AirHttpClient(IAirContentProcessor configuration) : this(configuration, new HttpClientParameters())
        { }

        public AirHttpClient(IAirContentProcessor configuration, IHttpClientParameters parameters)
        { 
            _airHttpClientAsync = new AirHttpClientAsync(configuration, parameters, new WebRequestProcessor());
        }

        #region SyncMethods
        public IAirHttpResponse<TResult> Get<TResult>(string url)
        {
            return GetAsync<TResult>(url).Result;
        }

        public IAirHttpResponse<TResult> Post<TPostBody, TResult>(string url, TPostBody obj)
        {
            return PostAsync<TPostBody, TResult>(url, obj).Result;
        }

        public IAirHttpResponse Post<TPostBody>(string url, TPostBody obj)
        {
            return PostAsync<TPostBody>(url, obj).Result;
        }

        public IAirHttpResponse<TResult> Put<TPostBody, TResult>(string url, TPostBody obj)
        {
            return PutAsync<TPostBody, TResult>(url, obj).Result;
        }

        public IAirHttpResponse Put<TPostBody>(string url, TPostBody obj)
        {
            return PutAsync<TPostBody>(url, obj).Result;
        }

        public IAirHttpResponse<TResult> Patch<TPostBody, TResult>(string url, TPostBody obj)
        {
            return PatchAsync<TPostBody, TResult>(url, obj).Result;
        }

        public IAirHttpResponse Patch<TPostBody>(string url, TPostBody obj)
        {
            return PatchAsync<TPostBody>(url, obj).Result;
        }

        public IAirHttpResponse Head(string url)
        {
            return HeadAsync(url).Result;
        }

        public IAirHttpResponse Delete(string url)
        {
            return DeleteAsync(url).Result;
        }

        #endregion

        #region AsynMethods
        public async Task<IAirHttpResponse<TResult>> GetAsync<TResult>(string url, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await _airHttpClientAsync.Get<TResult>(url, cancellationToken).ConfigureAwait(false);
        }

        public async Task<IAirHttpResponse<TResult>> PostAsync<TPostBody, TResult>(string url, TPostBody obj, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await _airHttpClientAsync.Post<TPostBody, TResult>(url, obj, cancellationToken).ConfigureAwait(false);
        }

        public async Task<IAirHttpResponse> PostAsync<TPostBody>(string url, TPostBody obj, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await _airHttpClientAsync.Post<TPostBody>(url, obj, cancellationToken).ConfigureAwait(false);
        }

        public async Task<IAirHttpResponse<TResult>> PutAsync<TPostBody, TResult>(string url, TPostBody obj, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await _airHttpClientAsync.Put<TPostBody, TResult>(url, obj, cancellationToken).ConfigureAwait(false);
        }

        public async Task<IAirHttpResponse> PutAsync<TPostBody>(string url, TPostBody obj, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await _airHttpClientAsync.Put<TPostBody>(url, obj, cancellationToken).ConfigureAwait(false);
        }


        public async Task<IAirHttpResponse<TResult>> PatchAsync<TPostBody, TResult>(string url, TPostBody obj, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await _airHttpClientAsync.Patch<TPostBody, TResult>(url, obj, cancellationToken).ConfigureAwait(false);
        }

        public async Task<IAirHttpResponse> PatchAsync<TPostBody>(string url, TPostBody obj, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await _airHttpClientAsync.Patch<TPostBody>(url, obj, cancellationToken).ConfigureAwait(false);
        }

        public async Task<IAirHttpResponse> HeadAsync(string url, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await _airHttpClientAsync.Head(url, cancellationToken).ConfigureAwait(false);
        }

        public async Task<IAirHttpResponse> DeleteAsync(string url, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await _airHttpClientAsync.Delete(url, cancellationToken).ConfigureAwait(false);
        }

        #endregion
    }
}