namespace Avalonia.FuncUI.DSL


[<AutoOpen>]
module RangeBase =
    open Avalonia.Controls.Primitives
    open Avalonia.FuncUI.Types
    open Avalonia.FuncUI.Builder
     
    type RangeBase with

        static member minimum<'t when 't :> RangeBase>(value: double) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<double>(RangeBase.MinimumProperty, value, ValueNone)
                
        static member maximum<'t when 't :> RangeBase>(value: double) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<double>(RangeBase.MaximumProperty, value, ValueNone)
            
        static member value<'t when 't :> RangeBase>(value: double) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<double>(RangeBase.ValueProperty, value, ValueNone)

        static member onValueChanged<'t when 't :> RangeBase>(func: double -> unit) : IAttr<'t> =
            AttrBuilder<'t>.CreateSubscription<double>(RangeBase.ValueProperty, func)

        static member smallChange<'t when 't :> RangeBase>(value: double) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<double>(RangeBase.SmallChangeProperty, value, ValueNone)
            
        static member largeChange<'t when 't :> RangeBase>(value: double) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<double>(RangeBase.LargeChangeProperty, value, ValueNone)