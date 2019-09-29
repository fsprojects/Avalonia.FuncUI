namespace Avalonia.FuncUI.DSL
open Avalonia
open Avalonia.Controls.Templates

[<AutoOpen>]
module Border =
    open System
    open System.Threading
    open System.Collections
    
    open FSharp.Data.UnitSystems.SI.UnitNames
    
    open Avalonia.Animation
    open Avalonia.Controls
    open Avalonia.FuncUI.Core.Domain
    open Avalonia.Interactivity
    open Avalonia.Styling
    open Avalonia.Media
    open Avalonia.Media.Immutable
    
    let create (attrs: IAttr<Border> list): IView<Border> =
        View.create<Border>(attrs)
    
    type Border with
        static member background<'t when 't :> Border>(value: #IBrush) : IAttr<'t> =
            let accessor = Accessor.AvaloniaProperty Border.BackgroundProperty
            let property = Property.createDirect(accessor, value)
            let attr = Attr.createProperty<'t> property
            attr :> IAttr<'t>
            
        static member background<'t when 't :> Border>(color: string) : IAttr<'t> =
            let accessor = Accessor.AvaloniaProperty Border.BackgroundProperty
            let property = Property.createDirect(accessor, ImmutableSolidColorBrush(Color.Parse(color)))
            let attr = Attr.createProperty<'t> property
            attr :> IAttr<'t>
            
        static member borderBrush<'t when 't :> Border>(value: #IBrush) : IAttr<'t> =
            let accessor = Accessor.AvaloniaProperty Border.BorderBrushProperty
            let property = Property.createDirect(accessor, value)
            let attr = Attr.createProperty<'t> property
            attr :> IAttr<'t>
            
        static member borderBrush<'t when 't :> Border>(color: string) : IAttr<'t> =
            let accessor = Accessor.AvaloniaProperty Border.BorderBrushProperty
            let property = Property.createDirect(accessor, ImmutableSolidColorBrush(Color.Parse(color)))
            let attr = Attr.createProperty<'t> property
            attr :> IAttr<'t>
            
        static member borderThickness<'t when 't :> Border>(value: Thickness) : IAttr<'t> =
            let accessor = Accessor.AvaloniaProperty Border.BorderThicknessProperty
            let property = Property.createDirect(accessor, value)
            let attr = Attr.createProperty<'t> property
            attr :> IAttr<'t>
            
        static member borderThickness<'t when 't :> Border>(value: float) : IAttr<'t> =
            let accessor = Accessor.AvaloniaProperty Border.BorderThicknessProperty
            let property = Property.createDirect(accessor, Thickness(value))
            let attr = Attr.createProperty<'t> property
            attr :> IAttr<'t>
            
        static member borderThickness<'t when 't :> Border>(horizontal: float, vertical: float) : IAttr<'t> =
            let accessor = Accessor.AvaloniaProperty Border.BorderThicknessProperty
            let property = Property.createDirect(accessor, Thickness(horizontal, vertical))
            let attr = Attr.createProperty<'t> property
            attr :> IAttr<'t>
            
        static member borderThickness<'t when 't :> Border>(left: float, top: float, right: float, bottom: float) : IAttr<'t> =
            let accessor = Accessor.AvaloniaProperty Border.BorderThicknessProperty
            let property = Property.createDirect(accessor, Thickness(left, top, right, bottom))
            let attr = Attr.createProperty<'t> property
            attr :> IAttr<'t>
            
        static member cornerRadius<'t when 't :> Border>(value: CornerRadius) : IAttr<'t> =
            let accessor = Accessor.AvaloniaProperty Border.CornerRadiusProperty
            let property = Property.createDirect(accessor, value)
            let attr = Attr.createProperty<'t> property
            attr :> IAttr<'t>
            
        static member cornerRadius<'t when 't :> Border>(value: float) : IAttr<'t> =
            let accessor = Accessor.AvaloniaProperty Border.CornerRadiusProperty
            let property = Property.createDirect(accessor, CornerRadius(value))
            let attr = Attr.createProperty<'t> property
            attr :> IAttr<'t>
                
        static member cornerRadius<'t when 't :> Border>(horizontal: float, vertical: float) : IAttr<'t> =
            let accessor = Accessor.AvaloniaProperty Border.CornerRadiusProperty
            let property = Property.createDirect(accessor, CornerRadius(horizontal, vertical))
            let attr = Attr.createProperty<'t> property
            attr :> IAttr<'t>
            
        static member cornerRadius<'t when 't :> Border>(left: float, top: float, right: float, bottom: float) : IAttr<'t> =
            let accessor = Accessor.AvaloniaProperty Border.CornerRadiusProperty
            let property = Property.createDirect(accessor, CornerRadius(left, top, right, bottom))
            let attr = Attr.createProperty<'t> property
            attr :> IAttr<'t>
            
        static member child<'t when 't :> Border>(value: IView) : IAttr<'t> =
            let getter : (IControl -> obj) option = Some (fun control -> (control :?> Border).Child :> obj)
            let setter : (IControl * obj -> unit) option = None
            
            let accessor = Accessor.create("Child", getter, setter)
            let content = Content.createSingle(Accessor.InstanceProperty accessor, Some value)
            let attr = Attr.createContent<'t> content
            attr :> IAttr<'t>
            
        static member child<'t when 't :> Border>(value: IView option) : IAttr<'t> =
            let getter : (IControl -> obj) option = Some (fun control -> (control :?> Border).Child :> obj)
            let setter : (IControl * obj -> unit) option = None
            
            let accessor = Accessor.create("Child", getter, setter)
            let content = Content.createSingle(Accessor.InstanceProperty accessor, value)
            let attr = Attr.createContent<'t> content
            attr :> IAttr<'t>
        