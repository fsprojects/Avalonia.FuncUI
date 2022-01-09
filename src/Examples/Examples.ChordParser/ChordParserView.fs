module Examples.ChordParser.ChordParserView

open System
open System.Runtime.CompilerServices
open Avalonia
open Avalonia.Controls
open Avalonia.Controls.ApplicationLifetimes
open Avalonia.FuncUI.Components
open Avalonia.FuncUI.DSL
open Avalonia.FuncUI.Types
open Avalonia.Layout
open Avalonia.FuncUI
open Avalonia.Media

type Model = 
    { 
        InputChordChart: string
        OutputChordChart: Result<string, string> 
        Transpose: int
        Accidental: string
        UCase: bool
    }

let initModel = 
    { 
        InputChordChart = 
#if DEBUG
            "(Bmaj7) Ooo Gustens,    you just (A#) so  (G)\n" +
            "Dang   (Dmin7 /G) Baaad."
#else
            ""
#endif
        OutputChordChart = Ok ""
        Transpose = 0
        Accidental = "b"
        UCase = false
    }

let cmp () = Component (fun ctx ->
    let model = ctx.useState (initModel, true)

    let setInput input = 
        model.Set { model with InputChordChart = input }

    Grid.create [
        Grid.rowDefinitions "20, *"
        Grid.columnDefinitions "*, 80, *"
        Grid.children [
            // Row labels
            TextBlock.create [
                TextBlock.text "Input Chord Chart"
                Grid.column 0
            ]
            TextBlock.create [
                TextBlock.text "Output Chord Chart"
                Grid.column 1
            ]

            // Row: inputs
            // Input Chord Chart
            TextBox.create [
                TextBox.text model.Current.InputChordChart
                TextBox.onTextChanged (fun e -> model.Set)
            ]


        ]
    ]
    :> IView
)
