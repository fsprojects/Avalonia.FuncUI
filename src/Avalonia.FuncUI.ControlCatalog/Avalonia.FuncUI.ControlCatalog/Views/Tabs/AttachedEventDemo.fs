namespace Avalonia.FuncUI.ControlCatalog.Views

module AttachedEventDemo =
    open Avalonia
    open Avalonia.Controls
    open Avalonia.Layout
    open Avalonia.Input
    open Avalonia.Interactivity
    open Elmish
    open Avalonia.FuncUI
    open Avalonia.FuncUI.DSL
    open Avalonia.FuncUI.Elmish

    type State = { count: int; input: int; info: string }

    let init () =
        { count = 0; input = 0; info = "" }, Cmd.none

    type Msg =
        | Increment
        | Decrement
        | SetInput of int
        | SetCount of int
        | Reset
        | SetInfo of string

    let update (msg: Msg) (state: State) : State * Cmd<_> =
        match msg with
        | Increment -> { state with count = state.count + 1 }, Cmd.none
        | Decrement -> { state with count = state.count - 1 }, Cmd.none
        | SetInput input -> { state with input = input }, Cmd.none
        | SetCount count -> { state with count = count }, Cmd.none
        | Reset -> init ()
        | SetInfo info -> { state with info = info }, Cmd.none

    let (|Name|_|) name (x: obj) : 't option when 't :> StyledElement =
        match x with
        | :? 't as t when t.Name = name -> Some t
        | _ -> None

    let sprintRoutedEvent (s: #Interactive) (e: #RoutedEventArgs) =
        let time = System.DateTime.Now.ToString ("HH:mm:ss.fff")

        let sender =
            if System.String.IsNullOrEmpty s.Name then
                s.GetType().Name
            else
                $"{s.GetType().Name}#{s.Name}"

        let source =
            match e.Source with
            | :? Interactive as i -> $"{i.GetType().Name}#{i.Name}"
            | _ -> e.Source.GetType().Name

        $"{time}: {sender} handled {e.RoutedEvent.Name} in {source}"

    let checkNumText input dispatch onFixed =
        let fixedInput = input |> String.filter System.Char.IsNumber
        if input <> fixedInput then
            SetInfo $"Fixed \"{input}\" -> \"{fixedInput}\"" |> dispatch
        onFixed fixedInput

    let view (state: State) (dispatch) =

        DockPanel.create [
            DockPanel.name "Root"
            DockPanel.attachedEvent (
                InputElement.PointerEnteredEvent,
                fun (s, e) -> sprintRoutedEvent s e |> SetInfo |> dispatch
            )
            DockPanel.attachedEvent (
                InputElement.PointerExitedEvent,
                fun (s, e) -> sprintRoutedEvent s e |> SetInfo |> dispatch
            )
            DockPanel.attachedEvent<_, TextInputEventArgs> (
                TextBox.TextInputEvent,
                fun (s, e) ->
                    sprintRoutedEvent s e |> SetInfo |> dispatch

                    match e.Source with
                    | Name (nameof SetInput) _ ->
                        checkNumText e.Text dispatch e.set_Text
                    | _ -> ()
            )
            DockPanel.attachedEvent<_, TextChangedEventArgs> (
                TextBox.TextChangedEvent,
                fun (_, e) ->
                    match e.Source with
                    | Name (nameof SetInput) (tb: TextBox) ->
                        checkNumText tb.Text dispatch (fun fixedInput ->
                            if tb.Text <> fixedInput then
                                tb.Text <- fixedInput
                            int32 fixedInput |> SetInput |> dispatch
                        )
                    | _ -> ()
            )
            DockPanel.attachedEvent<_, RoutedEventArgs> (
                Button.ClickEvent,
                (fun (s, e) ->
                    match e.Source with
                    | Name (nameof Increment) _ -> Increment |> dispatch
                    | Name (nameof Decrement) _ -> Decrement |> dispatch
                    | Name (nameof Reset) _ -> Reset |> dispatch
                    | Name (nameof SetCount) _ -> SetCount state.input |> dispatch
                    | _ -> ()

                    sprintRoutedEvent s e |> SetInfo |> dispatch),
                OnChangeOf state.input
            )

            DockPanel.children [
                TextBlock.create [
                    TextBlock.name (nameof state.info)
                    TextBlock.dock Dock.Bottom
                    TextBlock.text state.info
                ]
                Button.create [
                    Button.name (nameof Reset)
                    Button.dock Dock.Bottom
                    Button.horizontalAlignment HorizontalAlignment.Stretch
                    Button.horizontalContentAlignment HorizontalAlignment.Center
                    Button.content "reset"
                ]
                Button.create [
                    Button.name (nameof Decrement)
                    Button.dock Dock.Bottom
                    Button.horizontalAlignment HorizontalAlignment.Stretch
                    Button.horizontalContentAlignment HorizontalAlignment.Center
                    Button.content "-"
                ]
                Button.create [
                    Button.name (nameof Increment)
                    Button.dock Dock.Bottom
                    Button.horizontalAlignment HorizontalAlignment.Stretch
                    Button.horizontalContentAlignment HorizontalAlignment.Center
                    Button.content "+"
                ]
                TextBox.create [
                    TextBox.name (nameof SetInput)
                    TextBox.dock Dock.Bottom
                    TextBox.text (string state.input)
                    TextBox.innerRightContent (Button.create [ Button.name (nameof SetCount); Button.content "Set" ])
                ]
                TextBlock.create [
                    TextBlock.name (nameof state.count)
                    TextBlock.dock Dock.Top
                    TextBlock.fontSize 48.0
                    TextBlock.verticalAlignment VerticalAlignment.Center
                    TextBlock.horizontalAlignment HorizontalAlignment.Center
                    TextBlock.text (string state.count)
                ]
            ]
        ]

    type Host() as this =
        inherit Hosts.HostControl()

        do
            Elmish.Program.mkProgram init update view
            |> Program.withHost this
            |> Program.withConsoleTrace
            |> Program.runWithAvaloniaSyncDispatch ()
