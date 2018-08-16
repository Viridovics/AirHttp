using AirHttp.Client;
using AirHttp.NewtonsoftJson.Configuration;

namespace AirHttp.NewtonsoftJson
{
    public class AirHttpNewtonsoftJsonClient : AirHttpClient
    {
        public AirHttpNewtonsoftJsonClient() : base(new NewtonsoftJsonAirHttpContentConfiguration())
        { }
    }
}