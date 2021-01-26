namespace Avalonia.FuncUI.DSL.Controls

open Avalonia.Controls
open Avalonia.Controls.Primitives
open Avalonia.Data
open Avalonia.FuncUI
open Avalonia.Styling
open Helpers

type ControlledExpander() =
    inherit Expander()
    
    interface IStyleable with
        member this.StyleKey = typeof<Expander>
    
    member val ControllerState : ControllerState = Listening with get, set
    member val OnChangeCallback : bool -> unit = ignore with get, set
    
    member this.SetControlledValue(v) =
        this.ControllerState <- Writing
        this.IsExpanded <- v
        this.ControllerState <- Listening
                        
    // For uncontrolled changes to the property, this handler resets the property
    // to the old value, and forwards the new value to the change callback.
    override this.OnIsExpandedChanged(e) =
        if this.ControllerState = Listening then
            this.ControllerState <- Resetting
            this.IsExpanded <- e.OldValue :?> bool
            this.ControllerState <- Listening
            e.NewValue :?> bool |> this.OnChangeCallback
        else if this.ControllerState = Writing then
            base.OnIsExpandedChanged(e)
