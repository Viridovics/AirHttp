(*
For output
open Xunit.Abstractions

type MyTests(output:ITestOutputHelper) =
*)

module AirHttpClientTests

open System
open System.Net
open Xunit

open FsUnit.Xunit

open AirHttp.Client
open AirHttp.Configuration

type FakeContentProcessor() =
    interface IAirContentProcessor with
        member this.ContentType with get () = "json"
        member this.DeserializeObject<'t>(serializedObject) =
            Unchecked.defaultof<'t>
        member this.SerializeObject<'t>(pocoObject: 't) =
            ""

type FakeWebRequestProcessor() =
    member val PassedMethodName = "NoMethod" with get, set
    interface IWebRequestProcessor with 
        member this.Process (httpWebRequest, body, encoding, cancellationToken) = 
                async{
                    this.PassedMethodName <- httpWebRequest.Method
                    return (new HttpWebResponse(), "")
                } |> Async.StartAsTask

let createFakeWebRequestProcessor() = FakeWebRequestProcessor()

let createHttpClientParametersWithoutCookie ()= 
    let parameters = HttpClientParameters()
    parameters.SaveCookie <- false
    parameters

let createAirHttpClient(webRequestProcessor) = AirHttpClient(FakeContentProcessor(), createHttpClientParametersWithoutCookie(), webRequestProcessor)

[<Fact>]
let ``Check get, post, patch, delete, put, head method headers for empty body and content requests`` () =
    let url = @"http://localhost"
    let fakeProcessor = createFakeWebRequestProcessor()
    let airClient = createAirHttpClient(fakeProcessor)

    airClient.Get(url).Failed  |> should equal false
    fakeProcessor.PassedMethodName |> should equal "GET"

    airClient.Post(url).Failed  |> should equal false
    fakeProcessor.PassedMethodName |> should equal "POST"

    airClient.Put(url).Failed  |> should equal false
    fakeProcessor.PassedMethodName |> should equal "PUT"

    airClient.Patch(url).Failed  |> should equal false
    fakeProcessor.PassedMethodName |> should equal "PATCH"

    airClient.Head(url).Failed  |> should equal false
    fakeProcessor.PassedMethodName |> should equal "HEAD"

    airClient.Delete(url).Failed  |> should equal false
    fakeProcessor.PassedMethodName |> should equal "DELETE"

