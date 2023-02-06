module Avalonia.FuncUI.PerfTests

open System.Buffers
open Avalonia.Controls
open Avalonia.FuncUI.DSL
open Avalonia.Layout
open Avalonia.Media
open BenchmarkDotNet.Attributes

[<MemoryDiagnoser>]
type CollectionBenchmarks () =

    [<Benchmark>]
    member this.FSharpList () =
        for i = 0 to 1000 do
            let a = [
                i; 2; 3; 4; 5; 6; 7; 8; 9; 10
                11; 12; 13; 14; 15; 16; 17; 18; 19; 20
                21; 22; 23; 24; 25; 26; 27; 28; 29; 30
                31; 32; 33; 34; 35; 36; 37; 38; 39; 40
                41; 42; 43; 44; 45; 46; 47; 48; 49; 50
                51; 52; 53; 54; 55; 56; 57; 58; 59; 60
                61; 62; 63; 64; 65; 66; 67; 68; 69; 70
                71; 72; 73; 74; 75; 76; 77; 78; 79; 80
                81; 82; 83; 84; 85; 86; 87; 88; 89; 90
                91; 92; 93; 94; 95; 96; 97; 98; 99; 100
            ]
            a.Length |> ignore

    [<Benchmark>]
    member this.Pooled () =
        for i = 0 to 1000 do
            use a = collection {
                i; 2; 3; 4; 5; 6; 7; 8; 9; 10
                11; 12; 13; 14; 15; 16; 17; 18; 19; 20
                21; 22; 23; 24; 25; 26; 27; 28; 29; 30
                31; 32; 33; 34; 35; 36; 37; 38; 39; 40
                41; 42; 43; 44; 45; 46; 47; 48; 49; 50
                51; 52; 53; 54; 55; 56; 57; 58; 59; 60
                61; 62; 63; 64; 65; 66; 67; 68; 69; 70
                71; 72; 73; 74; 75; 76; 77; 78; 79; 80
                81; 82; 83; 84; 85; 86; 87; 88; 89; 90
                91; 92; 93; 94; 95; 96; 97; 98; 99; 100
            }
            a.Length |> ignore

open Avalonia.Controls
open Avalonia.FuncUI.DSL
open Avalonia.Layout
open Avalonia.Media
open BenchmarkDotNet.Attributes

[<MemoryDiagnoser>]
type VirtualDomCreationBenchmarks () =

    [<Benchmark>]
    member this.CreateView () =
        StackPanel.create [
            StackPanel.margin 10
            StackPanel.orientation Orientation.Vertical
            StackPanel.children [

                for childCount in [ 0 .. 100 ] do
                    StackPanel.create [
                        StackPanel.children [
                            for _ in [ 0 .. childCount ] do
                                TextBlock.create [
                                    TextBlock.text "Hello World"
                                    TextBlock.margin 10
                                ]
                        ]
                    ]

                for attrCount in [ 0 .. 20 ] do
                    TextBlock.create [
                        yield!
                            function
                            | 0  -> TextBlock.text "Hello World"
                            | 1  -> TextBlock.fontSize 32
                            | 2  -> TextBlock.fontFamily "Arial"
                            | 3  -> TextBlock.fontStyle FontStyle.Italic
                            | 4  -> TextBlock.fontWeight FontWeight.Bold
                            | 5  -> TextBlock.foreground Brushes.Red
                            | 6  -> TextBlock.background Brushes.Gray
                            | 7  -> TextBlock.textDecorations TextDecorations.Underline
                            | 8  -> TextBlock.textWrapping TextWrapping.Wrap
                            | 9  -> TextBlock.textTrimming TextTrimming.CharacterEllipsis
                            | 10 -> TextBlock.textAlignment TextAlignment.Center
                            | 11 -> TextBlock.padding (1, 2, 3, 4)
                            | 12 -> TextBlock.margin (1, 2, 3, 4)
                            | 13 -> TextBlock.horizontalAlignment HorizontalAlignment.Center
                            | 14 -> TextBlock.verticalAlignment VerticalAlignment.Center
                            | 15 -> TextBlock.width 100
                            | 16 -> TextBlock.height 100
                            | 17 -> TextBlock.minWidth 100
                            | 18 -> TextBlock.minHeight 100
                            | 19 -> TextBlock.maxWidth 100
                            | 20 -> TextBlock.maxHeight 100
                            | _ -> TextBlock.text "Hello World"
                            |> Seq.initInfinite
                            |> Seq.take attrCount
                    ]
            ]
        ]

