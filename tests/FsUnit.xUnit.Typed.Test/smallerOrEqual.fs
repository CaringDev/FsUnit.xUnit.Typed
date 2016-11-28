module SmallerOrEqual

open CompilerHelper
open FsUnit.Xunit.Typed
open Xunit

[<Fact>]
let ``succeeds for smaller int``() =
    1 |> should (be smallerOrEqualThan) 2

[<Fact>]
let ``succeeds for equal int``() =
    1 |> should (be smallerOrEqualThan) 1

[<Fact>]
let ``fails for floats when second is smaller``() =
    Assert.Throws<MatchException>(fun () ->
        System.Math.PI |> should (be smallerOrEqualThan) System.Math.E)

[<Fact>]
let ``succeeds for string``() =
    "a" |> should (be smallerOrEqualThan) "b"

module is =
    [<Fact>]
    let typesafe() =
        "0 |> should (be smallerOrEqualThan) 1.0"
        |> shouldNotTypeCheckBecause wrongType<int, float>