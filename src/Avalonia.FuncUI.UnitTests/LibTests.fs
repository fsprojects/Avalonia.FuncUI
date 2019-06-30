namespace Avalonia.FuncUI.UnitTests

open Xunit
open System

module Library =

    [<Fact>]
    let ``fun`` () =
        
        let view () =
            let add = fun (count: int) -> count + 1
            let sub = fun (count: int) -> count - 1
            (add, sub)

        let (add', sub') = view ()
        let (add'', sub'') = view ()

        Assert.Equal(add'.GetType(), add''.GetType())
        Assert.Equal(sub'.GetType(), sub''.GetType())
        Assert.NotEqual(add'.GetType(), sub'.GetType())
        Assert.NotEqual(add''.GetType(), sub''.GetType())