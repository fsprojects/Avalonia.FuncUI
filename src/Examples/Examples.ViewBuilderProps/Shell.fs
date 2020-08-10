namespace Examples.ViewBuilderProps

/// This is the main module of your application
/// here you handle all of your child pages as well as their
/// messages and their updates, useful to update multiple parts
/// of your application, Please refer to the `view` function
/// to see how to handle different kinds of "*child*" controls
module Shell =
    open Elmish
    open Avalonia
    open Avalonia.Controls
    open Avalonia.Input
    open Avalonia.FuncUI.DSL
    open Avalonia.FuncUI
    open Avalonia.FuncUI.Builder
    open Avalonia.FuncUI.Components.Hosts
    open Avalonia.FuncUI.Elmish

    type FormValues =
        { password: string; email: string }
    type State =
        /// store the child state in your main state
        { counterState: Counter.State
          formValues: FormValues }

    type Msg =
        | CounterMsg of Counter.Msg
        | FormSubmitted of {|email: string; password: string |}

    let init =
        let counterState = Counter.init
        { counterState = counterState
          formValues = { email = "email@domain.com"; password = "" } },
        /// If your children controls don't emit any commands
        /// in the init function, you can just return Cmd.none
        /// otherwise, you can use a batch operation on all of them
        /// you can add more init commands as you need
        Cmd.none

    let update (msg: Msg) (state: State): State * Cmd<_> =
        match msg with
        | CounterMsg countermsg ->
            let counterMsg =
                Counter.update countermsg state.counterState
            { state with counterState = counterMsg },
            /// map the message to the kind of message 
            /// your child control needs to handle
            Cmd.none
        | FormSubmitted values ->
            printfn "%A" values
            { state with formValues = { email = ""; password = "" } } , Cmd.none

    let view (state: State) (dispatch) =
        let formProps: Form.HostProps =
            { email = state.formValues.email
              password = state.formValues.password
              submit = fun (props) -> dispatch (FormSubmitted props) }
        DockPanel.create [
            DockPanel.children [
                TabControl.create [
                      TabControl.tabStripPlacement Dock.Top
                      TabControl.viewItems [
                            TabItem.create [
                                  TabItem.header "Counter Sample"
                                  TabItem.content (Counter.view state.counterState (CounterMsg >> dispatch)) ]
                            TabItem.create [
                                  TabItem.header "Form"
                                  TabItem.content (ViewBuilder.Create<Form.Host>([], [| formProps |])) ]
                            ]
                      ]
                ]
            ]

    /// This is the main window of your application
    /// you can do all sort of useful things here like setting heights and widths
    /// as well as attaching your dev tools that can be super useful when developing with
    /// Avalonia
    type MainWindow() as this =
        inherit HostWindow()
        do
            base.Title <- "Full App"
            base.Width <- 800.0
            base.Height <- 600.0
            base.MinWidth <- 800.0
            base.MinHeight <- 600.0

            //this.VisualRoot.VisualRoot.Renderer.DrawFps <- true
            //this.VisualRoot.VisualRoot.Renderer.DrawDirtyRects <- true

            Elmish.Program.mkProgram (fun () -> init) update view
            |> Program.withHost this
            |> Program.run
