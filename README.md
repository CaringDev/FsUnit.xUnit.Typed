# FsUnit.xUnit.Typed

[![Build status](https://ci.appveyor.com/api/projects/status/q99p965qj63xt962?svg=true)](https://ci.appveyor.com/project/rasch/fsunit-xunit-typed)

- a library for [xUnit](https://xunit.github.io/) testing in idiomatic [F#](http://fsharp.org/)
    - functional
    - *typesafe*
- a non-endorsed, hard fork of and heavily inspired by [FsUnit.xUnit](http://fsprojects.github.io/FsUnit/)
- supporting most needed functionality for xUnit

```fsharp
open FsUnit.Xunit.Typed
open Xunit

[<Fact>]
let ``this does not compile``() =
    42 |> should (be greaterThan) 7.0
```