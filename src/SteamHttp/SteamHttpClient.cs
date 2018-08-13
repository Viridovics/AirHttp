using Newtonsoft.Json;

namespace SteamHttp
{
    public class SteamHttpClient
    {
        public SteamHttpClient()
        {
            System.Console.WriteLine("Create object");
        }

        public T Get<T>(string content)
        {
            return JsonConvert.DeserializeObject<T>(content);
        }
    }
}