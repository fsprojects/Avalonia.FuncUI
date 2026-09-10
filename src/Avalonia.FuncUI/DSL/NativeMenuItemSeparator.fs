namespace Avalonia.FuncUI.DSL

open Avalonia.Controls
open Avalonia.FuncUI.Types

[<AutoOpen>]
module NativeMenuItemSeparator =
    let create (attrs: IAttr<NativeMenuItemSeparator> list) : IView<NativeMenuItemSeparator> =
        ViewBuilder.Create<NativeMenuItemSeparator>(attrs)
