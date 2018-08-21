module AirHttpResponseTests

open System
open System.Net
open Xunit

open FsUnit.Xunit

open AirHttp.Responses

[<Fact>]
let ``Successful creation empty AirHttpResponse`` () =
    let httpWebResponse = new HttpWebResponse()
    let response = AirHttpResponse.CreateSuccessfulResponse(httpWebResponse)
    response.Failed |> should equal false
    response.OriginalResponse |> should equal httpWebResponse

[<Fact>]
let ``Faulted creation empty AirHttpResponse`` () =
    let responseException = new Exception("Test")
    let response = AirHttpResponse.CreateFaultedResponse(responseException)
    response.Failed |> should equal true
    response.FaultException.Message |> should equal "Test"
    (fun () -> response.OriginalResponse |> ignore) |> should throw typeof<InvalidOperationException>

[<Fact>]
let ``Successful creation AirHttpResponse with value`` () =
    let httpWebResponse = new HttpWebResponse()
    let value = new Object()
    let response = AirHttpResponse.CreateSuccessfulResponseWithValue(httpWebResponse, value)
    response.Failed |> should equal false
    response.OriginalResponse |> should equal httpWebResponse
    response.Value |> should equal value

[<Fact>]
let ``Faulted creation AirHttpResponse with value`` () =
    let responseException = new Exception("Test")
    let response = AirHttpResponse<Object>.CreateFaultedResponseWithValue(responseException)
    response.Failed |> should equal true
    response.FaultException.Message |> should equal "Test"
    (fun () -> response.OriginalResponse |> ignore) |> should throw typeof<InvalidOperationException>
    (fun () -> response.Value |> ignore) |> should throw typeof<InvalidOperationException>