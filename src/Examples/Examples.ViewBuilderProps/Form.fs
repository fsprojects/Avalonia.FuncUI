namespace Examples.ViewBuilderProps

/// You can use modules in Avalonia.FuncUI in the same way you would do
/// in [Elmish ](https://elmish.github.io/elmish/)
module Form =
    open Elmish
    open System.Diagnostics
    open System.Runtime.InteropServices
    open Avalonia.Controls
    open Avalonia.Layout
    open Avalonia.FuncUI
    open Avalonia.FuncUI.Components.Hosts
    open Avalonia.FuncUI.DSL
    open Avalonia.FuncUI.Elmish
    open Avalonia.FuncUI.Types


    type State =
        { email: string
          password: string
          validationErr: Option<string> }

    type SubmitFn = {| email: string; password: string |} -> unit

    type HostProps =
        { email: string; password: string; submit: SubmitFn }


    type Msg =
        | ValidateForm
        | ValidateResult of Result<{| email: string; password: string |}, string>
        | Submit of {| email: string; password: string |}
        | SetValidationError of Option<string>
        | SetEmail of string
        | SetPassword of string
    let init (props: HostProps) =
        { email = props.email
          password = props.password
          validationErr = None }, Cmd.none
        
        
    let update (msg: Msg) (state: State) (submitFn: SubmitFn) =
        match msg with
        | ValidateForm ->
            let result = Ok {| email = state.email; password = state.password |}
            state, Cmd.ofMsg (ValidateResult result)
        | ValidateResult result ->
            match result with
            | Ok result -> state, Cmd.batch [Cmd.ofMsg (Submit result); Cmd.ofMsg (SetValidationError None)]
            | Error err -> state, Cmd.ofMsg (SetValidationError (Some err))
        | SetValidationError err ->
            { state with validationErr = err }, Cmd.none
        | Submit props ->
            submitFn props
            state, Cmd.none
        | SetEmail email -> { state with email = email }, Cmd.none
        | SetPassword password -> { state with password = password }, Cmd.none
            
    let form (state: State) (dispatch: Msg -> unit) =
        StackPanel.create [
            StackPanel.spacing 12.
            StackPanel.dock Dock.Top
            StackPanel.children [
                TextBlock.create [
                    TextBlock.text "Fake Login"
                    TextBlock.fontSize 18.
                ]
                TextBlock.create [
                    TextBlock.text "Email"
                ]
                TextBox.create [
                    TextBox.watermark "user@domain.com"
                    TextBox.text state.email
                    TextBox.onTextChanged(fun text -> dispatch (SetEmail text))
                ]
                TextBlock.create [
                    TextBlock.text "Password"
                ]
                TextBox.create [
                    TextBox.passwordChar '*'
                    TextBox.watermark "**********"
                    TextBox.text state.password
                    TextBox.onTextChanged(fun text -> dispatch (SetPassword text))
                ]
                Button.create [
                    Button.content "Log in"
                    Button.onClick(fun _ -> dispatch ValidateForm)
                ]
            ]
        ]
        
    let view (state: State) (dispatch: Msg -> unit) =
        DockPanel.create [
            DockPanel.horizontalAlignment HorizontalAlignment.Center
            DockPanel.verticalAlignment VerticalAlignment.Top
            DockPanel.margin (0.0, 20.0, 0.0, 0.0)
            DockPanel.children [
                form state dispatch
            ]
        ]
    
    type Host(props: HostProps) as this =
        inherit HostControl()
        do
            let updateWithPropsCb msg state =
                update msg state (fun formValues -> props.submit formValues)

            Program.mkProgram init updateWithPropsCb view
            |> Program.withHost this
            |> Program.runWith props
            
        