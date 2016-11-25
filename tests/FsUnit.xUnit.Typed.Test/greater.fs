module Greater

open CompilerHelper
open FsUnit.Xunit.Typed
open Xunit

[<Fact>]
let ``succeeds for int``() =
    2 |> should (be greaterThan) 1

[<Fact>]
let ``fails for floats when second is greater``() =
    Assert.Throws<MatchException>(fun () ->
        System.Math.E |> should (be greaterThan) System.Math.PI)

[<Fact>]
let ``succeeds for string``() =
    "b" |> should (be greaterThan) "a"

[<Fact>]
let ``equal is not greater``() =
    Assert.Throws<MatchException>(fun () ->
        -7m |> should (be greaterThan) -7m)

module is =
    [<Fact>]
    let typesafe() =
        "1.0 |> should (be greaterThan) 0"
        |> shouldNotCompileBecause wrongType<float, int>