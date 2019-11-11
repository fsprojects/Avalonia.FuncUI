namespace Avalonia.FuncUI.ControlCatalog.Views

open Avalonia
open System
open Avalonia.Controls
open Avalonia.Controls
open Avalonia.Controls.Presenters
open Avalonia.Controls.Shapes
open Avalonia.FuncUI.DSL
open Avalonia.FuncUI.Components
open Avalonia.FuncUI.Elmish
open Elmish
open Avalonia.Layout
open Avalonia.Markup.Xaml.Styling
open Avalonia.Media
open Avalonia.Styling
open Avalonia.Styling
open SharpDX.Direct3D11

module StylesDemo =
    type State = { color: string }

    let init = { color = "gray" }

    type Msg =
    | SetColor of string

    let update (msg: Msg) (state: State) : State =
        match msg with
        | SetColor color -> { state with color = color }
           
    let view (state: State) (dispatch) =
        StackPanel.create [
            StackPanel.styles (
                let styles = Styles()
                let style = Style(fun x -> x.Class("round").OfType<Button>().Template().OfType<ContentPresenter>())
                         
                let setter = new Setter(ContentPresenter.CornerRadiusProperty, CornerRadius(10.0))
                style.Setters.Add setter
                styles.Add style
                styles
            )
            StackPanel.children [
                Button.create [
                    Button.margin 10.0
                    Button.content "Button with round corners (inline)"
                    Button.styles (
                         let styles = Styles()
                         let style = Style(fun x -> x.OfType<Button>().Template().OfType<ContentPresenter>())
                         
                         let setter = new Setter(ContentPresenter.CornerRadiusProperty, CornerRadius(10.0))
                         style.Setters.Add setter
                         
                         
                         styles.Add style
                         styles
                    )
                ]
                
                Button.create [
                    Button.margin 10.0
                    Button.content "Button with round corners (class selector)"
                    Button.classes [ "round" ]
                ] 
            ]
        ]   
        
    type Host() as this =
        inherit Hosts.HostControl()
        do
            this.Styles.Load "avares://Avalonia.FuncUI.ControlCatalog/Views/Tabs/Styles.xaml"
            
            Elmish.Program.mkSimple (fun () -> init) update view
            |> Program.withHost this
            |> Program.withConsoleTrace
            |> Program.run
        
        
        

