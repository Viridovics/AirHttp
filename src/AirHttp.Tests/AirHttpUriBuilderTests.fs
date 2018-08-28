module AirHttpUriBuilderTests

open System
open Xunit

open FsUnit.Xunit

open AirHttp.Uri
open AirHttp.Uri.Extensions

[<Fact>]
let ``Only main uri`` () =
    let builder = new UriBuilder("domain.com/")
    builder.ToString() |> should equal "domain.com/"
    (fun () -> new UriBuilder(null) |> ignore) |> should throw typeof<InvalidOperationException>

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
    (fun () -> builder.AddSegment(null) |> ignore) |> should throw typeof<InvalidOperationException>
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
    (fun () -> builder.AddParam(null, "value") |> ignore) |> should throw typeof<InvalidOperationException>
    (fun () -> builder.AddParam("param", null) |> ignore) |> should throw typeof<InvalidOperationException>
    
    builder.AddParam("param", "value").ToString() |> should equal "http://www.domain.com/test?param=value"
    builder.AddParam("param2", "value2").ToString() |> should equal "http://www.domain.com/test?param=value&param2=value2"

[<Fact>]
let ``Add parameters without segments`` () =
    let builder = (new UriBuilder("domain.com/")).AddHttp().AddWWW()
    builder.AddParam("param", "value").ToString() |> should equal "http://www.domain.com/?param=value"
    builder.AddParam("param2", "value2").ToString() |> should equal "http://www.domain.com/?param=value&param2=value2"

[<Fact>]
let ``Check extensions`` () =
    let builderHttp = "domain.com/".AddHttp()
    builderHttp.ToString() |> should equal "http://domain.com/"
    let builderHttps = "domain.com/".AddHttps()
    builderHttps.ToString() |> should equal "https://domain.com/"
    "domain.com/".AddWWW().ToString() |> should equal "www.domain.com/"
    "domain.com/".AddSegment("test").ToString() |> should equal "domain.com/test"
    "domain.com/".AddParam("param", "value").ToString() |> should equal "domain.com/?param=value"