namespace Avalonia.FuncUI.DSL

[<AutoOpen>]
module Animatable =  
    open Avalonia.Animation
    open Avalonia.FuncUI.Builder
    open Avalonia.FuncUI.Types
    
    type Animatable with
        static member transitions<'t when 't :> Animatable>(transitions: Transitions) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<Transitions>(Animatable.TransitionsProperty, transitions, ValueNone)
            
        // static member clock<'t when 't :> Animatable>(clock: IClock) : IAttr<'t> =
        //    AttrBuilder<'t>.CreateProperty<IClock>(Animatable.ClockProperty, clock, ValueNone)