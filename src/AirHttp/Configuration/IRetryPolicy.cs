using System;
using AirHttp.Responses.Interfaces;

namespace AirHttp.Configuration
{
    public interface IRetryPolicy
    {
        Predicate<IAirHttpResponse> GoodCondition { get; }
        Predicate<IAirHttpResponse> BadCondition { get; }
        uint AttemptsCount { get; }
        TimeSpan RetryTimeout { get; }
    }
}
