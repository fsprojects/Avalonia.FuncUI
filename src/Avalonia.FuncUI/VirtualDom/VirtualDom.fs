namespace Avalonia.FuncUI.VirtualDom

open Avalonia.Controls

open Avalonia.FuncUI.Types
open Delta

module rec VirtualDom =

    let create (view: IView) : IControl =
        let root = Patcher.create view.ViewType
        let delta = ViewDelta.From view
        Patcher.patch(root, delta)
        root
    
    let update (root: IControl, last: IView, next: IView) : unit =
        let delta = Differ.diff(last, next)
        Patcher.patch(root, delta)