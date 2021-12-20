namespace Examples.MusicPlayer


module Player =
    open Elmish
    open LibVLCSharp.Shared
    open Avalonia.Controls
    open Avalonia.Layout
    open Avalonia.FuncUI
    open Avalonia.FuncUI.DSL
    open Examples.MusicPlayer

    type State =
        { length: int64
          sliderPos: int
          isPlaying: bool
          loopState: Types.LoopState }

    /// if you need to rise an event that needs to be consumed
    /// by an sibling control or a control that is not part of the hierachy (parent/child) of this control
    /// try to use an external message and handle it on the main module of your app
    type ExternalMsg =
        | Next
        | Previous
        | Play
        | Shuffle
        | SetLoopState of Types.LoopState

    type Msg =
        | Play of Types.SongRecord
        | Seek of double
        | SetPos of int64
        | SetLength of int64
        | SetLoopState of Types.LoopState
        | SetPlayState of bool
        | Previous
        | Pause
        | Stop
        | PlayInternal
        | Next
        | Shuffle

    let init =
        { length = 0L
          sliderPos = 0
          isPlaying = false
          loopState = Types.LoopState.Off }

    let update (msg: Msg) (state: State) (player: MediaPlayer) =
        match msg with
        | SetPlayState isPlaying ->
            { state with isPlaying = isPlaying }, Cmd.none, None
        | Play song ->
            use media = PlayerLib.getMediaFromlocal song.path
            player.Play media |> ignore
            let batch = Cmd.batch [ Cmd.ofMsg (SetLength player.Length); Cmd.ofMsg (SetPlayState true) ]
            state, batch, None
        | Seek position ->
            let time = (position |> int64) * player.Length / 100L
            (* find a way to differentiate from user action vs player event *)
            state, Cmd.none, None
        | SetLength length -> { state with length = length }, Cmd.none, None
        | SetPos position ->
            let pos = (position * 100L / player.Length) |> int
            { state with sliderPos = pos }, Cmd.none, None
        | SetLoopState loopState ->
            { state with loopState = loopState }, Cmd.none, Some(ExternalMsg.SetLoopState loopState)
        | Shuffle -> state, Cmd.none, Some ExternalMsg.Shuffle
        | Previous ->
            player.PreviousChapter()
            state, Cmd.none, Some ExternalMsg.Previous
        | Next ->
            player.NextChapter()
            state, Cmd.none, Some ExternalMsg.Next
        | Pause ->
            player.Pause()
            state, Cmd.ofMsg (SetPlayState false), None
        | Stop ->
            player.Stop()
            state, Cmd.ofMsg (SetPlayState false), None
        | PlayInternal -> state, Cmd.none, Some ExternalMsg.Play


    let private mediaButtons (state: State) (dispatch: Msg -> unit) =
        StackPanel.create [
            StackPanel.verticalAlignment VerticalAlignment.Bottom
            StackPanel.horizontalAlignment HorizontalAlignment.Left
            StackPanel.orientation Orientation.Horizontal
            StackPanel.dock Dock.Top
            StackPanel.children [
                if state.isPlaying then
                    Button.create [
                        Button.content Icons.previous
                        Button.classes [ "mediabtn" ]
                        Button.onClick (fun _ -> dispatch Previous)
                    ]
                    Button.create [
                        Button.content Icons.pause
                        Button.classes [ "mediabtn" ]
                        Button.onClick (fun _ -> dispatch Pause)
                    ]
                    Button.create [
                        Button.content Icons.stop
                        Button.classes [ "mediabtn" ]
                        Button.onClick (fun _ -> dispatch Stop)
                    ]
                    Button.create [
                        Button.content Icons.next
                        Button.classes [ "mediabtn" ]
                        Button.onClick (fun _ -> dispatch Next)
                    ]
                else
                    Button.create [
                        Button.content Icons.play
                        Button.classes [ "mediabtn" ]
                        Button.onClick (fun _ -> dispatch PlayInternal)
                    ]
                    Button.create [
                        Button.content Icons.shuffle
                        Button.classes [ "mediabtn" ]
                        Button.onClick (fun _ -> dispatch Shuffle)
                    ]
                match state.loopState with
                | Types.LoopState.All ->
                    Button.create [
                        Button.content Icons.repeat
                        Button.classes [ "mediabtn" ]
                        Button.onClick (fun _ -> dispatch (SetLoopState Types.LoopState.Single))
                    ]
                | Types.LoopState.Single ->
                    Button.create [
                        Button.content Icons.repeatOne
                        Button.classes [ "mediabtn" ]
                        Button.onClick (fun _ -> dispatch (SetLoopState Types.LoopState.Off))
                    ]
                | Types.LoopState.Off ->
                    Button.create [
                        Button.content Icons.repeatOff
                        Button.classes [ "mediabtn" ]
                        Button.onClick (fun _ -> dispatch (SetLoopState Types.LoopState.All))
                    ]
            ]
        ]

    let private progressBar (state: State) (dispatch: Msg -> unit) =
        StackPanel.create [
            StackPanel.verticalAlignment VerticalAlignment.Bottom
            StackPanel.horizontalAlignment HorizontalAlignment.Center
            StackPanel.orientation Orientation.Horizontal
            StackPanel.dock Dock.Bottom
            StackPanel.children [
                Slider.create [
                    Slider.minimum 0.0
                    Slider.maximum 100.0
                    Slider.width 428.0
                    Slider.horizontalAlignment HorizontalAlignment.Center
                    Slider.value (state.sliderPos |> double)
                    Slider.onValueChanged (fun value -> dispatch (Seek value))
                ]
            ]
        ]

    let view (state: State) (dispatch: Msg -> unit) =
        DockPanel.create [
            DockPanel.classes [ "mediabar" ]
            DockPanel.dock Dock.Bottom
            DockPanel.horizontalAlignment HorizontalAlignment.Center
            DockPanel.children [
                 progressBar state dispatch
                 mediaButtons state dispatch
            ]
        ]