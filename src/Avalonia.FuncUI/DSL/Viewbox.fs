namespace Avalonia.FuncUI.DSL

[<AutoOpen>]
module Viewbox =
    open Avalonia.Controls
    open Avalonia.Media
    open Avalonia.FuncUI.Types
    open Avalonia.FuncUI.Builder

    let create (attrs: Attr<Viewbox> list): IView<Viewbox> =
        ViewBuilder.Create<Viewbox>(attrs)

    type Viewbox with

        /// <summary>
        /// Sets the stretch mode, which determines how child fits into the available space.
        /// </summary>
        static member stretch<'t when 't :> Viewbox>(value: Stretch) : Attr<'t> =
            AttrBuilder<'t>.CreateProperty<Stretch>(Viewbox.StretchProperty, value, ValueNone)

        /// <summary>
        /// Sets a value controlling in what direction contents will be stretched.
        /// </summary>
        static member stretchDirection<'t when 't :> Viewbox>(value: StretchDirection) : Attr<'t> =
            AttrBuilder<'t>.CreateProperty<StretchDirection>(Viewbox.StretchDirectionProperty, value, ValueNone)

        /// <summary>
        /// Sets the child of the Viewbox
        /// </summary>
        static member child<'t when 't :> Viewbox>(value: IView option) : Attr<'t> =
            AttrBuilder<'t>.CreateContentSingle(Viewbox.ChildProperty, value)

        /// <summary>
        /// Sets the child of the Viewbox
        /// </summary>
        static member child<'t when 't :> Viewbox>(value: IView) : Attr<'t> =
            value |> Some |> Viewbox.child
