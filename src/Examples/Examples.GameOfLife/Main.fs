namespace Examples.GameOfLife

module Main =
    open Avalonia.Controls
    open Avalonia.FuncUI.DSL
    open Elmish
    
    type State =
        { board : BoardMatrix
          evolutionRunning : bool }
        
    let initialState() =
        { board = BoardMatrix.constructBasic(50, 50)
          evolutionRunning = false }, Cmd.none

    type Msg =
    | BoardMsg of Board.Msg   
    | StartEvolution
    | StopEvolution

    let update (msg: Msg) (state: State) : State * Cmd<_>=
        match msg with
        | StartEvolution -> { state with evolutionRunning = true }, Cmd.none
        | StopEvolution -> { state with evolutionRunning = false }, Cmd.none
        | BoardMsg msg ->
            match msg with
            | Board.Evolve ->
                if state.evolutionRunning then
                   { state with board = Board.update msg state.board }, Cmd.none
                else
                    state, Cmd.none
            | _ -> { state with board = Board.update msg state.board }, Cmd.none
    
    let view (state: State) (dispatch: Msg -> unit) =
        DockPanel.create [
            DockPanel.children [
                Button.create [
                    Button.isVisible (not state.evolutionRunning)
                    Button.dock Dock.Bottom
                    Button.background "#16a085"
                    Button.onClick ((fun _ -> StartEvolution |> dispatch), SubPatchOptions.Never)
                    Button.content "start"
                ]                
                Button.create [
                    Button.isVisible state.evolutionRunning
                    Button.dock Dock.Bottom
                    Button.background "#d35400"
                    Button.onClick ((fun _ -> StopEvolution |> dispatch), SubPatchOptions.Never)
                    Button.content "stop"
                ]
                Board.view state.board (BoardMsg >> dispatch ) 
            ]
        ]       