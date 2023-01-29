module Avalonia.FuncUI.PerfTests

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

