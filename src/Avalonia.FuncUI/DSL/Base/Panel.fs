namespace Avalonia.FuncUI.DSL


[<AutoOpen>]
module Panel =  
    open Avalonia.Controls
    open Avalonia.Media.Immutable
    open System    
    open System.Windows.Input
    open Avalonia
    open Avalonia.FuncUI.Core.Domain
    open Avalonia.Media
    open Avalonia.Styling
    open Avalonia.Controls.Primitives
    open Avalonia.Controls.Templates
    open Avalonia.Animation
    open Avalonia.Layout
    open Avalonia.Interactivity
    open Avalonia.Input

    type Panel with
            
        static member children<'t when 't :> Panel>(value: IView list) : IAttr<'t> =
            let getter : (IControl -> obj) option = Some (fun control -> (control :?> Panel).Children :> obj)
            let setter : (IControl * obj -> unit) option = None
            
            let accessor = Accessor.create("Children", getter, setter)
            let content = Content.createMultiple(Accessor.InstanceProperty accessor, value)
            let attr = Attr.createContent<'t> content
            attr :> IAttr<'t>
            
        static member background<'t when 't :> Panel>(value: #IBrush) : IAttr<'t> =
            let accessor = Accessor.AvaloniaProperty Panel.BackgroundProperty
            let property = Property.createDirect(accessor, value)
            let attr = Attr.createProperty<'t> property
            attr :> IAttr<'t>    