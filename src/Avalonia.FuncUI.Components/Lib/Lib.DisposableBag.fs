namespace Avalonia.FuncUI

open System

type DisposableBag () =
    let items = ResizeArray<IDisposable>()
    member this.Add (item: IDisposable) =
        items.Add item

    interface IDisposable with
        member this.Dispose () =
            for item in items do
                item.Dispose ()


