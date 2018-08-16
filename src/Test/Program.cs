using AirHttp.Responses.DefferedExtensions;
using AirHttp.NewtonsoftJson;
using AirHttp.SystemXml;
using System;
using System.Xml.Serialization;
using Contracts;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            //var s = new AirHttpClient(new NewtonsoftJsonAirHttpContentConfiguration());
            var s = new AirHttpNewtonsoftJsonClient();
            //var s = new AirHttpSystemXmlClient();
            var o = s.Get<TestObj>(@"http://localhost:52870/api/Test/7");
            System.Console.WriteLine(o.Value.Id);
            o = s.Get<TestObj>(@"http://localhost:52870/api/Test/7");
            System.Console.WriteLine(o.Value.Id);
            o = s.Get<TestObj>(@"http://localhost:52870/api/Test/7");
            System.Console.WriteLine(o.Value.Id);
            o = s.Get<TestObj>(@"http://localhost:52870/api/Test/7");
            System.Console.WriteLine(o.Value.Id);
            o = s.Get<TestObj>(@"http://localhost:52870/api/Test/7");
            System.Console.WriteLine(o.Value.Id);
            s.Post<TestObj, TestObj>(@"http://localhost:52870/api/Test/", o.Value)
                .Fail(e => System.Console.WriteLine(e))
                .Success(val => System.Console.WriteLine("val is " + val.Id));

            s.Head(@"http://localhost:52870/api/Test/14")
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
