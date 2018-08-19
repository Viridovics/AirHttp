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
        static void Main(string[] args)
        {
            //var s = new AirHttpClient(new NewtonsoftJsonAirHttpContentConfiguration());
            //var s = new AirHttpNewtonsoftJsonClient();
            //var s = new AirHttpSystemXmlClient();
            StringBuilder requestData = new StringBuilder();
            System.Console.WriteLine(requestData.ToString());
            var airClientAsync = new AirHttpClientAsync(new NewtonsoftJsonAirContentProcessor() , new DefaultHttpClientParameters()
            {
                TimeoutInMilliseconds = 7000
            });
            var cts = new CancellationTokenSource();
            var token = cts.Token;
            var o1T = airClientAsync.Get<TestObj>(@"http://localhost:52870/api/Test/7", token);
            var t = Task.Run(() => o1T); 
            Thread.Sleep(1000);
            cts.Cancel();
            var o1 = o1T.Result;
            System.Console.WriteLine(o1T.Result.FaultException);
            //System.Console.WriteLine(o1.Value.Id);
            return;
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
