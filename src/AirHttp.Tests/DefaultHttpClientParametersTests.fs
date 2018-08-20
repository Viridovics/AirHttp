module Tests

open System
open Xunit

open FsUnit.Xunit

open AirHttp.Configuration

let defaultParameters = new HttpClientParameters()

[<Fact>]
let ``Test default connection timeout parameter`` () =
    defaultParameters.TimeoutInMilliseconds |> float |>
        should equal (TimeSpan.FromSeconds(100.0).TotalMilliseconds)

[<Fact>]
let ``Test save cookie parameter`` () =
    defaultParameters.SaveCookie |>
        should equal true
