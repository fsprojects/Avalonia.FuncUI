namespace Avalonia.FuncUI.VirtualDom

open Avalonia.Controls

open Avalonia.FuncUI.Core.Domain
module rec VirtualDom =

    let update (host: IControl) (last: IView option) (next: IView option) =
        ()