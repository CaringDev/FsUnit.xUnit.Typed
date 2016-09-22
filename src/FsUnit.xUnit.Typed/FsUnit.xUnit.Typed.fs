module FsUnit.Xunit.Typed

open NHamcrest

open Xunit.Sdk

type MatchException(expected, actual, userMessage) =
    inherit AssertActualExpectedException(expected, actual, userMessage)

let be = id

let inline should (f : 'e -> IMatcher<_>) expected actual =
    let matcher = f expected
    if matcher |> isNull then
        failwith "Did you mean to supply `Null`"
    else if not  <| matcher.Matches actual then
        let description = StringDescription()
        matcher.DescribeTo description
        let mismatchDescription = StringDescription()
        matcher.DescribeMismatch(actual, mismatchDescription)
        MatchException(string description, string mismatchDescription, null) |> raise
