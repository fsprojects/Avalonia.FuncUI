namespace Example.Tetris

module View =

    open Avalonia.FuncUI.DSL
    open Avalonia.FuncUI.Helpers
    open Avalonia.FuncUI.Types
    open Avalonia.Controls
    open Avalonia.Controls.Primitives
    open Avalonia.Layout

    open Core
    open Game

    let shapeToColor shape =
        match shape with
        | Shape.I -> "#00ffff"
        | Shape.O -> "#ffd700"
        | Shape.J -> "#1e90ff"
        | Shape.L -> "#ffa500"
        | Shape.S -> "#00FF00"
        | Shape.Z -> "#ff0000"
        | Shape.T -> "#800080"

    let boardView state _ : IView =
        let w, h = config.width, config.height

        let toColor state =
            let cells = state.board

            let minoPos =
                state.tetrimino.pos.XYs
                |> Array.map (fun (x, y) -> (x + state.tetrimino.x, y + state.tetrimino.y))

            let minoShape = state.tetrimino.shape

            [| for y in 3 .. (Array2D.length1 cells) - 3 do
                   for x in 2 .. (Array2D.length2 cells) - 3 do
                       if not state.isOver && minoPos |> Array.contains (x, y) then
                           yield shapeToColor minoShape
                       else
                           match cells[y, x] with
                           | Cell.Empty -> yield "#222222"
                           | Cell.Guard -> yield "#AAAAAA"
                           | Cell.Mino shape -> yield shapeToColor shape |]

        let cellSize = 24.0

        UniformGrid.create
            [ UniformGrid.columns (w - 4)
              UniformGrid.rows (h - 4)
              UniformGrid.width (cellSize * float (w - 4))
              UniformGrid.height (cellSize * float (h - 4))
              UniformGrid.children (
                  state
                  |> toColor
                  |> Array.map (fun (color: string) ->
                      Border.create [ Border.padding 1; Border.child (Border.create [ Border.background color ]) ]
                      |> generalize)
                  |> Array.toList
              ) ]

    let holdView (state: State) _ =
        StackPanel.create
            [ StackPanel.orientation Orientation.Horizontal
              StackPanel.dock Dock.Top
              StackPanel.top 0.0

              StackPanel.children
                  [ UniformGrid.create
                        [ UniformGrid.columns 4
                          UniformGrid.rows 4
                          UniformGrid.width 70.0
                          UniformGrid.height 70.0
                          UniformGrid.left 360.0
                          UniformGrid.top 0.0
                          UniformGrid.children (
                              state.hold
                              |> Option.map (fun mino ->
                                  { mino with
                                      pos =
                                          { mino.pos with
                                              XYs = mino.pos.XYs |> Array.map (fun (a, b) -> (a + 2, b + 1)) } })
                              |> Option.map (fun mino ->
                                  [| for y in 0..3 do
                                         for x in 0..3 do
                                             if not state.isOver && mino.pos.XYs |> Array.contains (x, y) then
                                                 yield shapeToColor mino.shape
                                             else
                                                 yield "#222222" |])
                              |> function
                                  | None -> []
                                  | Some a ->
                                      a
                                      |> Array.map (fun (color: string) ->
                                          Border.create
                                              [ Border.padding 1.5
                                                Border.child (Border.create [ Border.background color ]) ]
                                          |> generalize)
                                      |> Array.toList
                          ) ] ] ]

    let menuView state _ =
        StackPanel.create
            [ StackPanel.orientation Orientation.Horizontal
              StackPanel.dock Dock.Top
              StackPanel.children
                  [ TextBlock.create
                        [ TextBlock.fontSize 16.
                          TextBlock.horizontalAlignment HorizontalAlignment.Center
                          TextBlock.width 350.
                          TextBlock.text ($"Score: %d{state.score}" ) ] ] ]

    let howToPlayView =
        StackPanel.create
            [ StackPanel.orientation Orientation.Horizontal
              StackPanel.dock Dock.Bottom
              StackPanel.children
                  [ TextBlock.create
                        [ TextBlock.fontSize 12.
                          TextBlock.horizontalAlignment HorizontalAlignment.Center
                          TextBlock.width 350.
                          TextBlock.text "[A] LEFT \n[D] RIGHT \n[SHIFT] ROT L \n[SPACE] ROT R \n[E] HOLD" ] ] ]

    let gameOverView state dispatch =
        StackPanel.create
            [ StackPanel.orientation Orientation.Vertical
              StackPanel.horizontalAlignment HorizontalAlignment.Center
              StackPanel.verticalAlignment VerticalAlignment.Center
              StackPanel.children
                  [ TextBlock.create [ TextBlock.fontSize 16.; TextBlock.margin 4.; TextBlock.text "Game Over" ]
                    Button.create
                        [ Button.fontSize 16.
                          Button.margin 4.
                          Button.onClick (fun _ -> dispatch NewGame)
                          Button.content "New game" ] ] ]

    let view state dispatch =
        if state.isOver then
            gameOverView state dispatch |> generalize
        else
            DockPanel.create
                [ DockPanel.background "#222222"
                  DockPanel.lastChildFill true
                  DockPanel.children
                      [ menuView state dispatch
                        Border.create
                            [ Control.dock Dock.Left
                              Border.borderThickness (20., 0., 0., 0.0)
                              Border.child (boardView state dispatch) ]
                        Border.create
                            [ Control.dock Dock.Right
                              Border.borderThickness (30.0, 0., 0., 0.0)
                              Border.child howToPlayView ]

                        Border.create
                            [ Control.dock Dock.Right
                              Border.borderThickness (20., 0., 0., 250.0)
                              Border.child (holdView state dispatch) ]

                        ] ]
            |> generalize
