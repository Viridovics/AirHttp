using AirHttp.Configuration;
using AirHttp.Protocols;

namespace AirHttp.ContentProcessors
{
    public class SimpleJsonContentProcessor : IAirContentProcessor
    {
        public string ContentType => ContentTypes.Json;

        public T DeserializeObject<T>(string serializedObject)
        {
            return SimpleJson.SimpleJson.DeserializeObject<T>(serializedObject);
        }

        public string SerializeObject<T>(T pocoObject)
        {
            return SimpleJson.SimpleJson.SerializeObject(pocoObject);
        }
    }
}