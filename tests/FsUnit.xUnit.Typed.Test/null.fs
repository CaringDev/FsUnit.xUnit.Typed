module Null

open CompilerHelper
open FsUnit.Xunit.Typed
open Xunit

[<Fact>]
let ``succeeds when null``() =
    null |> should be null

[<Fact>]
let ``fails when not null``() =
    Assert.Throws<MatchException>(fun () ->
        obj() |> should be null)

[<Fact>]
let ``succeeds when non-null not null``() =
    obj() |> should (not' be) null

[<Fact>]
let ``fails when null not null``() =
    Assert.Throws<MatchException>(fun () ->
        null |> should (not' be) null)

module is =
    [<Fact>]
    let typesafe() =
        "null |> should be 1.0"
        |> shouldNotTypeCheckBecause "This expression was expected to have type 'NHamcrest.IMatcher<'a>' but here has type 'float'"

module equal =

    [<Fact>]
    let ``succeeds when null``() =
        null |> should equal null

    [<Fact>]
    let ``fails when not null``() =
        Assert.Throws<MatchException>(fun () ->
            obj() |> should equal null)

    [<Fact>]
    let ``succeeds when non-null not null``() =
        obj() |> should (not' equal) null

    [<Fact>]
    let ``fails when null not null``() =
        Assert.Throws<MatchException>(fun () ->
            null |> should (not' equal) null)

    module is =
        [<Fact>]
        let typesafe() =
            "null |> should (not' equal) 1.0"
            |> shouldNotTypeCheckBecause "The type 'float' does not have 'null' as a proper value"