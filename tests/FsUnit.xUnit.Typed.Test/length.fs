module Length

open CompilerHelper
open FsUnit.Xunit.Typed
open Xunit

[<Fact>]
let ``succeeds for list``() =
    [ 1; 2; 3 ] |> should (have length) 3

[<Fact>]
let ``fails for list when wrong count``() =
    Assert.Throws<MatchException>(fun () ->
        [ 1; 2; ] |> should (have length) 3)

[<Fact>]
let ``succeeds for array``() =
    [| 1; 2; 3 |] |> should (have length) 3

[<Fact>]
let ``succeeds for string``() =
    "abc" |> should (have length) 3

module is =
    [<Fact>]
    let typesafe() =
        "System.Collections.Generic.List() |> should (have length) 0"
        |> shouldNotCompileBecause "The type 'System.Collections.Generic.List<'a>' does not support the operator 'get_Length'"