using System;
using System.Net;
using SteamHttp.Client;
using SteamHttp.Configuration;
using SteamHttp.NewtonsoftJson;
using SteamHttp.NewtonsoftJson.Configuration;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            //var s = new SteamHttpClient(new NewtonsoftJsonSteamHttpContentConfiguration());
            var s = new SteamHttpNewtonsoftJsonClient();
            var o = s.Get<Obj>(@"http://localhost:52870/api/Test/7");
            System.Console.WriteLine(o.Value.Id);
            var o2 = s.Post<Obj, Obj>(@"http://localhost:52870/api/Test/", o.Value);
            System.Console.WriteLine(o2.Value.Id);
            Console.WriteLine("Hello World!");
        }

        public class Obj 
        {
            public int Id { get; set; }
        }
    }
}
