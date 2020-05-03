# FsUnit.xUnit.Typed

[![NuGet badge](https://buildstats.info/nuget/FsUnit.xUnit.Typed?includePreReleases=true)](https://www.nuget.org/packages/FsUnit.xUnit.Typed)\
[![Build status](https://ci.appveyor.com/api/projects/status/ffv5lwhfngiuulh0/branch/master?svg=true)](https://ci.appveyor.com/project/CaringDev/fsunit-xunit-typed/branch/master/tests) ![.NET Core](https://github.com/CaringDev/FsUnit.xUnit.Typed/workflows/.NET%20Core/badge.svg)\
[![Build history](https://buildstats.info/appveyor/chart/CaringDev/FsUnit-xUnit-Typed?includeBuildsFromPullRequest=false&branch=master)](https://ci.appveyor.com/project/CaringDev/fsunit-xunit-typed/history)

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
