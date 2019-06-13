namespace Inspector

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

module Analyzer =
    open System

    let findAllControls () : Type list = 
        let assemblies = AppDomain.CurrentDomain.GetAssemblies()
        [
            for assembly in assemblies do
                // maybe only use exported types
                for _type in assembly.GetTypes() do
                    // exclude interfaces
                    if not _type.IsInterface then
                        for _interface in  _type.GetInterfaces() do
                            if _interface = typeof<Avalonia.Controls.IControl> then
                                yield _type
        ]

