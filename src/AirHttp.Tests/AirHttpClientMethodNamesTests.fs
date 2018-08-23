(*
For output
open Xunit.Abstractions

type MyTests(output:ITestOutputHelper) =
*)

module AirHttpClientMethodNamesTests

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
    let parameters = new HttpClientParameters()
    parameters.SaveCookie <- false
    parameters

let createAirHttpClient(webRequestProcessor) = AirHttpClient(new FakeContentProcessor(), createHttpClientParametersWithoutCookie(), webRequestProcessor)

[<Fact>]
let ``Check get, post, patch, delete, put, head, exec method headers for empty body and empty content requests`` () =
    let url = @"http://localhost"
    let fakeProcessor = createFakeWebRequestProcessor()
    let airClient = createAirHttpClient(fakeProcessor)

    airClient.Exec("method", url).Failed  |> should equal false
    fakeProcessor.PassedMethodName |> should equal "method"

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

[<Fact>]
let ``Check get, post, patch, put, exec method headers for empty body and not empty content requests`` () =
    let url = @"http://localhost"
    let fakeProcessor = createFakeWebRequestProcessor()
    let airClient = createAirHttpClient(fakeProcessor)

    airClient.Exec<Object>("method", url).Failed  |> should equal false
    fakeProcessor.PassedMethodName |> should equal "method"

    airClient.Get<Object>(url).Failed  |> should equal false
    fakeProcessor.PassedMethodName |> should equal "GET"

    airClient.Post<Object>(url).Failed  |> should equal false
    fakeProcessor.PassedMethodName |> should equal "POST"

    airClient.Put<Object>(url).Failed  |> should equal false
    fakeProcessor.PassedMethodName |> should equal "PUT"

    airClient.Patch<Object>(url).Failed  |> should equal false
    fakeProcessor.PassedMethodName |> should equal "PATCH"

[<Fact>]
let ``Check post, patch, put, exec method headers for not empty body and empty content requests`` () =
    let url = @"http://localhost"
    let fakeProcessor = createFakeWebRequestProcessor()
    let airClient = createAirHttpClient(fakeProcessor)

    let bodyObj = Object()

    airClient.Exec<Object>("method", url, bodyObj).Failed  |> should equal false
    fakeProcessor.PassedMethodName |> should equal "method"

    airClient.Post<Object>(url, bodyObj).Failed  |> should equal false
    fakeProcessor.PassedMethodName |> should equal "POST"

    airClient.Put<Object>(url, bodyObj).Failed  |> should equal false
    fakeProcessor.PassedMethodName |> should equal "PUT"

    airClient.Patch<Object>(url, bodyObj).Failed  |> should equal false
    fakeProcessor.PassedMethodName |> should equal "PATCH"

[<Fact>]
let ``Check post, patch, put, exec method headers for not empty body and not empty content requests`` () =
    let url = @"http://localhost"
    let fakeProcessor = createFakeWebRequestProcessor()
    let airClient = createAirHttpClient(fakeProcessor)

    let bodyObj = Object()

    airClient.Exec<Object, Object>("method", url, bodyObj).Failed  |> should equal false
    fakeProcessor.PassedMethodName |> should equal "method"

    airClient.Post<Object, Object>(url, bodyObj).Failed  |> should equal false
    fakeProcessor.PassedMethodName |> should equal "POST"

    airClient.Put<Object, Object>(url, bodyObj).Failed  |> should equal false
    fakeProcessor.PassedMethodName |> should equal "PUT"

    airClient.Patch<Object, Object>(url, bodyObj).Failed  |> should equal false
    fakeProcessor.PassedMethodName |> should equal "PATCH"

[<Fact>]
let ``Reconfigure method name`` () =
    let url = @"http://localhost"
    let fakeProcessor = createFakeWebRequestProcessor()
    let airClient = createAirHttpClient(fakeProcessor)

    let bodyObj = Object()
    airClient.Post<Object, Object>(url, bodyObj).Failed  |> should equal false
    fakeProcessor.PassedMethodName |> should equal "POST"

    let parameters = new HttpClientParameters()
    parameters.SaveCookie <- false
    parameters.ConfigureRequest <- (fun request -> request.Method <- "ReconfiguredMethod")
    airClient.Reconfigure(parameters)

    airClient.Put<Object, Object>(url, bodyObj).Failed  |> should equal false
    fakeProcessor.PassedMethodName |> should equal "ReconfiguredMethod"

    airClient.Patch<Object, Object>(url, bodyObj).Failed  |> should equal false
    fakeProcessor.PassedMethodName |> should equal "ReconfiguredMethod"

[<Fact>]
let ``Check async get, post, patch, delete, put, head, exec method headers for empty body and empty content requests`` () =
    let url = @"http://localhost"
    let fakeProcessor = createFakeWebRequestProcessor()
    let airClient = createAirHttpClient(fakeProcessor)

    airClient.ExecAsync("method", url).Result.Failed  |> should equal false
    fakeProcessor.PassedMethodName |> should equal "method"

    airClient.GetAsync(url).Result.Failed  |> should equal false
    fakeProcessor.PassedMethodName |> should equal "GET"

    airClient.PostAsync(url).Result.Failed  |> should equal false
    fakeProcessor.PassedMethodName |> should equal "POST"

    airClient.PutAsync(url).Result.Failed  |> should equal false
    fakeProcessor.PassedMethodName |> should equal "PUT"

    airClient.PatchAsync(url).Result.Failed  |> should equal false
    fakeProcessor.PassedMethodName |> should equal "PATCH"

    airClient.HeadAsync(url).Result.Failed  |> should equal false
    fakeProcessor.PassedMethodName |> should equal "HEAD"

    airClient.DeleteAsync(url).Result.Failed  |> should equal false
    fakeProcessor.PassedMethodName |> should equal "DELETE"

[<Fact>]
let ``Check async get, post, patch, put, exec method headers for empty body and not empty content requests`` () =
    let url = @"http://localhost"
    let fakeProcessor = createFakeWebRequestProcessor()
    let airClient = createAirHttpClient(fakeProcessor)

    airClient.ExecAsync<Object>("method", url).Result.Failed  |> should equal false
    fakeProcessor.PassedMethodName |> should equal "method"

    airClient.GetAsync<Object>(url).Result.Failed  |> should equal false
    fakeProcessor.PassedMethodName |> should equal "GET"

    airClient.PostAsync<Object>(url).Result.Failed  |> should equal false
    fakeProcessor.PassedMethodName |> should equal "POST"

    airClient.PutAsync<Object>(url).Result.Failed  |> should equal false
    fakeProcessor.PassedMethodName |> should equal "PUT"

    airClient.PatchAsync<Object>(url).Result.Failed  |> should equal false
    fakeProcessor.PassedMethodName |> should equal "PATCH"

[<Fact>]
let ``Check async post, patch, put, exec method headers for not empty body and empty content requests`` () =
    let url = @"http://localhost"
    let fakeProcessor = createFakeWebRequestProcessor()
    let airClient = createAirHttpClient(fakeProcessor)

    let bodyObj = Object()

    airClient.ExecAsync<Object>("method", url, bodyObj).Result.Failed  |> should equal false
    fakeProcessor.PassedMethodName |> should equal "method"

    airClient.PostAsync<Object>(url, bodyObj).Result.Failed  |> should equal false
    fakeProcessor.PassedMethodName |> should equal "POST"

    airClient.PutAsync<Object>(url, bodyObj).Result.Failed  |> should equal false
    fakeProcessor.PassedMethodName |> should equal "PUT"

    airClient.PatchAsync<Object>(url, bodyObj).Result.Failed  |> should equal false
    fakeProcessor.PassedMethodName |> should equal "PATCH"

[<Fact>]
let ``Check async post, patch, put, exec method headers for not empty body and not empty content requests`` () =
    let url = @"http://localhost"
    let fakeProcessor = createFakeWebRequestProcessor()
    let airClient = createAirHttpClient(fakeProcessor)

    let bodyObj = Object()

    airClient.ExecAsync<Object, Object>("method", url, bodyObj).Result.Failed  |> should equal false
    fakeProcessor.PassedMethodName |> should equal "method"

    airClient.PostAsync<Object, Object>(url, bodyObj).Result.Failed  |> should equal false
    fakeProcessor.PassedMethodName |> should equal "POST"

    airClient.PutAsync<Object, Object>(url, bodyObj).Result.Failed  |> should equal false
    fakeProcessor.PassedMethodName |> should equal "PUT"

    airClient.PatchAsync<Object, Object>(url, bodyObj).Result.Failed  |> should equal false
    fakeProcessor.PassedMethodName |> should equal "PATCH"