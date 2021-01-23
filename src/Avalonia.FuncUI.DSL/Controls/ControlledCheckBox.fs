namespace Avalonia.FuncUI.Controls

open System
open Avalonia.Controls
open Avalonia.Interactivity
open Avalonia.Styling

[<Struct>]
type CheckBoxState =
    | Checked
    | Unchecked
    | Indeterminate
    
type CheckBoxEventArgs() =
    inherit RoutedEventArgs()
    member val State : CheckBoxState = Unchecked with get, set

type ControlledCheckBox() =
    inherit CheckBox()
    
    let toState (b: Nullable<bool>) =
        if not b.HasValue then Indeterminate
        else match b.Value with
                | true -> Checked
                | false -> Unchecked
                
    let next isThree state =
        match (state, isThree) with
        | (Indeterminate, _) -> Unchecked
        | (Unchecked, _) -> Checked
        | (Checked, true) -> Indeterminate
        | (Checked, false) -> Unchecked    
    
    interface IStyleable with
        member this.StyleKey = typeof<CheckBox>
    
    member val OnChangeCallback : CheckBoxEventArgs -> unit = ignore with get, set
    
    member this.State() =
        this.IsChecked |> toState
    
    member this.SynthesizeEvent(state) =
        let e = CheckBoxEventArgs()
        e.Source <- this
        e.Route <- RoutingStrategies.Bubble
        e.State <- state
        e
    
    override this.Toggle() =
        let nextState = this.IsChecked |> toState |> next this.IsThreeState
        let e = this.SynthesizeEvent(nextState)
        this.OnChangeCallback e
        e.Handled <- true
        
    override this.OnClick() =
        this.Toggle()
        
    member this.MutateControlledValue state =
        if state = Indeterminate then do
            this.IsChecked <- System.Nullable<bool>()
        elif state = Checked then do
            this.IsChecked <- true
        else do
            this.IsChecked <- false
            