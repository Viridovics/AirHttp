using AirHttp.Client;
using AirHttp.SystemXml.Configuration;

namespace AirHttp.SystemXml
{
    public class AirHttpSystemXmlClient : AirHttpClient
    {
        public AirHttpSystemXmlClient() : base(new SystemXmlAirHttpContentConfiguration())
        { }
    }
}