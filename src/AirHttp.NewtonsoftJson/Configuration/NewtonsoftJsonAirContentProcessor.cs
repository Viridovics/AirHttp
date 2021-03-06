using Newtonsoft.Json;
using AirHttp.Configuration;
using AirHttp.Protocols;

namespace AirHttp.NewtonsoftJson.Configuration
{
    public class NewtonsoftJsonAirContentProcessor : IAirContentProcessor
    {
        public string ContentType => ContentTypes.Json;

        public T DeserializeObject<T>(string serializedObject)
        {
            return JsonConvert.DeserializeObject<T>(serializedObject);
        }

        public string SerializeObject<T>(T pocoObject)
        {
            return JsonConvert.SerializeObject(pocoObject);
        }
    }
}
