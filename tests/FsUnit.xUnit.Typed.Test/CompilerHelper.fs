module CompilerHelper

open FSharp.Compiler.CodeDom
open Microsoft.FSharp.Compiler
open Microsoft.FSharp.Compiler.SourceCodeServices
open System
open System.CodeDom
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
    | r -> failwithf "Did not compile due to wrong reason(s): %A, expected %s" r msg

let wrongType<'expected, 'actual> =
    use provider = new FSharpCodeProvider()
    let expected = provider.GetTypeOutput(CodeTypeReference(typeof<'expected>))
    let actual = provider.GetTypeOutput(CodeTypeReference(typeof<'actual>))
    sprintf "This expression was expected to have type '%s' but here has type '%s'" expected actual