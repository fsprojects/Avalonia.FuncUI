namespace Avalonia.FuncUI.DSL

[<AutoOpen>]
module Canvas =  
    open Avalonia.Controls
    open Avalonia.FuncUI.Builder
    open Avalonia.FuncUI.Types
    
    let create (attrs: IAttr<Canvas> list): IView<Canvas> =
        ViewBuilder.Create<Canvas>(attrs)
    
    type Control with
        static member left<'t when 't :> Control>(value: double) : IAttr<'t> =
            AttrBuilder.CreateProperty<'t, double>(Canvas.LeftProperty, value, ValueNone)
            
        static member top<'t when 't :> Control>(value: double) : IAttr<'t> =
            AttrBuilder.CreateProperty<'t, double>(Canvas.TopProperty, value, ValueNone)
            
        static member right<'t when 't :> Control>(value: double) : IAttr<'t> =
            AttrBuilder.CreateProperty<'t, double>(Canvas.RightProperty, value, ValueNone)
            
        static member bottom<'t when 't :> Control>(value: double) : IAttr<'t> =
            AttrBuilder.CreateProperty<'t, double>(Canvas.BottomProperty, value, ValueNone)
    
    type Canvas with
        end