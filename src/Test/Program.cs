using SteamHttp.Responses.DefferedExtensions;
using SteamHttp.NewtonsoftJson;

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
            s.Post<Obj, Obj>(@"http://localhost:52870/api/Test/", o.Value)
                .Success(val => System.Console.WriteLine("val is " + val.Id));

            s.Head(@"http://localhost:52870/api/Test/14")
                .IfFaulted(e => System.Console.WriteLine(e))
                .Success(resp => System.Console.WriteLine($"Content-Length: {resp.ContentLength}"));

        }

        public class Obj
        {
            public int Id { get; set; }
        }
    }
}
