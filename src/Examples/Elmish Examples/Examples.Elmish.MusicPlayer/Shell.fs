namespace Examples.MusicPlayer

module Shell =
    open System
    open Elmish
    open Avalonia
    open Avalonia.Controls
    open Avalonia.Input
    open Avalonia.Layout
    open Avalonia.FuncUI.Elmish
    open Avalonia.FuncUI.Hosts
    open Avalonia.FuncUI.DSL
    open LibVLCSharp.Shared
    open Examples.MusicPlayer

    type State =
        { title: string
          playerState: Player.State
          playlistState: Playlist.State }

    type Msg =
        | PlayerMsg of Player.Msg
        | PlaylistMsg of Playlist.Msg
        | SetTitle of string
        | OpenFiles
        | OpenFolder
        | AfterSelectFolder of string
        | AfterSelectFiles of string array
        (* Handle Media Player Events *)
        | Playing
        | Paused
        | Stopped
        | Ended
        | TimeChanged of int64
        | ChapterChanged of int
        | LengthChanged of int64

    module Subs =
        let registerSubscriptions (player: MediaPlayer) (state: State) =
            let playing (player: MediaPlayer) dispatch =
                player.Playing.Subscribe(fun _ -> dispatch Playing)

            let paused (player: MediaPlayer) dispatch =
                player.Paused.Subscribe(fun _ -> dispatch Paused)

            let stopped (player: MediaPlayer) dispatch =
                player.Stopped.Subscribe(fun _ -> dispatch Stopped)

            let ended (player: MediaPlayer) dispatch =
                player.EndReached.Subscribe(fun _ -> dispatch Ended)

            let timeChanged (player: MediaPlayer) dispatch =
                player.TimeChanged.Subscribe(fun args -> dispatch (TimeChanged args.Time))

            let chapterChanged (player: MediaPlayer) dispatch =
                player.ChapterChanged.Subscribe(fun args -> dispatch (ChapterChanged args.Chapter))

            let lengthChanged (player: MediaPlayer) dispatch =
                player.LengthChanged.Subscribe(fun args -> dispatch (LengthChanged args.Length))

            [
                [ nameof playing ], playing player
                [ nameof paused ], paused player
                [ nameof stopped ], stopped player
                [ nameof ended ], ended player
                [ nameof timeChanged ], timeChanged player
                [ nameof chapterChanged ], chapterChanged player
                [ nameof lengthChanged ], lengthChanged player
            ]


    let init () =
        { title = "Music Player in F# :)"
          playerState = Player.init
          playlistState = Playlist.init }, Cmd.none

    (* here we use these functions to handle which kind of actions we need to take
       in case one of our children sends an external message 
       In this case Playlist and Player need to be communicating with each other
       but they are not parent/child controls they are more akin to sibling controls.
       That does not limit which interactions can be done from a message, for example
       when selecting a song from the playlist we tell the player wich song we need
       to play as well as telling the Shell module to update the window's Title property *)
    
    let private handlePlaylistExternal (msg: Playlist.ExternalMsg option) =
        match msg with
        | None -> Cmd.none
        | Some msg ->
            match msg with
            | Playlist.ExternalMsg.PlaySong(int, song) ->
                Cmd.batch [
                    Cmd.ofMsg (PlayerMsg(Player.Msg.Play song))
                    Cmd.ofMsg (SetTitle song.name)
                ]

    let private handlePlayerExternal (msg: Player.ExternalMsg option) =
        match msg with
        | None -> Cmd.none
        | Some msg ->
            match msg with
            | Player.ExternalMsg.Play -> Cmd.ofMsg (PlaylistMsg(Playlist.Msg.GetAny))
            | Player.ExternalMsg.Next -> Cmd.ofMsg (PlaylistMsg(Playlist.Msg.GetNext))
            | Player.ExternalMsg.Previous -> Cmd.ofMsg (PlaylistMsg(Playlist.Msg.GetPrevious))
            | Player.ExternalMsg.Shuffle -> Cmd.ofMsg (PlaylistMsg(Playlist.Msg.Shuffle))
            | Player.ExternalMsg.SetLoopState loopstate -> Cmd.ofMsg (PlaylistMsg(Playlist.Msg.SetLoopState loopstate))

    let update (window: HostWindow) (player: MediaPlayer) (msg: Msg) (state: State) =
        match msg with
        | PlayerMsg playermsg ->
            let s, cmd, external = Player.update playermsg state.playerState player
            let handled = handlePlayerExternal external
            let mapped = Cmd.map PlayerMsg cmd
            let batch = Cmd.batch [ mapped; handled ]
            { state with playerState = s }, batch
        | PlaylistMsg playlistmsg ->
            let s, cmd, external = Playlist.update playlistmsg state.playlistState
            let mapped = Cmd.map PlaylistMsg cmd
            let handled = handlePlaylistExternal external
            let batch = Cmd.batch [ mapped; handled ]
            { state with playlistState = s }, batch
        | SetTitle title ->
            window.Title <- title
            { state with title = title }, Cmd.none
        | OpenFiles ->
            let dialog = Dialogs.getMusicFilesDialog None
            let showDialog window = dialog.ShowAsync(window) |> Async.AwaitTask
            state, Cmd.OfAsync.perform showDialog window AfterSelectFiles
        | OpenFolder ->
            let dialog = Dialogs.getFolderDialog
            let showDialog window = dialog.ShowAsync(window) |> Async.AwaitTask
            state, Cmd.OfAsync.perform showDialog window AfterSelectFolder
        | AfterSelectFolder path ->
            let songs = Songs.populateFromDirectory path |> Array.toList
            state, Cmd.map PlaylistMsg (Cmd.ofMsg (Playlist.Msg.AddFiles songs))
        | AfterSelectFiles paths ->
            let songs = Songs.populateSongs paths |> Array.toList

            state, Cmd.map PlaylistMsg (Cmd.ofMsg (Playlist.Msg.AddFiles songs))
        (* The follwing messages are fired from the player's subscriptions
           I feel these are can help to handle updates accross the whole application
           There are a lot more of events the Player Emits, but for the moment
           we'll work with these *)
        | Playing -> state, Cmd.none
        | Paused -> state, Cmd.none
        | Stopped -> state, Cmd.none
        | Ended -> state, Cmd.map PlaylistMsg (Cmd.ofMsg (Playlist.Msg.GetNext))
        | TimeChanged time -> state, Cmd.map PlayerMsg (Cmd.ofMsg (Player.Msg.SetPos time))
        | ChapterChanged chapter -> state, Cmd.none
        | LengthChanged length -> state, Cmd.none

    let menuBar state dispatch =
        Menu.create [
            Menu.dock Dock.Top
            Menu.viewItems [
                MenuItem.create [
                    MenuItem.header "Files"
                    MenuItem.viewItems [
                        MenuItem.create [
                            MenuItem.header "Select Files"
                            MenuItem.icon (Image.FromString "avares://Examples.Elmish.MusicPlayer/Assets/Icons/file-multiple-dark.png")
                            MenuItem.onClick (fun _ -> dispatch OpenFiles)
                        ]
                        MenuItem.create [
                            MenuItem.header "Select Folder"
                            MenuItem.icon (Image.FromString "avares://Examples.Elmish.MusicPlayer/Assets/Icons/folder-music-dark.png")
                            MenuItem.onClick (fun _ -> dispatch OpenFolder)
                        ]
                    ]
                ]
            ]
        ]

    let view (state: State) (dispatch: Msg -> unit) =
        DockPanel.create [
            DockPanel.verticalAlignment VerticalAlignment.Stretch
            DockPanel.horizontalAlignment HorizontalAlignment.Stretch
            DockPanel.lastChildFill false
            DockPanel.children [
                menuBar state dispatch
                Playlist.view state.playlistState (PlaylistMsg >> dispatch)
                Player.view state.playerState (PlayerMsg >> dispatch)
            ]
        ]

    type ShellWindow() as this =
        inherit HostWindow()
        do
            let iconPath = System.IO.Path.Combine("Assets", "Icons", "icon.ico")

            base.Title <- "Music Player in F# :)"
            base.Icon <- WindowIcon(iconPath)
            base.Width <- 800.0
            base.Height <- 600.0
            base.MinWidth <- 526.0
            base.MinHeight <- 526.0
            this.SystemDecorations <- SystemDecorations.Full
            

            //this.VisualRoot.VisualRoot.Renderer.DrawFps <- true
            //this.VisualRoot.VisualRoot.Renderer.DrawDirtyRects <- true
            let player = PlayerLib.getEmptyPlayer
#if DEBUG
            this.AttachDevTools(KeyGesture(Key.F12))
#endif
            Program.mkProgram init (update this player) view
            |> Program.withHost this
            |> Program.withSubscription (Subs.registerSubscriptions player)
#if DEBUG
            |> Program.withConsoleTrace
#endif
            |> Program.runWithAvaloniaSyncDispatch ()
