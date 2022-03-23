namespace FullTemplate

module About =
    open Avalonia.FuncUI
    open Avalonia.FuncUI.Types
    open System.Diagnostics
    open System.Runtime.InteropServices
    open Avalonia.Controls
    open Avalonia.Layout
    open Avalonia.FuncUI.DSL

    type Links =
        | AvaloniaRepository
        | AvaloniaAwesome
        | FuncUIRepository
        | FuncUISamples

    let openUrl url =
        let url = 
            match url with 
            | AvaloniaRepository -> "https://github.com/AvaloniaUI/Avalonia"
            | AvaloniaAwesome -> "https://github.com/AvaloniaCommunity/awesome-avalonia"
            | FuncUIRepository -> "https://github.com/fsprojects/Avalonia.FuncUI"
            | FuncUISamples -> "https://github.com/fsprojects/Avalonia.FuncUI/tree/master/src/Examples"
                
        if RuntimeInformation.IsOSPlatform(OSPlatform.Windows) then
            let start = sprintf "/c start %s" url
            Process.Start(ProcessStartInfo("cmd", start)) |> ignore
        else if RuntimeInformation.IsOSPlatform(OSPlatform.Linux) then
            Process.Start("xdg-open", url) |> ignore
        else if RuntimeInformation.IsOSPlatform(OSPlatform.OSX) then
            Process.Start("open", url) |> ignore

    let headerView (dock: Dock): IView = 
        StackPanel.create [
            StackPanel.dock dock
            StackPanel.verticalAlignment VerticalAlignment.Top
            StackPanel.children [
                TextBlock.create [
                    TextBlock.classes [ "title" ]
                    TextBlock.text "Thank you for using Avalonia.FuncUI"
                ]
                TextBlock.create [
                    TextBlock.classes [ "subtitle" ]
                    TextBlock.text (
                        "Avalonia.FuncUI is a project that provides you with an Elmish DSL for Avalonia Controls\n" + 
                        "for you to use in an F# idiomatic way. We hope you like the project and spread the word :)\n" +
                        "Questions ? Reach to us on Gitter, also check the links below"
                    )
                ]
            ]
        ]
        
        
    let avaloniaLinksView (dock: Dock) : IView = 
        StackPanel.create [
            StackPanel.dock dock
            StackPanel.horizontalAlignment HorizontalAlignment.Left
            StackPanel.children [
                TextBlock.create [
                    TextBlock.classes [ "title" ]
                    TextBlock.text "Avalonia"
                ]
                TextBlock.create [
                    TextBlock.classes [ "link" ]
                    TextBlock.onTapped(fun _ -> openUrl AvaloniaRepository)
                    TextBlock.text "Avalonia Repository"
                ]
                TextBlock.create [
                    TextBlock.classes [ "link" ]
                    TextBlock.onTapped(fun _ -> openUrl AvaloniaAwesome)
                    TextBlock.text "Awesome Avalonia"
                ]
            ]
        ]
        
    let avaloniaFuncUILinksView (dock: Dock) : IView = 
        StackPanel.create [
            StackPanel.dock dock
            StackPanel.horizontalAlignment HorizontalAlignment.Right
            StackPanel.children [
                TextBlock.create [
                    TextBlock.classes [ "title" ]
                    TextBlock.text "Avalonia.FuncUI"
                ]
                TextBlock.create [
                    TextBlock.classes [ "link" ]
                    TextBlock.onTapped(fun _ -> openUrl FuncUIRepository)
                    TextBlock.text "Avalonia.FuncUI Repository"
                ]
                TextBlock.create [
                    TextBlock.classes [ "link" ]
                    TextBlock.onTapped(fun _ -> openUrl FuncUISamples)
                    TextBlock.text "Samples"
                ] 
            ]
        ]
        
    let view =
        Component.create("About", fun _ ->
            DockPanel.create [
                DockPanel.horizontalAlignment HorizontalAlignment.Center
                DockPanel.verticalAlignment VerticalAlignment.Top
                DockPanel.margin (0.0, 20.0, 0.0, 0.0)
                DockPanel.children [
                    headerView Dock.Top
                    avaloniaLinksView Dock.Left
                    avaloniaFuncUILinksView Dock.Right
                ]
            ]
        )
        