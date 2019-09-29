namespace Avalonia.FuncUI.DSL

[<AutoOpen>]
module Image =
    open Avalonia
    open Avalonia.Controls
    open Avalonia.Controls.Templates
    open Avalonia.FuncUI.Types
    open Avalonia.Media
    open Avalonia.Media.Imaging
    
    let create (attrs: IAttr<Image> list): IView<Image> =
        View.create<Image>(attrs)
    
    type Image with
        static member source<'t when 't :> Image>(value: #IBitmap) : IAttr<'t> =
            let accessor = Accessor.AvaloniaProperty Image.SourceProperty
            let property = Property.createDirect(accessor, value)
            let attr = Attr.createProperty<'t> property
            attr :> IAttr<'t>
            
        static member stretch<'t when 't :> Image>(value: Stretch) : IAttr<'t> =
            let accessor = Accessor.AvaloniaProperty Image.StretchProperty
            let property = Property.createDirect(accessor, value)
            let attr = Attr.createProperty<'t> property
            attr :> IAttr<'t>