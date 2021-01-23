namespace Examples.ControlledComponents

open System.Text.RegularExpressions
open Avalonia.FuncUI.DSL
open Avalonia.FuncUI.Controls
open Avalonia.FuncUI.Helpers

module ControlledDemo =
    open Avalonia.Controls
    open Avalonia.Layout
    
    type State = { maskedString : string; isChecked: CheckBoxState; pickerString : string; changeCt : int }
    let init = { maskedString = ""; isChecked = Unchecked; pickerString = "A"; changeCt = 0 }

    type Msg =
    | SetMaskedString of string
    | SetPickerString of string
    | SetIsChecked of CheckBoxState
    | IncrChange

    let update (msg: Msg) (state: State) : State =
        match msg with
        | SetMaskedString str -> { state with maskedString = str }
        | SetPickerString str -> { state with pickerString = str }
        | SetIsChecked s -> { state with isChecked = s }
        | IncrChange -> { state with changeCt = state.changeCt + 1 }
        
    let mask = @"[^aeiouAEIOU]"
    let noVowels = String.filter (fun c -> Regex.IsMatch(string c, mask))
    
    let labelView label children =
        DockPanel.create [
            DockPanel.dock Dock.Top
            DockPanel.horizontalAlignment HorizontalAlignment.Stretch
            DockPanel.children (
                (TextBlock.create [
                    TextBlock.text label
                    TextBlock.dock Dock.Top
                    TextBlock.horizontalAlignment HorizontalAlignment.Stretch
                ] |> generalize) :: children
            )
        ]
    
    let labelTextBoxView header onChange value =
        labelView header [
            ControlledTextBox.create [
                TextBox.dock Dock.Top
                TextBox.horizontalAlignment HorizontalAlignment.Stretch
                ControlledTextBox.onChange onChange
                ControlledTextBox.value value
            ]
        ]
        
    let picker value onChange  =
        labelView "Picker: Update value without extra messages dispatched" [
            StackPanel.create [
                StackPanel.horizontalAlignment HorizontalAlignment.Stretch
                StackPanel.children [
                    Button.create [
                        Button.content "A"
                        Button.onClick (fun _ -> onChange "A")
                    ]
                    Button.create [
                        Button.content "B"
                        Button.onClick (fun _ -> onChange "B")
                    ]
                    Button.create [
                        Button.content "C"
                        Button.onClick (fun _ -> onChange "C")
                    ]
                ]
            ]
            ControlledTextBox.create [
                TextBox.horizontalAlignment HorizontalAlignment.Stretch
                ControlledTextBox.onChange (fun e -> onChange e.Text)
                ControlledTextBox.value value
            ]
        ]
    
    let view (state: State) (dispatch) =
        DockPanel.create [
            DockPanel.children [
                labelView "Controlled checkbox" [
                    ControlledCheckBox.create [
                        ControlledCheckBox.isThreeState true
                        ControlledCheckBox.value state.isChecked
                        ControlledCheckBox.onChange (fun s -> s.State |> SetIsChecked |> dispatch)
                    ]                    
                ]
                labelTextBoxView
                    "Masked input: Filters vowels"
                    (fun e -> e.Text |> noVowels |> SetMaskedString |> dispatch)
                    state.maskedString
                labelTextBoxView
                    "Change counter: Counts onChange events received"
                    (fun _ -> IncrChange |> dispatch)
                    (string state.changeCt)
                DockPanel.create [
                    DockPanel.dock Dock.Bottom
                    DockPanel.horizontalAlignment HorizontalAlignment.Stretch
                    DockPanel.children [
                        picker state.pickerString (SetPickerString >> dispatch)
                    ]
                ]                
            ]
        ]