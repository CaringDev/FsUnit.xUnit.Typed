module Null

open Xunit
open CompilerHelper

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

[<Fact>]
let ``is typesafe``() =
    "null |> should be 1.0"
    |> shouldNotCompileBecause "This expression was expected to have type 'NHamcrest.IMatcher<'a>' but here has type 'float'"

module ``equal`` =

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

    [<Fact>]
    let ``is typesafe``() =
        "null |> should (not' equal) 1.0"
        |> shouldNotCompileBecause "The type 'float' does not have 'null' as a proper value"