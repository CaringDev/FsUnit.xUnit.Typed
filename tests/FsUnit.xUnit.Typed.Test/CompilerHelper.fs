module CompilerHelper

open Microsoft.FSharp.Compiler
open Microsoft.FSharp.Compiler.SourceCodeServices
open System
open System.Reflection
open System.Text.RegularExpressions

type Result =
| Ok of Assembly
| ParseError of string list
| CompileError of string list

let private checker = FSharpChecker.Create(keepAllBackgroundResolutions = true, msbuildEnabled = false)
let private autoOpen = [ typedefof<Result>.Assembly.Location ]
let private whiteSpace = Regex(@"\s+", RegexOptions.Compiled)

let private simplify =
    Seq.map (fun (e : FSharpErrorInfo) -> e.Message.Trim())
    >> Seq.map (fun e -> whiteSpace.Replace(e, " "))
    >> List.ofSeq

let compile src =
    let fn, dll =
        let guid = Guid.NewGuid().ToString()
        sprintf "%s.fsx" guid, sprintf "%s.dll" guid
    async {
        let! projectOptions = checker.GetProjectOptionsFromScript(fn, src)
        let! parseResults = checker.ParseFileInProject(fn, src, projectOptions)
        return
            match parseResults.ParseTree with
            | Some tree ->
                match checker.CompileToDynamicAssembly([tree], dll, autoOpen, None) with
                | [||], 0, Some assembly -> Ok assembly
                | err, _, _ -> err |> simplify |> CompileError
            | None -> parseResults.Errors |> simplify |> ParseError
    }

let shouldNotCompileBecause msg src =
    let src = "open FsUnit.Xunit.Typed\n\n" + src
    let result = compile src |> Async.RunSynchronously
    match result with
    | CompileError [e] when msg = e -> ()
    | Ok _ -> failwith "Compiled"
    | r -> failwithf "Did not compile due to wrong reason(s): %A" r