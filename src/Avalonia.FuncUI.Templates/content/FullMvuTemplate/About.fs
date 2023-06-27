namespace FullMvuTemplate

/// You can use modules in Avalonia.FuncUI in the same way you would do
/// in [Elmish ](https://elmish.github.io/elmish/)
module About =
    open Elmish
    open Avalonia.FuncUI
    open Avalonia.FuncUI.Types
    open System.Diagnostics
    open System.Runtime.InteropServices
    open Avalonia.Controls
    open Avalonia.Layout
    open Avalonia.FuncUI.DSL


    type State =
        { noop: bool }

    type Links =
        | AvaloniaRepository
        | AvaloniaAwesome
        | FuncUIRepository
        | FuncUISamples

    type Msg = OpenUrl of Links

    let init() = { noop = false }, Cmd.none


    let update (msg: Msg) (state: State) =
        match msg with
        | OpenUrl link -> 
            let url = 
                match link with 
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
            state, Cmd.none

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
                        "for you to use in an F# idiomatic way. We hope you like the project and spread the word :)\n"
                    )
                ]
            ]
        ] |> Helpers.generalize
        
        
    let avaloniaLinksView (dock: Dock) (dispatch: Msg -> unit) : IView = 
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
                    TextBlock.onTapped(fun _ -> dispatch (OpenUrl AvaloniaRepository))
                    TextBlock.text "Avalonia Repository"
                ]
                TextBlock.create [
                    TextBlock.classes [ "link" ]
                    TextBlock.onTapped(fun _ -> dispatch (OpenUrl AvaloniaAwesome))
                    TextBlock.text "Awesome Avalonia"
                ]
            ]
        ] |> Helpers.generalize
        
    let avaloniaFuncUILinksView (dock: Dock) (dispatch: Msg -> unit) : IView = 
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
                    TextBlock.onTapped(fun _ -> dispatch (OpenUrl FuncUIRepository))
                    TextBlock.text "Avalonia.FuncUI Repository"
                ]
                TextBlock.create [
                    TextBlock.classes [ "link" ]
                    TextBlock.onTapped(fun _ -> dispatch (OpenUrl FuncUISamples))
                    TextBlock.text "Samples"
                ] 
            ]
        ] |> Helpers.generalize
        
    let view (state: State) (dispatch: Msg -> unit) =
        DockPanel.create [
            DockPanel.horizontalAlignment HorizontalAlignment.Center
            DockPanel.verticalAlignment VerticalAlignment.Top
            DockPanel.margin (0.0, 20.0, 0.0, 0.0)
            DockPanel.children [
                headerView Dock.Top
                avaloniaLinksView Dock.Left dispatch
                avaloniaFuncUILinksView Dock.Right dispatch
            ]
        ]