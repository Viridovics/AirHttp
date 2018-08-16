using AirHttp.Responses.DefferedExtensions;
using AirHttp.NewtonsoftJson;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            //var s = new AirHttpClient(new NewtonsoftJsonAirHttpContentConfiguration());
            var s = new AirHttpNewtonsoftJsonClient();
            var o = s.Get<Obj>(@"http://localhost:52870/api/Test/7");
            System.Console.WriteLine(o.Value.Id);
            s.Post<Obj, Obj>(@"http://localhost:52870/api/Test/", o.Value)
                .Success(val => System.Console.WriteLine("val is " + val.Id));

            s.Head(@"http://localhost:52870/api/Test/14")
                .Fail(e => System.Console.WriteLine(e))
                .Success(resp => System.Console.WriteLine($"Content-Length: {resp.ContentLength}"));

        }

        public class Obj
        {
            public int Id { get; set; }
        }
    }
}
