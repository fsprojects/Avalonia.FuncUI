namespace Avalonia.FuncUI.UnitTests

open Avalonia.FuncUI.VirtualDom
open Xunit
open System

module Library =

    [<Fact>]
    let ``function equality`` () =
        
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
        
    [<Fact>]
    let ``function captures state`` () =
        let greeting = "Hello"
        
        let a = (fun name -> printfn "%s %s!" greeting name)
        Assert.True(FunctionAnalysis.capturesState a)
        
        let b = (fun name -> printfn "Hello %s!" name)
        Assert.False(FunctionAnalysis.capturesState b)
        ()
        
    [<Fact>]
    let ``function captures state and caches result`` () =
        let greeting = "Hello"
        
        let a = (fun name -> printfn "%s %s!" greeting name)
        Assert.True(FunctionAnalysis.capturesState a)
        
        let b = (fun name -> printfn "Hello %s!" name)
        Assert.False(FunctionAnalysis.capturesState b)
        
        // check if cached
        Assert.True(FunctionAnalysis.cache.ContainsKey(a.GetType()))
        Assert.True(FunctionAnalysis.cache.ContainsKey(b.GetType()))
        ()