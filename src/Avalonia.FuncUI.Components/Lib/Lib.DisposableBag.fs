namespace Avalonia.FuncUI

open System

type DisposableBag () =
    let items = ResizeArray<IDisposable>()
    member this.Add (item: IDisposable) =
        if item <> null then
            items.Add item

    interface IDisposable with
        member this.Dispose () =
            for item in items do
                if item <> null then
                    item.Dispose ()


