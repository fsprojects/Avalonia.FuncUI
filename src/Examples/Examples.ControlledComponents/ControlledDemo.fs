namespace Examples.ControlledComponents

open System.Text.RegularExpressions
open Avalonia.FuncUI.DSL
open Avalonia.FuncUI.Controls
open Avalonia.FuncUI.DSL
open Avalonia.FuncUI.Helpers

module ControlledDemo =
    open Avalonia.Controls
    open Avalonia.Layout
    
    type State =
        { maskedString : string
          checkItems: (string * bool) list
          pickerString : string
          changeCt : int }
    let init =
        { maskedString = ""
          checkItems = [("A", false); ("B", true); ("C", false)]
          pickerString = "A"
          changeCt = 0 }

    type Msg =
    | SetMaskedString of string
    | SetPickerString of string
    | SetIsChecked of (string * bool)
    | ToggleCheckAll
    | IncrChange
    
    let selectAllState selectedCt listCt =
        if selectedCt = 0 then Unchecked
        elif selectedCt = listCt then Checked
        else Indeterminate

    let update (msg: Msg) (state: State) : State =
        match msg with
        | SetMaskedString str -> { state with maskedString = str }
        | SetPickerString str -> { state with pickerString = str }
        | SetIsChecked (name, isChecked) ->
            let list =
                state.checkItems
                |> List.map (fun t -> if (fst t) = name then (name, isChecked) else t)
            { state with checkItems = list }
        | ToggleCheckAll ->
            let nextList =
                let isChecked = state.checkItems |> List.forall snd |> not
                state.checkItems |> List.map (fun (n, _) -> n, isChecked)
            { state with checkItems = nextList }
            
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
        
    let boolToState b = if b then Checked else Unchecked
    let stateToBool s = s = Checked
        
    let selectAllView state dispatch =
        let selectAll =
            (DockPanel.create [
                DockPanel.dock Dock.Top
                DockPanel.horizontalAlignment HorizontalAlignment.Stretch
                DockPanel.children [
                    TextBlock.create [
                        TextBlock.text "Select All"
                    ]
                    ControlledCheckBox.create [
                        ControlledCheckBox.value (
                            let selectedCt = state.checkItems |> List.filter snd |> List.length
                            let listCt = state.checkItems |> List.length
                            selectAllState selectedCt listCt
                        )
                        ControlledCheckBox.onChange (fun _ -> ToggleCheckAll |> dispatch)
                    ]
                ]
            ]) |> generalize
        let rows =
            (state.checkItems |> List.map (fun (name, isChecked) ->
                DockPanel.create [
                    DockPanel.horizontalAlignment HorizontalAlignment.Stretch
                    DockPanel.children [
                        TextBlock.create [
                            TextBlock.text name
                        ]
                        ControlledCheckBox.create [
                            ControlledCheckBox.value (isChecked |> boolToState)
                            ControlledCheckBox.onChange (fun e ->
                                let b = e.State |> stateToBool
                                (name, b) |> SetIsChecked |> dispatch
                            )
                        ]
                    ]
                ] |> generalize))
        labelView
            "Checkboxes: Select All as a function of state"            
            (selectAll::rows)

    
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
                selectAllView state dispatch
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