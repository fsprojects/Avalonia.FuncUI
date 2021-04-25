namespace Avalonia.FuncUI.DSL

open Avalonia.FuncUI.Controls

[<AutoOpen>]
module ControlledCheckBox =
    open Avalonia.FuncUI.Builder
    open Avalonia.FuncUI.Types
    
    let create (attrs: IAttr<ControlledCheckBox> list): IView<ControlledCheckBox> =
        ViewBuilder.Create<ControlledCheckBox>(attrs)
        
    type ControlledCheckBox with
        static member value<'t when 't :> ControlledCheckBox> state =
            let getter : 't -> CheckBoxState = fun c -> c.State()
            let setter : ('t * CheckBoxState) -> unit = (fun (c, v) -> v |> c.MutateControlledValue)
            AttrBuilder<'t>.CreateProperty<CheckBoxState>("Value", state, ValueSome getter, ValueSome setter, ValueNone)
            
        static member onChange<'t when 't :> ControlledCheckBox> fn =
            let getter : 't -> (CheckBoxEventArgs -> unit) = fun c -> c.OnChangeCallback
            let setter : ('t * (CheckBoxEventArgs -> unit)) -> unit =
                (fun (c, f) -> c.OnChangeCallback <- f)            
            AttrBuilder.CreateProperty<CheckBoxEventArgs -> unit>("OnChange", fn, ValueSome getter, ValueSome setter, ValueNone)
