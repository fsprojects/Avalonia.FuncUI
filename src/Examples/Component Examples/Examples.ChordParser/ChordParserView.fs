module Examples.ChordParser.ChordParserView

open Avalonia.Layout
open Avalonia.Controls
open Avalonia.FuncUI.DSL
open Avalonia.FuncUI.Types
open Avalonia.FuncUI
open UseElmish

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
            SampleCharts.autumnLeaves
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

let update msg model = 
    match msg with
    | ParseChart ->
        let c = ChordParser.App.tryProcessText model.Transpose model.Accidental model.UCase model.InputChordChart
        { model with OutputChordChart = c }, Cmd.none
    | TransposeUp ->
        if model.Transpose < 11 
        then { model with Transpose = model.Transpose + 1 }, Cmd.ofMsg ParseChart
        else model, Cmd.none
    | TransposeDown ->
        if model.Transpose > -11 
        then { model with Transpose = model.Transpose - 1 }, Cmd.ofMsg ParseChart
        else model, Cmd.none
    | SetInputChart chart -> 
        { model with InputChordChart = chart }, Cmd.ofMsg ParseChart

let cmp () = Component (fun ctx ->
    let model, dispatch = useElmish (ctx.useState (initModel, true), update)
    
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

            // Input Chord Chart
            TextBox.create [
                TextBox.text model.InputChordChart
                TextBox.onTextChanged (fun txt -> dispatch (SetInputChart txt))
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
                        Button.onClick (fun e -> dispatch TransposeUp)
                    ]

                    TextBlock.create [
                        TextBlock.text (string model.Transpose)
                        TextBlock.horizontalAlignment HorizontalAlignment.Center
                    ]

                    Button.create [
                        Button.content "▼"
                        Button.horizontalAlignment HorizontalAlignment.Stretch
                        Button.horizontalContentAlignment HorizontalAlignment.Center
                        Button.onClick (fun e -> dispatch TransposeDown)
                    ]
                ]
            ]

            // Output Chord Chart
            TextBox.create [
                TextBox.text <|  
                    match model.OutputChordChart with
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
