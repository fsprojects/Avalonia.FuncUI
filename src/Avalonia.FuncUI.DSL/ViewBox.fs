namespace Avalonia.FuncUI.DSL

open Avalonia.Media

[<AutoOpen>]
module ViewBox =
    open Avalonia.Controls.Primitives
    open Avalonia.Controls
    open Avalonia.Layout
    open Avalonia.FuncUI.Types
    open Avalonia.FuncUI.Builder
   
    let create (attrs: IAttr<Viewbox> list): IView<Viewbox> =
        ViewBuilder.Create<Viewbox>(attrs)

    type Viewbox with

        /// <summary>
        /// Gets or sets the stretch mode, which determines how child fits into the available space.
        /// </summary>
        static member stretch<'t when 't :> Viewbox>(value: Stretch) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<Stretch>(Viewbox.StretchProperty, value, ValueNone)
