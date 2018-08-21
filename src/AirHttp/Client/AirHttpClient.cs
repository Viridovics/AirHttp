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

        public AirHttpClient(IAirContentProcessor contentProcessor) : this(contentProcessor, new HttpClientParameters())
        { }

        public AirHttpClient(IAirContentProcessor contentProcessor, IHttpClientParameters parameters) : this(contentProcessor, parameters, new WebRequestProcessor())
        { }

        internal AirHttpClient(IAirContentProcessor contentProcessor, IHttpClientParameters parameters, IWebRequestProcessor webRequestProcessor)
        { 
            _airHttpClientAsync = new AirHttpClientAsync(contentProcessor, parameters, webRequestProcessor);
        }

        #region SyncMethods

        public IAirHttpResponse Get(string url)
        {
            return GetAsync(url).Result;
        }

        public IAirHttpResponse<TResult> Get<TResult>(string url)
        {
            return GetAsync<TResult>(url).Result;
        }

        public IAirHttpResponse Post(string url)
        {
            return PostAsync(url).Result;
        }

        public IAirHttpResponse<TResult> Post<TBody, TResult>(string url, TBody obj)
        {
            return PostAsync<TBody, TResult>(url, obj).Result;
        }

        public IAirHttpResponse Post<TBody>(string url, TBody obj)
        {
            return PostAsync<TBody>(url, obj).Result;
        }

        public IAirHttpResponse Put(string url)
        {
            return PutAsync(url).Result;
        }

        public IAirHttpResponse<TResult> Put<TBody, TResult>(string url, TBody obj)
        {
            return PutAsync<TBody, TResult>(url, obj).Result;
        }

        public IAirHttpResponse Put<TBody>(string url, TBody obj)
        {
            return PutAsync<TBody>(url, obj).Result;
        }
        
        public IAirHttpResponse Patch(string url)
        {
            return PatchAsync(url).Result;
        }

        public IAirHttpResponse<TResult> Patch<TBody, TResult>(string url, TBody obj)
        {
            return PatchAsync<TBody, TResult>(url, obj).Result;
        }

        public IAirHttpResponse Patch<TBody>(string url, TBody obj)
        {
            return PatchAsync<TBody>(url, obj).Result;
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
        public async Task<IAirHttpResponse> GetAsync(string url, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await _airHttpClientAsync.Get(url, cancellationToken).ConfigureAwait(false);
        }

        public async Task<IAirHttpResponse<TResult>> GetAsync<TResult>(string url, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await _airHttpClientAsync.Get<TResult>(url, cancellationToken).ConfigureAwait(false);
        }

        public async Task<IAirHttpResponse> PostAsync(string url, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await _airHttpClientAsync.Post(url, cancellationToken).ConfigureAwait(false);
        }

        public async Task<IAirHttpResponse<TResult>> PostAsync<TBody, TResult>(string url, TBody obj, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await _airHttpClientAsync.Post<TBody, TResult>(url, obj, cancellationToken).ConfigureAwait(false);
        }

        public async Task<IAirHttpResponse> PostAsync<TBody>(string url, TBody obj, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await _airHttpClientAsync.Post<TBody>(url, obj, cancellationToken).ConfigureAwait(false);
        }

        public async Task<IAirHttpResponse> PutAsync(string url, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await _airHttpClientAsync.Put(url, cancellationToken).ConfigureAwait(false);
        }

        public async Task<IAirHttpResponse<TResult>> PutAsync<TBody, TResult>(string url, TBody obj, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await _airHttpClientAsync.Put<TBody, TResult>(url, obj, cancellationToken).ConfigureAwait(false);
        }

        public async Task<IAirHttpResponse> PutAsync<TBody>(string url, TBody obj, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await _airHttpClientAsync.Put<TBody>(url, obj, cancellationToken).ConfigureAwait(false);
        }

        public async Task<IAirHttpResponse> PatchAsync(string url, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await _airHttpClientAsync.Patch(url, cancellationToken).ConfigureAwait(false);
        }

        public async Task<IAirHttpResponse<TResult>> PatchAsync<TBody, TResult>(string url, TBody obj, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await _airHttpClientAsync.Patch<TBody, TResult>(url, obj, cancellationToken).ConfigureAwait(false);
        }

        public async Task<IAirHttpResponse> PatchAsync<TBody>(string url, TBody obj, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await _airHttpClientAsync.Patch<TBody>(url, obj, cancellationToken).ConfigureAwait(false);
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