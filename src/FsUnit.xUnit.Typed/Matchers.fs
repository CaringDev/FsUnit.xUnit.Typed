[<AutoOpen>]
module FsUnit.Xunit.Matchers

open NHamcrest
open NHamcrest.Core

let matcher m s f =
    { new IMatcher<_> with
        member __.Matches i = m i
        member __.DescribeMismatch(i, md) = md.AppendText(f i) |> ignore
        member __.DescribeTo d = d.AppendText s |> ignore
    }

let inline equal expected =
    matcher ((=) expected) (string expected) (sprintf "was %A")

let fail () =
    matcher
        (fun i -> try i(); false with _ -> true)
        "Expected an exception"
        (fun _ -> "No exception raised")

let failWith<'exn when 'exn :> exn> () =
    let mutable exnType = ""
    matcher
        (fun i -> try i(); false with :? 'exn -> true | e -> exnType <- e.GetType().FullName; false)
        (sprintf "Expected exception of type %A" typeof<'exn>.FullName)
        (fun _ -> sprintf "Got %s" exnType)

let not' (m : 'e -> IMatcher<'a>) e =
    match m e with
    | null -> Is.NotNull<_>() : IMatcher<_>
    | r -> Is.Not<'a>(r) :> IMatcher<_>