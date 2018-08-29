using System;
using System.Collections.Generic;

namespace AirHttp.UriFluentBuilder.Extensions
{
    public static class UriBuilderExtensions
    {
        public static UriBuilder AddHttp(this string uri)
        {
            return CreateUriBuilderWithSetup(uri, b => b.AddHttp());
        }

        public static UriBuilder AddHttps(this string uri)
        {
            return CreateUriBuilderWithSetup(uri, b => b.AddHttps());
        }

        public static UriBuilder AddWWW(this string uri)
        {
            return CreateUriBuilderWithSetup(uri, b => b.AddWWW());
        }

        public static UriBuilder AddPort(this string uri, ushort port)
        {
            return CreateUriBuilderWithSetup(uri, b => b.AddPort(port));
        }

        public static UriBuilder AddSegment(this string uri, object segment)
        {
            return CreateUriBuilderWithSetup(uri, b => b.AddSegment(segment));
        }

        public static UriBuilder AddQueryParam(this string uri, string name, object value)
        {
            return CreateUriBuilderWithSetup(uri, b => b.AddQueryParam(name, value));
        }

        public static UriBuilder AddQueryParams(this string uri, object objectWithProperties)
        {
            return CreateUriBuilderWithSetup(uri, b => b.AddQueryParams(objectWithProperties));
        }
        
        public static UriBuilder AddQueryParams<T>(this string uri, string name, IEnumerable<T> values)
        {
            return CreateUriBuilderWithSetup(uri, b => b.AddQueryParams(name, values));
        }

        public static UriBuilder AddQueryParams<T>(this string uri, IDictionary<string, T> parameters)
        {
            return CreateUriBuilderWithSetup(uri, b => b.AddQueryParams(parameters));
        }
        
        private static UriBuilder CreateUriBuilderWithSetup(string uri, Func<UriBuilder, UriBuilder> setupBuilder)
        {
            return setupBuilder(new UriBuilder(uri));
        }
    }

}