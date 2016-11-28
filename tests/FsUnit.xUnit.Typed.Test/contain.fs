module Contain

open CompilerHelper
open FsUnit.Xunit.Typed
open Xunit

[<Fact>]
let ``succeeds for seq``() =
    seq { yield 1; yield 2 } |> should contain 2

[<Fact>]
let ``fails for seq when empty``() =
    Assert.Throws<MatchException>(fun () ->
        Seq.empty |> should contain 1)

[<Fact>]
let ``succeeds for list``() =
    [ 1.0; 2.0 ] |> should contain 2.0

[<Fact>]
let ``succeeds for array``() =
    [| 1.0; 2.0 |] |> should contain 2.0

[<Fact>]
let ``succeeds for string``() =
    "ab" |> should contain 'b'

module is =
    [<Fact>]
    let typesafe() =
        "[ 1 ] |> should contain 'a'"
        |> shouldNotTypeCheckBecause wrongType<int, char>