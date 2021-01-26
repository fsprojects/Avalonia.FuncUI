namespace Avalonia.FuncUI.Controls

open System.Collections
open Avalonia
open Avalonia.Controls
open Avalonia.Controls.Primitives
open Avalonia.FuncUI
open Avalonia.FuncUI.DSL.Controls.Helpers
open Avalonia.Styling
open System.Linq
open Helpers

type ControlledComboBox() =
    inherit ComboBox()
    let idxName = SelectingItemsControl.SelectedIndexProperty.Name
    
    interface IStyleable with
        member this.StyleKey = typeof<ComboBox>
        
    member val ControllerState : ControllerState = Listening with get, set
    member val OnChangeCallback : int -> unit = ignore with get, set
    
    member this.SetControlledValue(v) =
        this.ControllerState <- Writing
        this.SelectedIndex <- v
        this.ControllerState <- Listening
    
    override this.OnPropertyChanged(args) =
        let isIdx = args.Property.Name = idxName
        if not isIdx then do
            base.OnPropertyChanged(args)
        elif this.ControllerState = Writing then do
            base.OnPropertyChanged(args)
        elif args.OldValue.HasValue &&
             args.NewValue.HasValue &&
             this.ControllerState = Listening then do
                 System.Console.WriteLine (this.SelectedItem.ToString())
                 this.ControllerState <- Resetting
                 let idx = args.OldValue.GetValueOrDefault<int>()
                 this.SelectedIndex <- idx
                 this.ControllerState <- Listening
                 let arg = args.NewValue.GetValueOrDefault<int>()
                 this.OnChangeCallback arg                
