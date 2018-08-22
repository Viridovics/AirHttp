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
        public AirHttpClientAsync(IAirContentProcessor contentProcessor) : this(contentProcessor, new HttpClientParameters())
        { }

        public AirHttpClientAsync(IAirContentProcessor contentProcessor, IHttpClientParameters parameters) : this(contentProcessor, parameters, new WebRequestProcessor())
        { }

        public void Reconfigure(IHttpClientParameters parameters)
        {
            _parameters = parameters;
        }

        public async Task<IAirHttpResponse> Get(string url, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await Exec(HttpMethods.Get, url, cancellationToken).ConfigureAwait(false);
        }

        public async Task<IAirHttpResponse<TResult>> Get<TResult>(string url, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await Exec<TResult>(HttpMethods.Get, url, cancellationToken).ConfigureAwait(false);
        }

        public async Task<IAirHttpResponse> Post(string url, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await Exec(HttpMethods.Post, url, cancellationToken).ConfigureAwait(false);
        }

        public async Task<IAirHttpResponse> Post<TBody>(string url, TBody obj, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await Exec<TBody>(HttpMethods.Post, url, obj, cancellationToken).ConfigureAwait(false);
        }

        public async Task<IAirHttpResponse<TResult>> Post<TResult>(string url, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await Exec<TResult>(HttpMethods.Post, url, cancellationToken).ConfigureAwait(false);
        }

        public async Task<IAirHttpResponse<TResult>> Post<TBody, TResult>(string url, TBody obj, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await Exec<TBody, TResult>(HttpMethods.Post, url, obj, cancellationToken).ConfigureAwait(false);
        }

        public async Task<IAirHttpResponse> Put(string url, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await Exec(HttpMethods.Put, url, cancellationToken).ConfigureAwait(false);
        }

        public async Task<IAirHttpResponse> Put<TBody>(string url, TBody obj, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await Exec<TBody>(HttpMethods.Put, url, obj, cancellationToken).ConfigureAwait(false);
        }

        public async Task<IAirHttpResponse<TResult>> Put<TResult>(string url, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await Exec<TResult>(HttpMethods.Put, url, cancellationToken).ConfigureAwait(false);
        }

        public async Task<IAirHttpResponse<TResult>> Put<TBody, TResult>(string url, TBody obj, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await Exec<TBody, TResult>(HttpMethods.Put, url, obj, cancellationToken).ConfigureAwait(false);
        }

        public async Task<IAirHttpResponse> Patch(string url, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await Exec(HttpMethods.Patch, url, cancellationToken).ConfigureAwait(false);
        }

        public async Task<IAirHttpResponse> Patch<TBody>(string url, TBody obj, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await Exec<TBody>(HttpMethods.Patch, url, obj, cancellationToken).ConfigureAwait(false);
        }

        public async Task<IAirHttpResponse<TResult>> Patch<TResult>(string url, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await Exec<TResult>(HttpMethods.Patch, url, cancellationToken).ConfigureAwait(false);
        }

        public async Task<IAirHttpResponse<TResult>> Patch<TBody, TResult>(string url, TBody obj, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await Exec<TBody, TResult>(HttpMethods.Patch, url, obj, cancellationToken).ConfigureAwait(false);
        }

        public async Task<IAirHttpResponse> Head(string url, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await Exec(HttpMethods.Head, url, cancellationToken).ConfigureAwait(false);
        }

        public async Task<IAirHttpResponse> Delete(string url, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await Exec(HttpMethods.Delete, url, cancellationToken).ConfigureAwait(false);
        }

        public async Task<IAirHttpResponse> Exec(string methodName, string url, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await QueryUrl(url, methodName, null, cancellationToken).ConfigureAwait(false);
        }

        public async Task<IAirHttpResponse> Exec<TBody>(string methodName, string url, TBody obj, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await QueryUrl(url, methodName, new Lazy<string>(() => _contentProcessor.SerializeObject(obj)), cancellationToken).ConfigureAwait(false);
        }

        public async Task<IAirHttpResponse<TResult>> Exec<TResult>(string methodName, string url, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await QueryUrl<TResult>(url, methodName, null, cancellationToken).ConfigureAwait(false);
        }

        public async Task<IAirHttpResponse<TResult>> Exec<TBody, TResult>(string methodName, string url, TBody obj, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await QueryUrl<TResult>(url, methodName, new Lazy<string>(() => _contentProcessor.SerializeObject(obj)), cancellationToken).ConfigureAwait(false);
        }
    }
}