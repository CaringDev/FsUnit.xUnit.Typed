module Smaller

open CompilerHelper
open FsUnit.Xunit.Typed
open Xunit

[<Fact>]
let ``succeeds for int``() =
    1 |> should (be smallerThan) 2

[<Fact>]
let ``fails for floats when second is smaller``() =
    Assert.Throws<MatchException>(fun () ->
        System.Math.PI |> should (be smallerThan) System.Math.E)

[<Fact>]
let ``succeeds for string``() =
    "a" |> should (be smallerThan) "b"

[<Fact>]
let ``equal is not smaller``() =
    Assert.Throws<MatchException>(fun () ->
        -7m |> should (be smallerThan) -7m)

module is =
    [<Fact>]
    let typesafe() =
        "0 |> should (be smallerThan) 1.0"
        |> shouldNotTypeCheckBecause wrongType<int, float>