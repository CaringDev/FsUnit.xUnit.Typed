[<AutoOpen>]
module FsUnit.Xunit.Typed.Matchers

open NHamcrest
open NHamcrest.Core

let matcher m s f =
    { new IMatcher<_> with
        member __.Matches i = m i
        member __.DescribeMismatch(i, md) = md.AppendText(f i) |> ignore
        member __.DescribeTo d = d.AppendText s |> ignore
    }

let contain value =
    matcher (Seq.contains value) (sprintf "to contain %A" value) (sprintf "%A")

let inline count c =
    let getCount (a : ^a) = (^a : (member Count : int) a)
    matcher (fun a -> getCount a = c) (sprintf "count to be %i" c) (getCount >> sprintf "%i")

let inline equal expected =
    matcher ((=) expected) (string expected) (sprintf "was %A")

let fail () =
    matcher
        (fun i -> try i(); false with _ -> true)
        "an exception"
        (fun _ -> "no exception raised")

let failWith<'exn when 'exn :> exn> () =
    let mutable exnType = ""
    matcher
        (fun i -> try i(); false with :? 'exn -> true | e -> exnType <- e.GetType().FullName; false)
        (sprintf "exception of type %A" typeof<'exn>.FullName)
        (fun _ -> sprintf "got %s" exnType)

let greaterThan value =
    matcher ((<) value) (sprintf "to be strictly greater than %A" value) (sprintf "%A")

let greaterOrEqualThan value =
    matcher ((<=) value) (sprintf "to be greater or equal than %A" value) (sprintf "%A")

let inline length l =
    let getLength (a : ^a) = (^a : (member Length : int) a)
    matcher (fun a -> getLength a = l) (sprintf "length to be %i" l) (getLength >> sprintf "%i")

let not' (m : 'e -> IMatcher<'a>) e =
    match m e with
    | null -> Is.NotNull<_>() : IMatcher<_>
    | r -> Is.Not<'a>(r) :> IMatcher<_>

let smallerThan value =
    matcher ((>) value) (sprintf "to be strictly greater than %A" value) (sprintf "%A")

let smallerOrEqualThan value =
    matcher ((>=) value) (sprintf "to be greater or equal than %A" value) (sprintf "%A")