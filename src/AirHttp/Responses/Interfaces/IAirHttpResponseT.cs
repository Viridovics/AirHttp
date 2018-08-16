namespace AirHttp.Responses.Interfaces
{
    public interface IAirHttpResponse<T> : IAirHttpResponse
    {
        T Value { get; }
    }
}