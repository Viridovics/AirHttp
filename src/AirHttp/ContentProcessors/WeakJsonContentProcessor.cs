using AirHttp.Configuration;
using AirHttp.Protocols;

namespace AirHttp.ContentProcessors
{
    public class WeakJsonContentProcessor : IAirContentProcessor
    {
        public string ContentType => ContentTypes.Json;

        public T DeserializeObject<T>(string serializedObject)
        {
            return SimpleJsonIgnoreCase.SimpleJson.DeserializeObject<T>(serializedObject);
        }

        public string SerializeObject<T>(T pocoObject)
        {
            return SimpleJsonIgnoreCase.SimpleJson.SerializeObject(pocoObject);
        }
    }
}