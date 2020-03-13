namespace Avalonia.FuncUI.UnitTests

open System
open System.Threading
open Avalonia.FuncUI.Library
open Avalonia.FuncUI.Types
open Xunit

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

    [<Fact>]
    let ``subscriptions are different if either of them captures state`` () =
        let dlgt = Action<_>(ignore)
        let s1CapturingState = {
            Subscription.name = "sub"
            subscribe = fun _ -> new CancellationTokenSource()
            func = dlgt
            funcCapturesState = true
            funcType = typeof<int>
        }
        let s2CapturingState = {
            Subscription.name = "sub"
            subscribe = fun _ -> new CancellationTokenSource()
            func = dlgt
            funcCapturesState = true
            funcType = typeof<int>
        }
        let s1NotCapturingState = {
            Subscription.name = "sub"
            subscribe = fun _ -> new CancellationTokenSource()
            func = dlgt
            funcCapturesState = false
            funcType = typeof<int>
        }
        let s2NotCapturingState = {
            Subscription.name = "sub"
            subscribe = fun _ -> new CancellationTokenSource()
            func = dlgt
            funcCapturesState = false
            funcType = typeof<int>
        }

        Assert.False(s1CapturingState.Equals(s2CapturingState))
        Assert.False(s1CapturingState.Equals(s1NotCapturingState))
        Assert.False(s2CapturingState.Equals(s1CapturingState))

        Assert.True(s1NotCapturingState.Equals(s2NotCapturingState))
