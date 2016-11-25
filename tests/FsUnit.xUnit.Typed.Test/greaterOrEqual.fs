module GreaterOrEqual

open CompilerHelper
open FsUnit.Xunit.Typed
open Xunit

[<Fact>]
let ``succeeds for greater int``() =
    2 |> should (be greaterOrEqualThan) 1

[<Fact>]
let ``succeeds for equal int``() =
    1 |> should (be greaterOrEqualThan) 1

[<Fact>]
let ``fails for floats when second is greater``() =
    Assert.Throws<MatchException>(fun () ->
        System.Math.E |> should (be greaterOrEqualThan) System.Math.PI)

[<Fact>]
let ``succeeds for string``() =
    "b" |> should (be greaterOrEqualThan) "a"

module is =
    [<Fact>]
    let typesafe() =
        "1.0 |> should (be greaterOrEqualThan) 0"
        |> shouldNotCompileBecause wrongType<float, int>