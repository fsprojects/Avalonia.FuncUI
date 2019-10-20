namespace Avalonia.FuncUI.DSL

[<AutoOpen>]
module StackPanel =
    open Avalonia.Controls
    open Avalonia.Layout
    open Avalonia.FuncUI.Types
   
    let create (attrs: IAttr<StackPanel> list): IView<StackPanel> =
        View.create<StackPanel>(attrs)

    type StackPanel with
           
        static member spacing<'t when 't :> StackPanel>(value: double) : IAttr<'t> =
            let accessor = Accessor.AvaloniaProperty StackPanel.SpacingProperty
            let property = Property.createDirect(accessor, value)
            let attr = Attr.createProperty<'t> property
            attr :> IAttr<'t>
           
        static member orientation<'t when 't :> StackPanel>(orientation: Orientation) : IAttr<'t> =
            let accessor = Accessor.AvaloniaProperty StackPanel.OrientationProperty
            let property = Property.createDirect(accessor, orientation)
            let attr = Attr.createProperty<'t> property
            attr :> IAttr<'t>