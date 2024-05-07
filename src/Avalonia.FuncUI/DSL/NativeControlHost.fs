namespace Avalonia.FuncUI.DSL

open Avalonia.Controls

[<AutoOpen>]
module NativeControlHost =
    open Avalonia.FuncUI.Types

    let create (attrs: IAttr<NativeControlHost> list): IView<NativeControlHost> =
        ViewBuilder.Create<NativeControlHost>(attrs)
