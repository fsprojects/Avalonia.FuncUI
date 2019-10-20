namespace Avalonia.FuncUI.DSL


[<AutoOpen>]
module Carousel =
    open System
    open System.Threading
    open System.Windows.Input 
    open Avalonia.Animation
    open System.Collections    
    open Avalonia.Controls
    open Avalonia.Interactivity
    open Avalonia.Input
    
    open Avalonia.FuncUI.Types
    
    let create (attrs: IAttr<Carousel> list): IView<Carousel> =
        View.create<Carousel>(attrs)
     
    type Carousel with

        static member isVirtualized<'t when 't :> Carousel>(value: bool) : IAttr<'t> =
            let accessor = Accessor.AvaloniaProperty Carousel.IsVirtualizedProperty
            let property = Property.createDirect(accessor, value)
            let attr = Attr.createProperty<'t> property
            attr :> IAttr<'t>
        
        static member pageTransition<'t when 't :> Carousel>(transition: IPageTransition) : IAttr<'t> =
            let accessor = Accessor.AvaloniaProperty Carousel.PageTransitionProperty
            let property = Property.createDirect(accessor, transition)
            let attr = Attr.createProperty<'t> property
            attr :> IAttr<'t>