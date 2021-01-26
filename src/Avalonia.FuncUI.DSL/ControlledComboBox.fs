namespace Avalonia.FuncUI.DSL.Controls
open Avalonia.FuncUI.Controls

[<AutoOpen>]
module ControlledComboBox =
    open Avalonia.FuncUI.Builder
    open Avalonia.FuncUI.Types    
    
    let create (attrs: IAttr<ControlledComboBox> list): IView<ControlledComboBox> =
        ViewBuilder.Create<ControlledComboBox>(attrs)
        
    type ControlledComboBox with
        static member controlledSelectedIndex<'t when 't :> ControlledComboBox> state =
            let getter : 't -> int = fun c -> c.SelectedIndex
            let setter : ('t * int) -> unit = (fun (c, v) -> v |> c.SetControlledValue)
            AttrBuilder<'t>.CreateProperty<int>("SelectedIndex", state, ValueSome getter, ValueSome setter, ValueNone)
            
        static member onControlledSelectedIndexChange<'t when 't :> ControlledComboBox> fn =
            let getter : 't -> (int -> unit) = fun c -> c.OnChangeCallback
            let setter : ('t * (int -> unit)) -> unit =
                (fun (c, f) -> c.OnChangeCallback <- f)            
            AttrBuilder.CreateProperty<int -> unit>("ControlledIndexOnChange", fn, ValueSome getter, ValueSome setter, ValueNone)
            
        static member controlledOpen state fn =
            (ControlledComboBox.controlledSelectedIndex state, ControlledComboBox.onControlledSelectedIndexChange fn)
            