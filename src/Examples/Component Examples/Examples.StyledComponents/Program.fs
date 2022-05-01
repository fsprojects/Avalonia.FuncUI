namespace Examples.StyledComponents

open System
open Avalonia
open Avalonia.Controls
open Avalonia.Controls.ApplicationLifetimes
open Avalonia.Controls.Presenters
open Avalonia.Controls.Shapes
open Avalonia.Controls.Templates
open Avalonia.FuncUI.Hosts
open Avalonia.FuncUI.Types
open Avalonia.Layout
open Avalonia.Media
open Avalonia.Styling
open Avalonia.Themes.Fluent
open Avalonia.FuncUI
open Avalonia.FuncUI.DSL

[<AbstractClass; Sealed>]
type Views =

    static member button () =
        Component.create ("button", fun ctx ->
            let buttonOutlet = ctx.useState<Button>(null, renderOnChange = false)
            
            ctx.attrs [
                Component.dock Dock.Top
            ]
            
            ctx.useEffect (
                handler = (fun _ ->

                    buttonOutlet.Current.Styles.Add(
                        let style = Style(fun selector -> selector.OfType<Button>())
                        style.Setters.Add(Setter(Button.BackgroundProperty, Brushes.Green))
                        
                        let template: IControlTemplate =
                            FuncControlTemplate(
                                Func<ITemplatedControl, INameScope, IControl>(fun templated scope ->
                                    let templated: Button = templated :?> Button
                                    
                                    let border = ContentPresenter()
                                    
                                    border.Bind(ContentPresenter.BackgroundProperty, templated.GetObservable(Button.BackgroundProperty))
                                    border.Bind(ContentPresenter.ContentProperty, templated.GetObservable(Button.ContentProperty))
                                    border.Bind(ContentPresenter.CornerRadiusProperty, templated.GetObservable(Button.CornerRadiusProperty))
                                    border.Bind(ContentPresenter.VerticalContentAlignmentProperty, templated.GetObservable(Button.VerticalContentAlignmentProperty))
                                    border.Bind(ContentPresenter.HorizontalContentAlignmentProperty, templated.GetObservable(Button.HorizontalContentAlignmentProperty))
                                    border.Bind(ContentPresenter.PaddingProperty, templated.GetObservable(Button.PaddingProperty))
                                    
                                    
                                    border
                                )
                             )
                        
                        
                        style.Setters.Add(Setter(Button.TemplateProperty, template))
                        style
                    )
                    
                    buttonOutlet.Current.Styles.Add(
                        let style = Style(fun selector ->
                            selector
                                .OfType<Button>()
                                .Class(":pointerover")
                                //.Template()
                                //.OfType<ContentPresenter>()
                                //.Name("PART_ContentPresenter")
                         )
                        style.Setters.Add(Setter(Button.BackgroundProperty, Brushes.Red))
                        style.Setters.Add(Setter(Button.RenderTransformProperty, RotateTransform(5.0)))
                        
                        style
                    )

                    ()    
                ),
                triggers = [ EffectTrigger.AfterInit ]
            )
           

            View.createWithOutlet buttonOutlet.Set Button.create [
                Button.verticalAlignment VerticalAlignment.Stretch
                Button.horizontalAlignment HorizontalAlignment.Stretch
                Button.verticalContentAlignment VerticalAlignment.Center
                Button.horizontalContentAlignment HorizontalAlignment.Center
                Button.cornerRadius 50
                Button.content "Hello World!"
                //Button.styles styles
            ] :> IView
        )

    static member main () =
        Component (fun ctx ->

            DockPanel.create [
                DockPanel.lastChildFill true
                DockPanel.background Brushes.White
                DockPanel.margin 50
                DockPanel.children [
                    Views.button ()
                ]
            ]
        )

type MainWindow() as this =
    inherit HostWindow()
    do
        base.Title <- "Drawing App"
        base.Width <- 500.0
        base.Height <- 500.0

        //this.VisualRoot.VisualRoot.Renderer.DrawFps <- true
        //this.VisualRoot.VisualRoot.Renderer.DrawDirtyRects <- true

        this.Content <- Views.main ()

type App() =
    inherit Application()

    override this.Initialize() =
        this.Styles.Add (FluentTheme(baseUri = null, Mode = FluentThemeMode.Light))

    override this.OnFrameworkInitializationCompleted() =
        match this.ApplicationLifetime with
        | :? IClassicDesktopStyleApplicationLifetime as desktopLifetime ->
            desktopLifetime.MainWindow <- MainWindow()

        | _ -> ()

module Program =

    [<EntryPoint>]
    let main(args: string[]) =
        AppBuilder
            .Configure<App>()
            .UsePlatformDetect()
            .UseSkia()
            .StartWithClassicDesktopLifetime(args)