namespace Avalonia.FuncUI.DSL

[<AutoOpen>]
module Decorator =
    open Avalonia
    open Avalonia.Controls
    open Avalonia.FuncUI.Types
    open Avalonia.FuncUI.Builder
    
    let create (attrs: IAttr<Decorator> list): IView<Decorator> =
        ViewBuilder.Create<Decorator>(attrs)
    
    type Decorator with
            
        static member child<'t when 't :> Decorator>(value: Control) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty(Decorator.ChildProperty, value, ValueNone)    
            
        static member child<'t when 't :> Decorator>(value: IView option) : IAttr<'t> =
            AttrBuilder<'t>.CreateContentSingle(Decorator.ChildProperty, value)
        
        static member child<'t when 't :> Decorator>(value: IView) : IAttr<'t> =
            value |> Some |> Decorator.child
            
        static member padding<'t when 't :> Decorator>(value: Thickness) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<Thickness>(Decorator.PaddingProperty, value, ValueNone)
            
        static member padding<'t when 't :> Decorator>(value: float) : IAttr<'t> =
            Thickness(value) |> Decorator.padding
            
        static member padding<'t when 't :> Decorator>(horizontal: float, vertical: float) : IAttr<'t> =
            Thickness(horizontal, vertical) |> Decorator.padding
            
        static member padding<'t when 't :> Decorator>(left: float, top: float, right: float, bottom: float) : IAttr<'t> =
            Thickness(left, top, right, bottom) |> Decorator.padding 