using System;
using System.Net;
using System.Text;
using AirHttp.Responses.Interfaces;

namespace AirHttp.Configuration
{
    public class RetryPolicy : IRetryPolicy
    {
        public Predicate<IAirHttpResponse> GoodCondition { get; set; }

        public Predicate<IAirHttpResponse> BadCondition { get; set; }

        public uint AttemptsCount { get; set; } = 3;

        public TimeSpan RetryTimeout  { get; set; } = TimeSpan.FromSeconds(1);
    }
}