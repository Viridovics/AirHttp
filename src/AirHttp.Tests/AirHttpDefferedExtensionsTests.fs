module AirHttpDefferedExtensionsTests

open System
open System.Net

open Xunit

open FsUnit.Xunit

open AirHttp.Responses
open AirHttp.Responses.DefferedExtensions
open AirHttp.Responses.Interfaces

type OkHttpWebResponse() =
   inherit HttpWebResponse()
   override this.StatusCode = HttpStatusCode.OK

type CallbackPathes() =
    member val FailPath = false with get, set
    member val SuccessPath = false with get, set
    member val AlwaysPath = false with get, set

[<Fact>]
let ``Test fail extension`` () =
    let callbackPathes = new CallbackPathes()
    let failResponse = AirHttpResponse.CreateFaultedResponse(new Exception("Test"))
    let response = failResponse.Fail((fun e -> 
                                        e.Message |> should equal "Test"
                                        callbackPathes.FailPath <- true))
    callbackPathes.FailPath |> should equal true
    response.FaultException.Message |> should equal "Test"

[<Fact>]
let ``Test success extension without value`` () =
    let callbackPathes = new CallbackPathes()
    let failResponse = AirHttpResponse.CreateSuccessfulResponse(new OkHttpWebResponse())
    let response = failResponse.Success((fun r -> 
                                            r.StatusCode |> should equal HttpStatusCode.OK
                                            callbackPathes.SuccessPath <- true))
    callbackPathes.SuccessPath |> should equal true
    response.StatusCode |> should equal HttpStatusCode.OK

[<Fact>]
let ``Test success extension with value`` () =
    let callbackPathes = new CallbackPathes()
    let obj = 10
    let successResponse = AirHttpResponse.CreateSuccessfulResponseWithValue(new OkHttpWebResponse(), obj)
    let response = successResponse.Success((fun (r:int) -> 
                                            r |> should equal obj
                                            callbackPathes.SuccessPath <- true))
    callbackPathes.SuccessPath |> should equal true
    response.StatusCode |> should equal HttpStatusCode.OK
    response.Value |> should equal obj

[<Fact>]
let ``Test success extension with value(response)`` () =
    let callbackPathes = new CallbackPathes()
    let obj = 10
    let successResponse = AirHttpResponse.CreateSuccessfulResponseWithValue(new OkHttpWebResponse(), obj)
    let callback = fun (r : IAirHttpResponse<int>) -> 
                                                    r.Value |> should equal obj
                                                    callbackPathes.SuccessPath <- true
    let response = successResponse.Success(callback)
    callbackPathes.SuccessPath |> should equal true
    response.StatusCode |> should equal HttpStatusCode.OK
    response.Value |> should equal obj

[<Fact>]
let ``Test success and fail extension composition (successful response)`` () =
    let callbackPathes = new CallbackPathes()
    let obj = 10
    let successResponse = AirHttpResponse.CreateSuccessfulResponseWithValue(new OkHttpWebResponse(), obj)
    let successCallback = fun (r : IAirHttpResponse<int>) -> 
                                                r.Value |> should equal obj
                                                callbackPathes.SuccessPath <- true
    let response = successResponse.Success(successCallback).Fail(fun r -> callbackPathes.FailPath <- true)                                                    
    callbackPathes.SuccessPath |> should equal true
    callbackPathes.FailPath |> should equal false
    response.StatusCode |> should equal HttpStatusCode.OK
    response.Value |> should equal obj

[<Fact>]
let ``Test success and fail extension composition (failed response)`` () =
    let callbackPathes = new CallbackPathes()
    let failResponse = AirHttpResponse.CreateFaultedResponse(new Exception("Test"))
    let response = failResponse.Success(fun (r : AirHttpResponse) -> callbackPathes.SuccessPath <- true).Fail(fun r -> callbackPathes.FailPath <- true)                                                    
    callbackPathes.SuccessPath |> should equal false
    callbackPathes.FailPath |> should equal true
    response.FaultException.Message |> should equal "Test"

[<Fact>]
let ``Test always extension)`` () =
    let callbackPathes = new CallbackPathes()
    let failResponse = AirHttpResponse.CreateFaultedResponse(new Exception("Test"))
    let response = failResponse.Always(fun r -> callbackPathes.AlwaysPath <- true)                                                    
    callbackPathes.AlwaysPath |> should equal true
    response.FaultException.Message |> should equal "Test"