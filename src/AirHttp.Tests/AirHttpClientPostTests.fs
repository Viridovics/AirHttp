module AirHttpClientPostTests

open System.Net
open System.Text

open Xunit

open FsUnit.Xunit

open AirHttp.Client
open AirHttp.Configuration
open AirHttp.ContentProcessors
open System

type ResponseObject() = 
    member val Id = -1 with get, set
    member val Name = "" with get, set

type RequestObject(name: string) = 
    member val Name = name with get, set

type OkHttpWebResponse() =
   inherit HttpWebResponse()
   override this.StatusCode = HttpStatusCode.OK

let createOkHttpWebResponse() : HttpWebResponse =
    upcast new OkHttpWebResponse()

type FakeWebRequestProcessor(checkBody : (Lazy<string> -> unit), content: string) =
    interface IWebRequestProcessor with 
        member this.Process (httpWebRequest, body, encoding, cancellationToken) = 
                async{
                    checkBody(body)
                    encoding |> should equal Encoding.UTF8
                    return (createOkHttpWebResponse(), content)
                } |> Async.StartAsTask

let createHttpClientParametersWithoutCookie ()= 
    let parameters = new HttpClientParameters()
    parameters.SaveCookie <- false
    parameters

let createAirHttpClient(webRequestProcessor) = AirHttpClient(new WeakJsonContentProcessor(), createHttpClientParametersWithoutCookie(), webRequestProcessor)

[<Fact>]
let ``Succesful post with empty response`` () =
    let url = @"http://localhost"
    let fakeProcessor = new FakeWebRequestProcessor((fun body -> body.Value |> should equal "{\"name\":\"post\"}"), 
                                                    "")
    let airClient = createAirHttpClient(fakeProcessor)

    let response = airClient.Post<RequestObject>(url, new RequestObject("post"))
    response.StatusCode |> should equal HttpStatusCode.OK

[<Fact>]
let ``Succesful post with value response`` () =
    let url = @"http://localhost"
    let fakeProcessor = new FakeWebRequestProcessor((fun body -> body.Value |> should equal "{\"name\":\"post\"}"), 
                                                    "{\"id\":5, \"name\":\"Ivan\"}")
    let airClient = createAirHttpClient(fakeProcessor)

    let response = airClient.Post<RequestObject, ResponseObject>(url, new RequestObject("post"))
    response.StatusCode |> should equal HttpStatusCode.OK
    response.Value.Name |> should equal "Ivan"
    response.Value.Id |> should equal 5

[<Fact>]
let ``Failed post with value response`` () =
    let url = @"http://localhost"
    let fakeProcessor = new FakeWebRequestProcessor((fun _ -> raise(new Exception("postException"))), 
                                                    "{\"id\":5, \"name\":\"Ivan\"}")
    let airClient = createAirHttpClient(fakeProcessor)

    let response = airClient.Post<RequestObject, ResponseObject>(url, new RequestObject("post"))
    response.Failed |> should equal true
    response.FaultException.Message |> should equal "postException"
