module CompilerHelper

open FSharp.Compiler.CodeDom
open FsUnit.Xunit.Typed
open Microsoft.FSharp.Compiler
open Microsoft.FSharp.Compiler.SourceCodeServices
open NHamcrest
open System
open System.CodeDom
open System.Text.RegularExpressions

type private Result =
| Ok
| CheckError of string list
| Aborted

let private checker = FSharpChecker.Create(keepAllBackgroundResolutions = true, msbuildEnabled = false)
let private whiteSpace = Regex(@"\s+", RegexOptions.Compiled)
let private preamble =
    [ typeof<MatchException>; typeof<IMatcher<_>> ]
    |> List.map (fun t -> sprintf "#r \"%s\"\n" <| t.Assembly.Location.Replace('\\', '/'))
    |> String.Concat
    |> sprintf "%s\nopen FsUnit.Xunit.Typed\n\n"

let private simplify =
    Seq.map (fun (e : FSharpErrorInfo) -> e.Message.Trim())
    >> Seq.map (fun e -> whiteSpace.Replace(e, " "))
    >> List.ofSeq

let private check src =
    let fn =
        let guid =
            let guid = Guid.NewGuid()
            guid.ToString("N")
        sprintf "%s.fsx" guid
    async {
        let! projectOptions, _ = checker.GetProjectOptionsFromScript(fn, src)
        let! _, checkResults = checker.ParseAndCheckFileInProject(fn, 0, src, projectOptions)
        return
            match checkResults with
            | FSharpCheckFileAnswer.Aborted -> Aborted
            | FSharpCheckFileAnswer.Succeeded res when res.Errors |> Array.isEmpty -> Ok
            | FSharpCheckFileAnswer.Succeeded res -> simplify res.Errors |> CheckError
    }

let shouldNotTypeCheckBecause msg src =
    async {
        let! result = check (preamble + src)
        match result with
        | CheckError [e] when msg = e -> ()
        | Ok -> failwith "Type checked"
        | Aborted -> failwith "Aborted"
        | CheckError r -> failwithf "Did not type check due to wrong reason(s): \"%A\", expected \"%s\"" r msg
    } |> Async.StartAsTask

let wrongType<'expected, 'actual> =
    use provider = new FSharpCodeProvider()
    let expected = provider.GetTypeOutput(CodeTypeReference(typeof<'expected>))
    let actual = provider.GetTypeOutput(CodeTypeReference(typeof<'actual>))
    sprintf "This expression was expected to have type '%s' but here has type '%s'" expected actual