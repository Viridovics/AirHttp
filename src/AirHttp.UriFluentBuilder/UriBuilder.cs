using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace AirHttp.UriFluentBuilder
{
    public class UriBuilder
    {
        private string _protocol;
        private string _www;
        private string _main;

        private List<string> _segments = new List<string>();

        private List<Tuple<string, string>> _parameters = new List<Tuple<string, string>>();

        public UriBuilder() { }

        public UriBuilder(string uri)
        {
            if (uri == null)
            {
                throw new ArgumentNullException(nameof(uri));
            }
            _main = uri;
        }

        public override string ToString()
        {
            return ConstructUri();
        }

        public UriBuilder AddHttp()
        {
            _protocol = "http://";
            return this;
        }

        public UriBuilder AddHttps()
        {
            _protocol = "https://";
            return this;
        }

        public UriBuilder AddWWW()
        {
            _www = "www.";
            return this;
        }

        public UriBuilder AddSegment(string segment)
        {
            if (string.IsNullOrEmpty(segment))
            {
                throw new ArgumentException("Parameter is null or empty", nameof(segment));
            }
            _segments.Add(segment);
            return this;
        }

        public UriBuilder AddQueryParam(string name, object value)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("Parameter is null or empty", nameof(name));
            }
            string serializedValue;
            if (value == null)
            {
                serializedValue = null;
            }
            else
            {
                serializedValue = value.ToString();
            }
            _parameters.Add(new Tuple<string, string>(name, serializedValue));
            return this;
        }

        public UriBuilder AddQueryParams<T>(IDictionary<string, T> parameters)
        {
            if(parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }
            foreach (var parameter in parameters)
            {
                AddQueryParam(parameter.Key, parameter.Value);
            }
            return this;
        }

        public UriBuilder AddQueryParams(object objectWithProperties)
        {
            if(objectWithProperties == null)
            {
                throw new ArgumentNullException(nameof(objectWithProperties));
            }
            var type = objectWithProperties.GetType();
            var properties = type.GetProperties();
            foreach (var property in properties)
            {
                object value = property.GetValue(objectWithProperties, new object[]{});
                AddQueryParam(property.Name, value);
            }
            return this;
        }

        public UriBuilder AddQueryParams<T>(string name, IEnumerable<T> values)
        {
            if(values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }
            foreach(var val in values)
            {
                AddQueryParam(name, val);
            }
            return this;
        }

        public static implicit operator string(UriBuilder builder)
        {
            return builder.ToString();
        }

        public static implicit operator Uri(UriBuilder builder)
        {
            return new Uri(builder.ToString());
        }

        private string ConstructUri()
        {
            var result = new StringBuilder();
            AddPartToResult(result, _protocol);
            AddPartToResult(result, _www);
            AddPartToResult(result, _main, _segments.Any());
            if(_segments.Any())
            {
                if(!string.IsNullOrEmpty(_main))
                {
                    result.Append("/");
                }
                var joinedSegments = string.Join("/", _segments);
                AddPartToResult(result, joinedSegments);
            }
            if (_parameters.Any())
            {
                AddPartToResult(result, "?");
                var joinedParameters = string.Join("&", _parameters.Select(p => string.IsNullOrEmpty(p.Item2) ? p.Item1 : p.Item1 + "=" + WebUtility.UrlEncode(p.Item2)));
                AddPartToResult(result, joinedParameters);
            }
            return result.ToString();
        }

        private void AddPartToResult(StringBuilder result, string part, bool withTrim = false)
        {
            if (!string.IsNullOrEmpty(part))
            {
                if (withTrim)
                {
                    result.Append(part.TrimEnd(new[] { '\\', '/' }));
                }
                else
                {
                    result.Append(part);
                }
            }
        }
    }
}