namespace Avalonia.FuncUI.DSL

[<AutoOpen>]
module RangeBase =
    open Avalonia.Controls.Primitives
    open Avalonia.FuncUI.Types
    open Avalonia.FuncUI.Builder
     
    type RangeBase with

        static member minimum<'t when 't :> RangeBase>(value: double) : IAttr<'t> =
            AttrBuilder.CreateProperty<'t, double>(RangeBase.MinimumProperty, value, ValueNone)
                
        static member maximum<'t when 't :> RangeBase>(value: double) : IAttr<'t> =
            AttrBuilder.CreateProperty<'t, double>(RangeBase.MaximumProperty, value, ValueNone)
            
        static member value<'t when 't :> RangeBase>(value: double) : IAttr<'t> =
            AttrBuilder.CreateProperty<'t, double>(RangeBase.ValueProperty, value, ValueNone)

        static member onValueChanged<'t when 't :> RangeBase>(func: double -> unit, ?subPatchOptions) : IAttr<'t> =
            AttrBuilder.CreateSubscription<'t, double>(RangeBase.ValueProperty, func, ?subPatchOptions = subPatchOptions)

        static member smallChange<'t when 't :> RangeBase>(value: double) : IAttr<'t> =
            AttrBuilder.CreateProperty<'t, double>(RangeBase.SmallChangeProperty, value, ValueNone)
            
        static member largeChange<'t when 't :> RangeBase>(value: double) : IAttr<'t> =
            AttrBuilder.CreateProperty<'t, double>(RangeBase.LargeChangeProperty, value, ValueNone)