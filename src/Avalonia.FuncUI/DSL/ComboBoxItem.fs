namespace Avalonia.FuncUI.DSL

[<AutoOpen>]
module ComboBoxItem =
    open Avalonia.Controls
    open Avalonia.FuncUI.Types
    open Avalonia.FuncUI.Builder

    let create (attrs: Attr<ComboBoxItem> list): View<ComboBoxItem> =
        ViewBuilder.Create<ComboBoxItem>(attrs)

    type ComboBoxItem with
        end