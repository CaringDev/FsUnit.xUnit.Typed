#I "packages/FAKE/tools"
#r "FakeLib.dll"

#load "packages/SourceLink.Fake/tools/Fake.fsx"
open SourceLink

open Fake
open Fake.Testing

let projectName = "FsUnit.xUnit.Typed"
let solutionFile = projectName |> sprintf "%s.sln"
let buildDir = "./.build"
let repoDir = __SOURCE_DIRECTORY__

let run target =
    !! solutionFile
    |> MSBuildRelease "" target
    |> Log target

Target "Clean" (fun _ ->
    run "Clean"
    CleanDir buildDir)

Target "Build" (fun _ ->
    run "Build")

Target "Test" (fun _ ->
    !! "tests/**/bin/Release/*Test.dll"
    |> xUnit2 (fun p ->
        { p with
            Parallel = ParallelMode.All
            ToolPath = "packages/xunit.runner.console/tools/xunit.console.exe"
            XmlOutputPath = buildDir </> "TestResults.xml" |> Some }))

Target "SourceLink" (fun _ ->
    let baseUrl = sprintf "https://raw.githubusercontent.com/rasch/%s/{0}/%%var2%%" projectName
    !! "src/**/*.??proj"
    |> Seq.iter (fun projFile ->
        let proj = VsProj.LoadRelease projFile
        if not <| Git.Information.isCleanWorkingCopy repoDir then
            traceImportant "Working copy not clean. SourceIndexing questionable."
        SourceLink.Index proj.CompilesNotLinked proj.OutputFilePdb repoDir baseUrl))

Target "Release" (fun _ ->
    Paket.Pack (fun p ->
        { p with
            BuildPlatform = "AnyCPU"
            OutputPath = buildDir </> "nugets"
            Symbols = true }))

"Clean" ==> "Build"
"Build" ==> "Test"
"Build" ==> "SourceLink"
"Test" ==> "Release"
"SourceLink" ==> "Release"

RunTargetOrDefault "Test"