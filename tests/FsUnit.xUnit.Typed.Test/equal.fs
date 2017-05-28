module Equal

open CompilerHelper
open FsUnit.Xunit.Typed
open Xunit

[<Fact>]
let ``succeeds when value equal``() =
    42 |> should equal 42

[<Fact>]
let ``succeeds when reference equal``() =
    let o = obj()
    o |> should equal o

[<Fact>]
let ``fails when value non-equal``() =
    Assert.Throws<MatchException>(fun () ->
        42 |> should equal 7)

[<Fact>]
let ``fails when reference non-equal``() =
    Assert.Throws<MatchException>(fun () ->
        obj() |> should equal <| obj())

type Foo = { bar : string }

[<Fact>]
let ``leverages F# print format``() =
    let actual = { bar = "baz" }
    let expected = { bar = "BAZ" }
    let exn = Assert.Throws<MatchException>(fun () ->
        actual |> should equal expected)
    Assert.EndsWith(sprintf "Expected: %A\r\nActual:   %A" expected actual, exn.Message)

module is =
    [<Fact>]
    let typesafe() =
        "1 |> should equal 1.0"
        |> shouldNotTypeCheckBecause wrongType<int, float>
