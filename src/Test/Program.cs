using AirHttp.Responses.DefferedExtensions;
using AirHttp.NewtonsoftJson.Configuration;
using AirHttp.SystemXml;
using AirHttp.Client;
using System;
using System.Xml.Serialization;
using Contracts;
using System.Threading.Tasks;
using AirHttp.Configuration;
using System.Text;
using System.Threading;

namespace Test
{
    class Program
    {
        static async Task Main(string[] args)
        {
            //var s = new AirHttpClient(new NewtonsoftJsonAirHttpContentConfiguration());
            //var s = new AirHttpNewtonsoftJsonClient();
            //var s = new AirHttpSystemXmlClient();
            System.Console.WriteLine("Start");
            var airClientAsync = new AirHttpClientAsync(new NewtonsoftJsonAirContentProcessor());
            var cts = new CancellationTokenSource();
            var token = cts.Token;
            var oT =  await airClientAsync.Get<TestObj>(@"http://localhost:52870/api/Test/7", token);
            System.Console.WriteLine(oT.Value.Id);
            System.Console.WriteLine(oT.Value.Name);
            oT = await airClientAsync.Get<TestObj>(@"http://localhost:52870/api/Test/7");
            System.Console.WriteLine(oT.Value.Id);
            oT = await airClientAsync.Get<TestObj>(@"http://localhost:52870/api/Test/7");
            System.Console.WriteLine(oT.Value.Id);
            oT = await airClientAsync.Get<TestObj>(@"http://localhost:52870/api/Test/7");
            System.Console.WriteLine(oT.Value.Id);
            oT = await airClientAsync.Get<TestObj>(@"http://localhost:52870/api/Test/7");
            System.Console.WriteLine(oT.Value.Id);
            (await airClientAsync.Post<TestObj, TestObj>(@"http://localhost:52870/api/Test/", oT.Value))
                .Fail(e => System.Console.WriteLine(e))
                .Success(val => System.Console.WriteLine("val is " + val.Id));

            (await airClientAsync.Head(@"http://localhost:52870/api/Test/14"))
                .Fail(e => System.Console.WriteLine(e))
                .Success(resp => System.Console.WriteLine($"Content-Length: {resp.ContentLength}"));
            //System.Console.WriteLine(o1.Value.Id);
            var airClient = new AirHttpClient(new NewtonsoftJsonAirContentProcessor());
            var o = airClient.Get<TestObj>(@"http://localhost:52870/api/Test/7");

            System.Console.WriteLine(o.Value.Id);
            o = airClient.Get<TestObj>(@"http://localhost:52870/api/Test/7");
            System.Console.WriteLine(o.Value.Id);
            o = airClient.Get<TestObj>(@"http://localhost:52870/api/Test/7");
            System.Console.WriteLine(o.Value.Id);
            o = airClient.Get<TestObj>(@"http://localhost:52870/api/Test/7");
            System.Console.WriteLine(o.Value.Id);
            o = airClient.Get<TestObj>(@"http://localhost:52870/api/Test/7");
            System.Console.WriteLine(o.Value.Id);
            airClient.Post<TestObj, TestObj>(@"http://localhost:52870/api/Test/", o.Value)
                .Fail(e => System.Console.WriteLine(e))
                .Success(val => System.Console.WriteLine("val is " + val.Id));

            airClient.Head(@"http://localhost:52870/api/Test/14")
                .Fail(e => System.Console.WriteLine(e))
                .Success(resp => System.Console.WriteLine($"Content-Length: {resp.ContentLength}"));

            o = airClient.Get<TestObj>(@"http://localhost:52870/api/Test/7");
            System.Console.WriteLine(o.Value.Id);
            o = await airClient.GetAsync<TestObj>(@"http://localhost:52870/api/Test/7");
            System.Console.WriteLine(o.Value.Id);
            o = await airClient.GetAsync<TestObj>(@"http://localhost:52870/api/Test/7");
            System.Console.WriteLine(o.Value.Id);
            o = await airClient.GetAsync<TestObj>(@"http://localhost:52870/api/Test/7");
            System.Console.WriteLine(o.Value.Id);
            (await airClient.PostAsync<TestObj, TestObj>(@"http://localhost:52870/api/Test/", o.Value))
                .Fail(e => System.Console.WriteLine(e))
                .Success(val => System.Console.WriteLine("val is " + val.Id));

            (await airClient.HeadAsync(@"http://localhost:52870/api/Test/14"))
                .Fail(e => System.Console.WriteLine(e))
                .Success(resp => System.Console.WriteLine($"Content-Length: {resp.ContentLength}"));

        }
    }

    /*   [Serializable]
       [XmlRoot("TestController.TestObj", Namespace="http://schemas.datacontract.org/2004/07/TestJson.Controllers")]
       public class TestObj
       {
           [XmlElement("Id")]
           public int Id { get; set; }

           [XmlElement("Name")]
           public string Name { get; set; }
       }*/
}
