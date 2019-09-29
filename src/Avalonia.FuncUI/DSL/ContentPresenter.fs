namespace Avalonia.FuncUI.DSL
open Avalonia
open Avalonia
open Avalonia
open Avalonia.Controls.Templates
open Avalonia.Layout

[<AutoOpen>]
module ContentPresenter =
    open Avalonia.Controls
    open Avalonia.Controls.Presenters
    open Avalonia.FuncUI.Types
    open Avalonia.Media
    open Avalonia.Media.Immutable
    
    let create (attrs: IAttr<ContentPresenter> list): IView<ContentPresenter> =
        View.create<ContentPresenter>(attrs)
    
    type ContentPresenter with
        static member background<'t when 't :> ContentPresenter>(value: #IBrush) : IAttr<'t> =
            let accessor = Accessor.AvaloniaProperty ContentPresenter.BackgroundProperty
            let property = Property.createDirect(accessor, value)
            let attr = Attr.createProperty<'t> property
            attr :> IAttr<'t>
            
        static member background<'t when 't :> ContentPresenter>(color: string) : IAttr<'t> =
            Color.Parse(color) |> ImmutableSolidColorBrush |> ContentPresenter.background

        static member borderBrush<'t when 't :> ContentPresenter>(value: #IBrush) : IAttr<'t> =
            let accessor = Accessor.AvaloniaProperty ContentPresenter.BorderBrushProperty
            let property = Property.createDirect(accessor, value)
            let attr = Attr.createProperty<'t> property
            attr :> IAttr<'t>
            
        static member borderBrush<'t when 't :> ContentPresenter>(color: string) : IAttr<'t> =
            Color.Parse(color) |> ImmutableSolidColorBrush |> ContentPresenter.borderBrush
            
        static member borderThickness<'t when 't :> ContentPresenter>(value: Thickness) : IAttr<'t> =
            let accessor = Accessor.AvaloniaProperty ContentPresenter.BorderThicknessProperty
            let property = Property.createDirect(accessor, value)
            let attr = Attr.createProperty<'t> property
            attr :> IAttr<'t>
            
        static member borderThickness<'t when 't :> ContentPresenter>(value: float) : IAttr<'t> =
            Thickness(value) |> ContentPresenter.borderThickness
            
        static member borderThickness<'t when 't :> ContentPresenter>(horizontal: float, vertical: float) : IAttr<'t> =
            Thickness(horizontal, vertical) |> ContentPresenter.borderThickness
            
        static member borderThickness<'t when 't :> ContentPresenter>(left: float, top: float, right: float, bottom: float) : IAttr<'t> =
            Thickness(left, top, right, bottom) |> ContentPresenter.borderThickness
            
        static member cornerRadius<'t when 't :> ContentPresenter>(value: CornerRadius) : IAttr<'t> =
            let accessor = Accessor.AvaloniaProperty ContentPresenter.CornerRadiusProperty
            let property = Property.createDirect(accessor, value)
            let attr = Attr.createProperty<'t> property
            attr :> IAttr<'t>
            
        static member cornerRadius<'t when 't :> ContentPresenter>(value: float) : IAttr<'t> =
            CornerRadius(value) |> ContentPresenter.cornerRadius
                
        static member cornerRadius<'t when 't :> ContentPresenter>(horizontal: float, vertical: float) : IAttr<'t> =
            CornerRadius(horizontal, vertical) |> ContentPresenter.cornerRadius
            
        static member cornerRadius<'t when 't :> ContentPresenter>(left: float, top: float, right: float, bottom: float) : IAttr<'t> =
            CornerRadius(left, right, top, bottom) |> ContentPresenter.cornerRadius
            
        static member child<'t when 't :> ContentPresenter>(value: IView option) : IAttr<'t> =
            let getter : (IControl -> obj) option = Some (fun control -> (control :?> ContentPresenter).Child :> obj)
            let setter : (IControl * obj -> unit) option = None
            
            let accessor = Accessor.create("Child", getter, setter)
            let content = Content.createSingle(Accessor.InstanceProperty accessor, value)
            let attr = Attr.createContent<'t> content
            attr :> IAttr<'t>
            
        static member child<'t when 't :> ContentPresenter>(value: IView) : IAttr<'t> =
            value |> Some |> ContentPresenter.child
                 
        static member content<'t when 't :> ContentPresenter>(value: IView option) : IAttr<'t> =
            let accessor = Accessor.AvaloniaProperty ContentPresenter.ContentProperty
            let content = Content.createSingle(accessor, value)
            let attr = Attr.createContent<'t> content
            attr :> IAttr<'t>

        static member content<'t when 't :> ContentPresenter>(value: IView) : IAttr<'t> =
            value |> Some |> ContentPresenter.content
             
        static member itemTemplate<'t when 't :> ContentPresenter>(template: IDataTemplate) : IAttr<'t> =
            let accessor = Accessor.AvaloniaProperty ContentPresenter.ContentTemplateProperty
            let property = Property.createDirect(accessor, template)
            let attr = Attr.createProperty<'t> property
            attr :> IAttr<'t>
            
        static member horizontalContentAlignment<'t when 't :> ContentPresenter>(value: HorizontalAlignment) : IAttr<'t> =
            let accessor = Accessor.AvaloniaProperty ContentPresenter.HorizontalContentAlignmentProperty
            let property = Property.createDirect(accessor, value)
            let attr = Attr.createProperty<'t> property
            attr :> IAttr<'t>
            
        static member verticalContentAlignment<'t when 't :> ContentPresenter>(value: VerticalAlignment) : IAttr<'t> =
            let accessor = Accessor.AvaloniaProperty ContentPresenter.VerticalAlignmentProperty
            let property = Property.createDirect(accessor, value)
            let attr = Attr.createProperty<'t> property
            attr :> IAttr<'t>
            
        static member padding<'t when 't :> ContentPresenter>(value: Thickness) : IAttr<'t> =
            let accessor = Accessor.AvaloniaProperty ContentPresenter.PaddingProperty
            let property = Property.createDirect(accessor, value)
            let attr = Attr.createProperty<'t> property
            attr :> IAttr<'t>
            
        static member padding<'t when 't :> ContentPresenter>(value: float) : IAttr<'t> =
            Thickness(value) |> ContentPresenter.padding
            
        static member padding<'t when 't :> ContentPresenter>(horizontal: float, vertical: float) : IAttr<'t> =
            Thickness(horizontal, vertical) |> ContentPresenter.padding
            
        static member padding<'t when 't :> ContentPresenter>(left: float, top: float, right: float, bottom: float) : IAttr<'t> =
            Thickness(left, top, right, bottom) |> ContentPresenter.padding 