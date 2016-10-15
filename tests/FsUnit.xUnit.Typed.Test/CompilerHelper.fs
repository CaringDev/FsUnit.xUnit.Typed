module CompilerHelper

open Microsoft.FSharp.Compiler
open Microsoft.FSharp.Compiler.SourceCodeServices
open System.Text.RegularExpressions
open Xunit

type Result =
| Ok
| ParseError of string list
| CheckError of string list
| CompileError of string list

let private simplify =
    Seq.map (fun (e : FSharpErrorInfo) -> e.Message.Trim())
    >> Seq.map (fun e -> Regex.Replace(e, @"\s+", " "))
    >> List.ofSeq

let compile src =
    let fn = "Tmp.fsx"
    let checker = FSharpChecker.Create()
    async {
        let! projectOptions = checker.GetProjectOptionsFromScript(fn, src)
        let! parseResults, checkResults = checker.ParseAndCheckFileInProject(fn, 0, src, projectOptions)
        return
            match parseResults.ParseTree, checkResults with
            | Some tree, _ ->
                match checker.CompileToDynamicAssembly([tree], "Tmp.dll", [ typedefof<Result>.Assembly.Location ], None, true) with
                | [||], _, _ -> Ok
                | err, _, _ -> err |> simplify |> CompileError
            | None, FSharpCheckFileAnswer.Succeeded(_) -> parseResults.Errors |> simplify |> ParseError
            | None, FSharpCheckFileAnswer.Aborted -> [ string checkResults ] |> CheckError
    } |> Async.RunSynchronously

let shouldNotCompileBecause msg src =
    match compile src with
    | CompileError [e] -> Assert.Equal(msg, e)
    | Ok -> failwith "Compiled"
    | r -> failwithf "Did not compile due to wrong reason(s): %A" r