namespace Examples.TodoApp


open System
open System.Runtime.CompilerServices
open System.Threading
open Avalonia
open Avalonia.Controls
open Avalonia.FuncUI
open Avalonia.FuncUI.Builder
open Avalonia.FuncUI.DSL
open Avalonia.FuncUI.Experimental
open Avalonia.FuncUI.Types
open Avalonia.Layout
open Avalonia.Media
open System

type ModalHostState (state: IWritable<IView list>) =

    member _.Push (modal: IView) =
        state.Set (modal :: state.Current)

    member _.Pop () =
        match state.Current with
        | [] -> ()
        | _ :: rest -> state.Set rest

type ModalHost () as this =
    inherit ComponentBase ()

    let modalStack = new State<IView list>(List.empty)

    static let _mainContentProperty: StyledProperty<IView option> =
        AvaloniaProperty.Register<ModalHost, IView option>("MainContent", None)

    static do ignore (
        _mainContentProperty.Changed.AddClassHandler<ModalHost, IView option>(fun instance mainContent ->
            instance.ForceRender()
        )
    )

    static member MainContentProperty = _mainContentProperty

    static member State = EnvironmentState<ModalHostState>.Create("state")

    member this.MainContent
        with get() = this.GetValue(ModalHost.MainContentProperty)
        and set(value) = ignore(this.SetValue(ModalHost.MainContentProperty, value))

    override this.Render(ctx: IComponentContext): IView =
        let modalStack = ctx.usePassed modalStack

        EnvironmentStateProvider.create (
            state = ModalHost.State,
            providedValue = ModalHostState(modalStack),
            content = (
                Panel.create [
                    Panel.verticalAlignment VerticalAlignment.Stretch
                    Panel.horizontalAlignment HorizontalAlignment.Stretch
                    Panel.children [
                        // main content
                        match this.MainContent with
                        | Some mainContent -> mainContent
                        | None -> ()

                        // modals
                        Panel.create [
                            Panel.verticalAlignment VerticalAlignment.Stretch
                            Panel.horizontalAlignment HorizontalAlignment.Stretch
                            Panel.children [
                                for modalView in modalStack.Current do
                                    Border.create [
                                        Border.background (SolidColorBrush(Colors.Black, 0.5))
                                        Border.padding 20
                                        Border.child modalView
                                    ]
                            ]
                        ]
                    ]
                ]
            )
        )

[<AutoOpen>]
module ModalHost =
    let create (attrs: IAttr<ModalHost> list): IView<ModalHost> =
        ViewBuilder.Create<ModalHost>(attrs)

    type ModalHost with

        static member mainContent<'t when 't :> ModalHost>(value: IView option) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty(ModalHost.MainContentProperty, value, ValueNone)

        static member mainContent<'t when 't :> ModalHost>(value: IView) : IAttr<'t> =
            value
            |> Some
            |> ModalHost.mainContent

[<AutoOpen>]
module __ContextExtensions_useModal =

    type IComponentContext with

        member this.useModalState() : ModalHostState =
            this.readEnvValue ModalHost.State



