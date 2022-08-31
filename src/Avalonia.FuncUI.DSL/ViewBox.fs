namespace Avalonia.FuncUI.DSL

[<AutoOpen>]
module Viewbox =
    open Avalonia.Controls
    open Avalonia.Media
    open Avalonia.FuncUI.Types
    open Avalonia.FuncUI.Builder
   
    let create (attrs: IAttr<Viewbox> list): IView<Viewbox> =
        ViewBuilder.Create<Viewbox>(attrs)

    type Viewbox with

        /// <summary>
        /// Sets the stretch mode, which determines how child fits into the available space.
        /// </summary>
        static member stretch<'t when 't :> Viewbox>(value: Stretch) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<Stretch>(Viewbox.StretchProperty, value, ValueNone)

        /// <summary>
        /// Sets a value controlling in what direction contents will be stretched.
        /// </summary>
        static member stretchDirection<'t when 't :> Viewbox>(value: StretchDirection) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<StretchDirection>(Viewbox.StretchDirectionProperty, value, ValueNone)

        /// <summary>
        /// Sets the child of the Viewbox
        /// </summary>
        static member child<'t when 't :> Viewbox>(value: IView option) : IAttr<'t> =
            AttrBuilder<'t>.CreateContentSingle(Viewbox.ChildProperty, value)
        
        /// <summary>
        /// Sets the child of the Viewbox
        /// </summary>
        static member child<'t when 't :> Viewbox>(value: IView) : IAttr<'t> =
            value |> Some |> Viewbox.child