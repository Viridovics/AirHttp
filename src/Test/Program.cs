using System;
using System.Net;
using SteamHttp;
using SteamHttp.Configuration;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var s = new SteamHttpClient(new DefaultSteamHttpConfiguration());
            var o = s.Get<Obj>(@"http://localhost:52870/api/Test/7");
            System.Console.WriteLine(o.Value.Id);
            Console.WriteLine("Hello World!");
        }

        public class Obj 
        {
            public int Id { get; set; }
        }
    }
}
