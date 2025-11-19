namespace Examples.ThemeSwitcher.DSL

open Avalonia.Controls
open Avalonia.FuncUI.Types

module ColorPicker =
    open Avalonia.FuncUI.DSL

    let create (attrs: IAttr<ColorPicker> list) : IView<ColorPicker> = ViewBuilder.Create<ColorPicker> (attrs)
