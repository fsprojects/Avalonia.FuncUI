namespace Avalonia.FuncUI.UnitTests

open System.Reactive.Subjects
open Avalonia.FuncUI.Library
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
    let ``IObservable<T>.SkipFirst`` () =
        let observable = new Subject<int>()
        let seenItems = ResizeArray<int>()

        let _ =
            observable
                .SkipFirst()
                .Subscribe(fun n -> seenItems.Add(n))

        observable.OnNext(1)
        observable.OnNext(2)
        observable.OnNext(3)

        Assert.Equal(2, seenItems.Count)
        Assert.Equal(2, seenItems.[0])
        Assert.Equal(3, seenItems.[1])