namespace AvaApp

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
    open Avalonia.FuncUI.Hosts
    open Avalonia.FuncUI.Elmish

    type State =
        /// store the child state in your main state
        { aboutState: About.State; counterState: Counter.State;}

    type Msg =
        | AboutMsg of About.Msg
        | CounterMsg of Counter.Msg

    let init =
        let aboutState, aboutCmd = About.init
        let counterState = Counter.init
        { aboutState = aboutState; counterState = counterState },
        /// If your children controls don't emit any commands
        /// in the init function, you can just return Cmd.none
        /// otherwise, you can use a batch operation on all of them
        /// you can add more init commands as you need
        Cmd.batch [ aboutCmd ]

    let update (msg: Msg) (state: State): State * Cmd<_> =
        match msg with
        | AboutMsg bpmsg ->
            let aboutState, cmd =
                About.update bpmsg state.aboutState
            { state with aboutState = aboutState },
            /// map the message to the kind of message 
            /// your child control needs to handle
            Cmd.map AboutMsg cmd
        | CounterMsg countermsg ->
            let counterMsg =
                Counter.update countermsg state.counterState
            { state with counterState = counterMsg },
            /// map the message to the kind of message 
            /// your child control needs to handle
            Cmd.none

    let view (state: State) (dispatch) =
        DockPanel.create
            [ DockPanel.children
                [ TabControl.create
                    [ TabControl.tabStripPlacement Dock.Top
                      TabControl.viewItems
                          [ TabItem.create
                                [ TabItem.header "Counter Sample"
                                  TabItem.content (Counter.view state.counterState (CounterMsg >> dispatch)) ]
                            TabItem.create
                                [ TabItem.header "About"
                                  TabItem.content (About.view state.aboutState (AboutMsg >> dispatch)) ] ] ] ] ]

    let program = Program.mkProgram (fun () -> init) update view