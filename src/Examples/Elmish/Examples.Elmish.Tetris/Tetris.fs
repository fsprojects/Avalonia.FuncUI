namespace Example.Tetris

module Game =

    open Core
    open System


    type State =
        { tetrimino: Tetrimino
          hold: Tetrimino option
          board: TetrisBoard
          lastUpdated: DateTime
          isOver: bool
          score: int }

    type Msg =
        | Empty
        | Update
        | NewGame
        | Left
        | Right
        | Down
        | RotL
        | RotR
        | Hold

    let generateMinos =
        let mutable queue = []
        let random = Random()

        let blocks =
            [| Shape.I; Shape.O; Shape.S; Shape.Z; Shape.J; Shape.L; Shape.T |]
            |> Array.map (Tetrimino.create config.initialXY)

        fun reset ->
            if reset then
                queue <- []

            match queue with
            | [] ->
                blocks |> Array.sortInPlaceBy (fun _ -> random.Next(0, 100))
                queue <- Array.toList blocks[1 .. blocks.Length - 1]
                blocks[0]
            | h :: t ->
                queue <- t
                h

    let init () =
        { tetrimino = generateMinos true
          hold = None
          board = TetrisBoard.init ()
          lastUpdated = DateTime.Now
          isOver = false
          score = 0 }


    let update msg state =
        match msg with
        | Update ->
            if state.isOver || (DateTime.Now - state.lastUpdated).TotalMilliseconds < 500.0 then
                state
            else
                let nxt = state.tetrimino |> Tetrimino.moveDown state.board

                if state.tetrimino <> nxt then
                    { state with
                        tetrimino = nxt
                        lastUpdated = DateTime.Now }
                else
                    nxt
                    |> Tetrimino.isHighLimitOver
                    |> function
                        | true ->
                            let existsOtherBlock = nxt |> Tetrimino.existsOtherBlock state.board

                            let res = state.board |> TetrisBoard.setTetrimino nxt

                            { state with
                                isOver = true
                                board = if existsOtherBlock then state.board else res.newBoard }
                        | false ->
                            let newMino = generateMinos false

                            let res = state.board |> TetrisBoard.setTetrimino nxt

                            { state with
                                board = res.newBoard
                                tetrimino = newMino
                                lastUpdated = DateTime.Now
                                isOver = newMino |> Tetrimino.existsOtherBlock res.newBoard
                                score = state.score + res.clearedLines }
        | RotL ->
            let nxt = state.tetrimino |> Tetrimino.rotateLeft state.board

            { state with
                tetrimino = nxt
                lastUpdated =
                    if
                        state.tetrimino.shape = Shape.O
                        || nxt = state.tetrimino
                        || nxt |> Tetrimino.moveDown state.board <> nxt
                    then
                        state.lastUpdated
                    else
                        DateTime.Now }
        | RotR ->
            let nxt = state.tetrimino |> Tetrimino.rotateRight state.board

            { state with
                tetrimino = nxt
                lastUpdated =
                    if
                        state.tetrimino.shape = Shape.O
                        || nxt = state.tetrimino
                        || nxt |> Tetrimino.moveDown state.board <> nxt
                    then
                        state.lastUpdated
                    else
                        DateTime.Now }
        | Left ->
            let nxt = state.tetrimino |> Tetrimino.moveLeft state.board

            { state with
                tetrimino = nxt
                lastUpdated =
                    if
                        state.tetrimino.shape = Shape.O
                        || nxt = state.tetrimino
                        || nxt |> Tetrimino.moveDown state.board <> nxt
                    then
                        state.lastUpdated
                    else
                        DateTime.Now }
        | Right ->
            let nxt = state.tetrimino |> Tetrimino.moveRight state.board

            { state with
                tetrimino = nxt
                lastUpdated =
                    if
                        state.tetrimino.shape = Shape.O
                        || nxt = state.tetrimino
                        || nxt |> Tetrimino.moveDown state.board <> nxt
                    then
                        state.lastUpdated
                    else
                        DateTime.Now }
        | Down ->
            { state with
                tetrimino = state.tetrimino |> Tetrimino.moveDown state.board }
        | Hold ->
            { state with
                tetrimino =
                    match state.hold with
                    | Some holdMino -> holdMino
                    | None -> generateMinos false
                hold = Some(Tetrimino.create config.initialXY state.tetrimino.shape) }
        | NewGame -> init ()
        | _ -> state
