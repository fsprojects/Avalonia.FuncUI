namespace Avalonia.FuncUI.DSL

[<AutoOpen>]
module Canvas =  
    open Avalonia.Controls
    open Avalonia.FuncUI.Builder
    open Avalonia.FuncUI.Types
    
    let create (attrs: IAttr<Canvas> list): IView<Canvas> =
        ViewBuilder.Create<Canvas>(attrs)
    
    type Control with
        /// An attached property specifying distance from the left of the parent Canvas. The applied control should be a child of a Canvas for this property to take effect.
        static member left<'t when 't :> Control>(value: double) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<double>(Canvas.LeftProperty, value, ValueNone)
            
        /// An attached property specifying distance from the top of the parent Canvas. The applied control should be a child of a Canvas for this property to take effect.
        static member top<'t when 't :> Control>(value: double) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<double>(Canvas.TopProperty, value, ValueNone)
            
        /// An attached property specifying distance from the right of the parent Canvas. The applied control should be a child of a Canvas for this property to take effect.
        static member right<'t when 't :> Control>(value: double) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<double>(Canvas.RightProperty, value, ValueNone)
            
        /// An attached property specifying distance from the bottom of the parent Canvas. The applied control should be a child of a Canvas for this property to take effect.
        static member bottom<'t when 't :> Control>(value: double) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<double>(Canvas.BottomProperty, value, ValueNone)
    
    type Canvas with
        end