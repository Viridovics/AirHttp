using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace AirHttp.Uri
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
            if (string.IsNullOrEmpty(uri))
            {
                throw new InvalidOperationException("uri is null or empty");
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
                throw new InvalidOperationException("Segment is null or empty");
            }
            _segments.Add(segment);
            return this;
        }

        public UriBuilder AddParam(string name, string value)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new InvalidOperationException("name of parameter is null or empty");
            }
            if (string.IsNullOrEmpty(value))
            {
                throw new InvalidOperationException("value of parameter is null or empty");
            }
            _parameters.Add(new Tuple<string, string>(name, value));
            return this;
        }

        public static implicit operator string(UriBuilder builder)
        {
            return builder.ToString();
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
                var joinedParameters = string.Join("&", _parameters.Select(p => p.Item1 + "=" + HttpUtility.UrlEncode(p.Item2)));
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