namespace Avalonia.FuncUI.DSL

open Avalonia.FuncUI.DSL.Controls

[<AutoOpen>]
module ControlledExpander =
    open Avalonia.FuncUI.Builder
    open Avalonia.FuncUI.Types
    
    let create (attrs: IAttr<ControlledExpander> list): IView<ControlledExpander> =
        ViewBuilder.Create<ControlledExpander>(attrs)
        
    type ControlledExpander with
        static member openValue<'t when 't :> ControlledExpander> state =
            let getter : 't -> bool = fun c -> c.IsExpanded
            let setter : ('t * bool) -> unit = (fun (c, v) -> v |> c.SetControlledValue)
            AttrBuilder<'t>.CreateProperty<bool>("OpenValue", state, ValueSome getter, ValueSome setter, ValueNone)
            
        static member onOpenChange<'t when 't :> ControlledExpander> fn =
            let getter : 't -> (bool -> unit) = fun c -> c.OnChangeCallback
            let setter : ('t * (bool -> unit)) -> unit =
                (fun (c, f) -> c.OnChangeCallback <- f)            
            AttrBuilder.CreateProperty<bool -> unit>("OnOpenChange", fn, ValueSome getter, ValueSome setter, ValueNone)
            
        static member controlledOpen state fn =
            (ControlledExpander.openValue state, ControlledExpander.onOpenChange fn)
            