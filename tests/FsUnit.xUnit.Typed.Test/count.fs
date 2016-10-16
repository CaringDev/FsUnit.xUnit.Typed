module Count

open CompilerHelper
open FsUnit.Xunit.Typed
open System.Collections.Generic
open Xunit

[<Fact>]
let ``succeeds for list``() =
    List([ 1; 2; 3 ]) |> should (have count) 3

[<Fact>]
let ``fails for list when wrong count``() =
    Assert.Throws<MatchException>(fun () ->
        List([ 1; 2; ]) |> should (have count) 3)

module is =
    [<Fact>]
    let typesafe() =
        "[ ] |> should (have count) 0"
        |> shouldNotCompileBecause "The type ''a list' does not support the operator 'get_Count'"