namespace Avalonia.FuncUI

open System

type internal DisposableBag () =
    let items = ResizeArray<IDisposable>()
    member this.Add (item: IDisposable) =
        if not (isNull item) then
            items.Add item

    interface IDisposable with
        member this.Dispose () =
            for item in items do
                if not (isNull item) then
                    item.Dispose ()


