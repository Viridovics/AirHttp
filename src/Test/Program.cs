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
using AirHttp.ContentProcessors;
using AirHttp.Client.Rest;
using System.Linq;
using AirHttp.UriFluentBuilder.Extensions;

namespace Test
{
    class Program
    {
        static async Task Main(string[] args)
        {
            /*var uriWithSegments = "localhost:52870".AddHttp().AddSegment("api").AddSegment("rest");
            //uriWithSegments is 'http://localhost:52870/api/rest'
            var uriWithQuery = "localhost".AddPort(8080).AddHttps().AddWWW().AddQueryParams(new { id = 5 })
                                                                    .AddQueryParam("point", "42");
            //uriWithQuery is 'https://www.localhost:8080?id=5&point=42'
            System.Console.WriteLine(uriWithQuery);


            var uri = new Uri(@"http://localhost:52870/api/Rest");*/
            
            var restClient = new AirRestClient<int, TestObj>(@"http://localhost:52870/api/Rest", new WeakJsonContentProcessor(), 
            new HttpClientParameters
            {
                RetryPolicy = new RetryPolicy
                {
                    RetryTimeout = TimeSpan.FromSeconds(2),
                    AttemptsCount = 2
                }
            });
            var collection = restClient.Get().Value.ToList();
            /*var o = restClient.Get(1).Value;
            restClient.Post(o);
            restClient.Put(3, o);
            restClient.Delete(5);*/
            return;
            //var s = new AirHttpClient(new NewtonsoftJsonAirHttpContentConfiguration());
            //var s = new AirHttpNewtonsoftJsonClient();
            //var s = new AirHttpSystemXmlClient();
            /*var contentProcessor = new SimpleJsonContentProcessor();
            var airClientCust = new AirHttpClient(contentProcessor,
                                              new HttpClientParameters
                                              {
                                                  TimeoutInMilliseconds = 2000
                                              });
            System.Console.WriteLine("Start");
            var airClientAsync = new AirHttpClientAsync(new SimpleJsonContentProcessor());
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
            airClient.Get<string[]>("http://localhost:52870/api/values/")
                .Success(values =>
                {
                    foreach(var val in values)
                    {
                        System.Console.WriteLine(val);
                    }
                })
                .Success(resp => System.Console.WriteLine(resp.StatusCode))
                .Always(resp => System.Console.WriteLine(resp.Value));*/
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
