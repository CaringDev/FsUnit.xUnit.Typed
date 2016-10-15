[<AutoOpen>]
module FsUnit.Xunit.Typed

open NHamcrest
open NHamcrest.Core

open Xunit.Sdk

type MatchException(expected, actual, userMessage) =
    inherit AssertActualExpectedException(expected, actual, userMessage)

let be = id

let inline should (f : 'e -> IMatcher<'a>) expected actual =
    let matcher =
        match f expected with
        | null -> Is.Null<_>() : IMatcher<_>
        | m -> m
    if not  <| matcher.Matches actual then
        let description = StringDescription()
        matcher.DescribeTo description
        let mismatchDescription = StringDescription()
        matcher.DescribeMismatch(actual, mismatchDescription)
        MatchException(string description, string mismatchDescription, null) |> raise
