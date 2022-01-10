module Examples.ChordParser.ChordParserView

open System
open System.Runtime.CompilerServices
open Avalonia
open Avalonia.Controls.ApplicationLifetimes
open Avalonia.FuncUI.Components
open Avalonia.Layout
open Avalonia.Media
open Avalonia.Controls
open Avalonia.FuncUI.DSL
open Avalonia.FuncUI.Types
open Avalonia.FuncUI
open ChordParser

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

type Msg = 
    | ParseChart
    | TransposeUp
    | TransposeDown
    | SetInputChart of chart: string

type State(writableModel: IWritable<Model>) = 
    inherit BaseState<Model, Msg>(writableModel)

    override this.Update msg model = 
        match msg with
        | ParseChart ->
            let c = ChordParser.App.tryProcessText model.Transpose model.Accidental model.UCase model.InputChordChart
            { model with OutputChordChart = c }
        | TransposeUp ->
            if model.Transpose < 11 
            then { model with Transpose = model.Transpose + 1 } |> this.Update ParseChart
            else model
        | TransposeDown ->
            if model.Transpose > -11 
            then { model with Transpose = model.Transpose - 1 } |> this.Update ParseChart
            else model
        | SetInputChart chart -> 
            { model with InputChordChart = chart } |> this.Update ParseChart

let cmp () = Component (fun ctx ->
    let state = State(ctx.useState (initModel, true))
    
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
                Grid.column 2
            ]

            // Row: inputs
            // Input Chord Chart
            TextBox.create [
                TextBox.text state.Model.InputChordChart
                TextBox.onTextChanged (fun txt -> state.Dispatch (SetInputChart txt))
                Grid.column 0
                Grid.row 1
            ]

            // Middle Column (Settings)
            StackPanel.create [
                Grid.column 1
                Grid.row 1

                StackPanel.children [
                    Button.create [
                        Button.content "▲"
                        Button.horizontalAlignment HorizontalAlignment.Stretch
                        Button.horizontalContentAlignment HorizontalAlignment.Center
                        Button.onClick (fun e -> state.Dispatch TransposeUp)
                    ]

                    TextBlock.create [
                        TextBlock.text (string state.Model.Transpose)
                        TextBlock.horizontalAlignment HorizontalAlignment.Center
                    ]

                    Button.create [
                        Button.content "▼"
                        Button.horizontalAlignment HorizontalAlignment.Stretch
                        Button.horizontalContentAlignment HorizontalAlignment.Center
                        Button.onClick (fun e -> state.Dispatch TransposeDown)
                    ]
                ]
            ]

            // Output Chord Chart
            TextBox.create [
                TextBox.text 
                    <|  match state.Model.OutputChordChart with
                        | Ok output -> output
                        | Error err -> err
                TextBox.verticalAlignment VerticalAlignment.Stretch
                TextBox.isReadOnly true
                Grid.column 2
                Grid.row 1
            ]

        ]
    ]
    :> IView
)
