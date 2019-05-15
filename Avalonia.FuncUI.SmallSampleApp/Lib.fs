namespace Avalonia.FuncUI.SmallSampleApp

[<AutoOpen>]
module AvaloniaExtensions =
    open Avalonia.Markup.Xaml.Styling
    open System
    open Avalonia.Styling

    type Styles with
        member this.Load (source: string) = 
            let style = new StyleInclude(null)
            style.Source <- new Uri(source)
            this.Add(style)