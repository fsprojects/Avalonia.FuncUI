namespace Avalonia.FuncUI.DSL

open Avalonia.FuncUI.Controls
open Avalonia.Input

[<AutoOpen>]
module ControlledTextBox =
    open Avalonia.FuncUI.Builder
    open Avalonia.FuncUI.Types
    
    let create (attrs: IAttr<ControlledTextBox> list): IView<ControlledTextBox> =
        ViewBuilder.Create<ControlledTextBox>(attrs)
        
    type ControlledTextBox with
    
        static member value<'t when 't :> ControlledTextBox> str =
            let getter : 't -> string = fun c -> c.Text
            let setter : ('t * string) -> unit = (fun (c, v) -> v |> c.MutateControlledValue)
            
            AttrBuilder<'t>.CreateProperty<string>("Value", str, ValueSome getter, ValueSome setter, ValueNone)
            
        static member onChange<'t when 't :> ControlledTextBox> fn =
            let getter : 't -> (TextInputEventArgs -> unit) = fun c -> c.OnChangeCallback
            let setter : ('t * (TextInputEventArgs -> unit)) -> unit =
                (fun (c, f) -> c.OnChangeCallback <- f)
            
            AttrBuilder<'t>.CreateProperty<TextInputEventArgs -> unit>("OnChange", fn, ValueSome getter, ValueSome setter, ValueNone)
            