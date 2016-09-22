module ``Equal``

open Xunit
open FsUnit.Xunit.Typed
open Matchers

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
        obj() |> should equal (obj()))