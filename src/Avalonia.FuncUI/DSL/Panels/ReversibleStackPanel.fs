namespace Avalonia.FuncUI.DSL

open Avalonia.Controls

[<AutoOpen>]
module ReversibleStackPanel =
    open Avalonia.FuncUI.Types
    open Avalonia.FuncUI.Builder

    let create (attrs: IAttr<ReversibleStackPanel> list): IView<ReversibleStackPanel> =
        ViewBuilder.Create<ReversibleStackPanel>(attrs)

    type ReversibleStackPanel with
        /// Gets or sets if the child controls will be layed out in reverse order.
        static member reverseOrder<'t when 't :> ReversibleStackPanel>(value: bool) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<bool>(ReversibleStackPanel.ReverseOrderProperty, value, ValueNone)
