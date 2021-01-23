namespace Examples.ControlledComponents

open System.Text.RegularExpressions
open Avalonia.FuncUI.DSL
open Avalonia.FuncUI.Controls
open Avalonia.FuncUI.DSL

module ControlledDemo =
    open Avalonia.Controls
    open Avalonia.Layout
    
    type State = { maskedString : string; pickerString : string; changeCt : int }
    let init = { maskedString = ""; pickerString = "A"; changeCt = 0 }

    type Msg =
    | SetMaskedString of string
    | SetPickerString of string
    | IncrChange

    let update (msg: Msg) (state: State) : State =
        match msg with
        | SetMaskedString str -> { state with maskedString = str }
        | SetPickerString str -> { state with pickerString = str }
        | IncrChange -> { state with changeCt = state.changeCt + 1 }
        
    let mask = @"[^aeiouAEIOU]"
    let noVowels = String.filter (fun c -> Regex.IsMatch(string c, mask))
    
    let labelTextBoxView header onChange value =
        DockPanel.create [
            DockPanel.dock Dock.Top
            DockPanel.horizontalAlignment HorizontalAlignment.Stretch
            DockPanel.children [
                TextBlock.create [
                    TextBlock.text header
                    TextBlock.dock Dock.Top
                    TextBlock.horizontalAlignment HorizontalAlignment.Stretch
                ]
                ControlledTextBox.create [
                    TextBox.dock Dock.Top
                    TextBox.horizontalAlignment HorizontalAlignment.Stretch
                    ControlledTextBox.onChange onChange
                    ControlledTextBox.value value
                ]
            ]
        ]
        
    let picker value onChange  =
        DockPanel.create [
            DockPanel.dock Dock.Top
            DockPanel.horizontalAlignment HorizontalAlignment.Stretch
            DockPanel.children [
                TextBlock.create [
                    TextBlock.text "Picker: Update value without extra messages dispatched"
                    TextBlock.dock Dock.Top
                    TextBlock.horizontalAlignment HorizontalAlignment.Stretch
                ]
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
        ]
    
    let view (state: State) (dispatch) =
        DockPanel.create [
            DockPanel.children [
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