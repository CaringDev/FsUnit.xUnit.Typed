module ``Fail``

open Xunit

[<Fact>]
let ``fails when successful``() =
    Assert.Throws<MatchException>(fun () ->
        fun () -> ()
        |> should fail ())

[<Fact>]
let ``succeeds when expected thrown``() =
    fun () -> failwith "exit"
    |> should fail ()

[<Fact>]
let ``succeeds when not failing``() =
    fun () -> ()
    |> should (not' fail) ()

[<Fact>]
let ``fails when exception not expected``() =
    Assert.Throws<MatchException>(fun () ->
        fun () -> failwith "exit"
        |> should (not' fail) ())

[<Fact>]
let ``target is called``() =
    let called = ref false
    fun () -> called := true; failwith "exit"
    |> should fail ()
    Assert.True(!called, "To be asserted function is called")