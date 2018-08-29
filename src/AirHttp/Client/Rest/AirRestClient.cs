using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AirHttp.Configuration;
using AirHttp.Protocols;
using AirHttp.Responses.Interfaces;
using AirHttp.UriFluentBuilder.Extensions;

namespace AirHttp.Client.Rest
{
    public class AirRestClient<TKey, TValue>
    {
        private readonly string _endpointUrl;
        private AirHttpClient _airHttpClient;

        public AirRestClient(string endpointUrl, IAirContentProcessor contentProcessor) : this(endpointUrl, contentProcessor, new HttpClientParameters())
        { }

        public AirRestClient(string endpointUrl, IAirContentProcessor contentProcessor, IHttpClientParameters parameters) : this(endpointUrl, contentProcessor, parameters, new WebRequestProcessor())
        { }

        internal AirRestClient(string endpointUrl, IAirContentProcessor contentProcessor, IHttpClientParameters parameters, IWebRequestProcessor webRequestProcessor)
        {
            _airHttpClient = new AirHttpClient(contentProcessor, parameters, webRequestProcessor);
            this._endpointUrl = endpointUrl;
        }

        public IAirHttpResponse<IEnumerable<TValue>> Get()
        {
            return _airHttpClient.Get<IEnumerable<TValue>>(_endpointUrl);
        }

        public Task<IAirHttpResponse<IEnumerable<TValue>>> GetAsync()
        {
            return _airHttpClient.GetAsync<IEnumerable<TValue>>(_endpointUrl);
        }

        public IAirHttpResponse<TValue> Get(TKey key)
        {
            return _airHttpClient.Get<TValue>(CreateUrlWithKey(key));
        }

        public Task<IAirHttpResponse<TValue>> GetAsync(TKey key)
        {
            return _airHttpClient.GetAsync<TValue>(CreateUrlWithKey(key));
        }

        public IAirHttpResponse Post(TValue postObject)
        {
            return _airHttpClient.Post(_endpointUrl, postObject);
        }

        public Task<IAirHttpResponse> PostAsync(TValue postObject)
        {
            return _airHttpClient.PostAsync(_endpointUrl, postObject);
        }

        public IAirHttpResponse Put(TKey key, TValue putObject)
        {
            return _airHttpClient.Put(CreateUrlWithKey(key), putObject);
        }

        public Task<IAirHttpResponse> PutAsync(TKey key, TValue putObject)
        {
            return _airHttpClient.PutAsync(CreateUrlWithKey(key), putObject);
        }

        public IAirHttpResponse Delete(TKey key)
        {
            return _airHttpClient.Delete(CreateUrlWithKey(key));
        }

        public Task<IAirHttpResponse> DeleteAsync(TKey key)
        {
            return _airHttpClient.DeleteAsync(CreateUrlWithKey(key));
        }

        private string CreateUrlWithKey(TKey key)
        {
            return _endpointUrl.AddSegment(key.ToString());
        }
    }
}