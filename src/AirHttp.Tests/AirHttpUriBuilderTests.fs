module AirHttpUriBuilderTests

open System
open System.Collections.Generic
open Xunit

open FsUnit.Xunit

open AirHttp.UriFluentBuilder
open AirHttp.UriFluentBuilder.Extensions

let inline (!>) (x:^a) : string = ((^a) : (static member op_Implicit : ^a -> string) x) 
let inline (!!>) (x:^a) : Uri = ((^a) : (static member op_Implicit : ^a -> Uri) x) 

[<Fact>]
let ``Only main uri`` () =
    let builder = new UriBuilder("domain.com/")
    builder.ToString() |> should equal "domain.com/"
    (fun () -> new UriBuilder(null) |> ignore) |> should throw typeof<ArgumentNullException>

[<Fact>]
let ``Add http/https/www`` () =
    let builderHttp = (new UriBuilder("domain.com/")).AddHttp()
    builderHttp.ToString() |> should equal "http://domain.com/"
    let builderHttps = (new UriBuilder("domain.com/")).AddHttps()
    builderHttps.ToString() |> should equal "https://domain.com/"
    builderHttps.AddWWW().ToString() |> should equal "https://www.domain.com/"

[<Fact>]
let ``Add segments (main part with /)`` () =
    let builder = (new UriBuilder("domain.com/")).AddHttp().AddWWW()
    (fun () -> builder.AddSegment(null) |> ignore) |> should throw typeof<ArgumentException>
    builder.AddSegment("test").ToString() |> should equal "http://www.domain.com/test"
    builder.AddSegment("test2").ToString() |> should equal "http://www.domain.com/test/test2"


[<Fact>]
let ``Add segments (main part without /)`` () =
    let builder = (new UriBuilder("domain.com")).AddHttp().AddWWW()
    builder.AddSegment("test").ToString() |> should equal "http://www.domain.com/test"
    builder.AddSegment("test2").ToString() |> should equal "http://www.domain.com/test/test2"

[<Fact>]
let ``Add parameters with segments`` () =
    let builder = (new UriBuilder("domain.com/")).AddHttp().AddWWW().AddSegment("test")
    (fun () -> builder.AddQueryParam(null, "value") |> ignore) |> should throw typeof<ArgumentException>
    
    builder.AddQueryParam("param", "value").ToString() |> should equal "http://www.domain.com/test?param=value"
    builder.AddQueryParam("param2", "value2").ToString() |> should equal "http://www.domain.com/test?param=value&param2=value2"
    builder.AddQueryParam("param3", "").ToString() |> should equal "http://www.domain.com/test?param=value&param2=value2&param3"
    builder.AddQueryParam("param4", null).ToString() |> should equal "http://www.domain.com/test?param=value&param2=value2&param3&param4"

[<Fact>]
let ``Add parameters without segments`` () =
    let builder = (new UriBuilder("domain.com/")).AddHttp().AddWWW()
    builder.AddQueryParam("param", "value").ToString() |> should equal "http://www.domain.com/?param=value"
    builder.AddQueryParam("param2", "value2").ToString() |> should equal "http://www.domain.com/?param=value&param2=value2"

[<Fact>]
let ``Add dictionary parameters`` () =
    let builder = (new UriBuilder("domain.com/")).AddHttp().AddWWW()
    !> builder.AddQueryParams(dict["param", 40; "param2", 700]) |> should equal "http://www.domain.com/?param=40&param2=700"
    (fun () -> builder.AddQueryParams(null:Dictionary<string, string>) |> ignore) |> should throw typeof<ArgumentNullException>

type Person = { 
  Id:int;
  Name: string;
}

[<Fact>]
let ``Add parameters from object`` () =
    let builder = (new UriBuilder("domain.com/")).AddHttp().AddWWW()
    !> builder.AddQueryParams({ Id=5; Name = "John"}) |> should equal "http://www.domain.com/?Id=5&Name=John"
    (fun () -> builder.AddQueryParams(null:obj) |> ignore) |> should throw typeof<ArgumentNullException>

[<Fact>]
let ``Add sequences parameters`` () =
    let builder = (new UriBuilder("domain.com/")).AddHttp().AddWWW()
    !> builder.AddQueryParams("param", ([40; 700] |> List.toSeq) ) |> should equal "http://www.domain.com/?param=40&param=700"
    (fun () -> builder.AddQueryParams("param", (null:seq<int>)) |> ignore) |> should throw typeof<ArgumentNullException>

[<Fact>]
let ``Check extensions`` () =
    let builderHttp = "domain.com/".AddHttp()
    builderHttp.ToString() |> should equal "http://domain.com/"
    let builderHttps = "domain.com/".AddHttps()
    builderHttps.ToString() |> should equal "https://domain.com/"
    "domain.com/".AddWWW().ToString() |> should equal "www.domain.com/"
    "domain.com/".AddSegment("test").ToString() |> should equal "domain.com/test"
    "domain.com/".AddQueryParam("param", "value").ToString() |> should equal "domain.com/?param=value"
    !> "domain.com/".AddQueryParams(dict["param", 40; "param2", 700]) |> should equal "domain.com/?param=40&param2=700"
    !> "domain.com/".AddQueryParams({ Id=5; Name = "John"}) |> should equal "domain.com/?Id=5&Name=John"
    !> "domain.com/".AddQueryParams("param", ([40; 700] |> List.toSeq)) |> should equal "domain.com/?param=40&param=700"


[<Fact>]
let ``Add empty builder`` () =
    let builder = (new UriBuilder()).AddSegment("domain.com/").AddHttp().AddWWW()
    !> builder.AddQueryParam("param", "value") |> should equal "http://www.domain.com/?param=value"
    !!> builder |> should equal (new Uri("http://www.domain.com/?param=value"))
