namespace Examples.Component.MusicPlayer

open System
open System.IO
open Avalonia.Controls
open Avalonia.FuncUI
open Avalonia.FuncUI.Components
open Avalonia.FuncUI.Types
open Avalonia.Input
open Avalonia.Layout
open Avalonia.FuncUI.Components.Hosts
open Avalonia.FuncUI.DSL

open type Icons.Icons

type Song =
    { filename: string
      path: string; }

type MediaState =
    | Stop
    | Play of Song option
        
type LoopState =
    | Off
    | Single
    | All


type PlayerState =
    { song: Song option
      state: MediaState
      loop: LoopState }

[<Struct>]
type PlayerActions =
    { onNext: unit -> unit
      onPrevious: unit -> unit
      onShuffle: unit -> unit
      onLoopState: LoopState -> unit
      onStateChange: MediaState -> unit }

[<RequireQualifiedAccess>]
module Menubar =
    let View onOpenFiles onOpenDirectory =
        let _view (ctx: IComponentContext) : IView =
            ctx.attrs [Border.dock Dock.Top]
            Menu.create [
                Menu.viewItems [
                    MenuItem.create [
                        MenuItem.header "Files"
                        MenuItem.viewItems [
                            MenuItem.create [
                                MenuItem.header "Select Files"
                                MenuItem.icon (Icon(Icons.FolderMultiple, "white"))
                                MenuItem.onClick (fun _ -> onOpenFiles())
                            ]
                            MenuItem.create [
                                MenuItem.header "Select Folder"
                                MenuItem.icon (Icon(Icons.FolderMusic, "white"))
                                MenuItem.onClick (fun _ -> onOpenDirectory())
                            ]
                        ]
                    ]
                ]
            ]
        Component.create("menubar", _view)

[<RequireQualifiedAccess>]
module Playlist =
    let View (playlist: IReadable<Song[]>) (current: IReadable<Song option>) onRequestSong =
        let _view (ctx: IComponentContext) : IView =
            ctx.attrs [ Border.dock Dock.Top ]
            let song = ctx.usePassedRead current
            let playlist = ctx.usePassedRead playlist
            StackPanel.create [
                StackPanel.dock Dock.Top
                StackPanel.children [
                    ItemsControl.create [
                        ItemsControl.dataItems playlist.Current
                        ItemsControl.itemTemplate (
                            DataTemplateView.create<_,_> (fun (item: Song) ->
                                TextBlock.create [
                                    TextBlock.classes ["plitem"]
                                    TextBlock.text item.filename
                                    TextBlock.onDoubleTapped(fun _ -> onRequestSong item)
                                    TextBlock.onKeyUp(fun event ->
                                        match event.Key, event.KeyModifiers with
                                        | Key.Enter, KeyModifiers.Control ->
                                            onRequestSong item
                                        | _ -> ())
                                ])
                        )
                    ]
                ]
            ]
        Component.create("playlist", _view)

[<RequireQualifiedAccess>]
module Player =

    let View (playState: IReadable<MediaState>) (loop: IReadable<LoopState>) (actions: PlayerActions) =
        Component.create("player", fun ctx ->
            let playState = ctx.usePassedRead playState
            let loop = ctx.usePassedRead loop
            ctx.attrs [Border.dock Dock.Bottom]
                
            DockPanel.create [
                DockPanel.classes [ "mediabar" ]
                DockPanel.horizontalAlignment HorizontalAlignment.Center
                DockPanel.children [
                    StackPanel.create [
                        StackPanel.verticalAlignment VerticalAlignment.Bottom
                        StackPanel.horizontalAlignment HorizontalAlignment.Left
                        StackPanel.orientation Orientation.Horizontal
                        StackPanel.dock Dock.Top
                        StackPanel.children [
                            match playState.Current with
                            | Play _ ->
                                Button.create [
                                    Button.content (Icon(Icons.Previous))
                                    Button.classes [ "mediabtn" ]
                                    Button.onClick (fun _ -> actions.onPrevious())
                                ]
                                Button.create [
                                    Button.content (Icon(Icons.Stop))
                                    Button.classes [ "mediabtn" ]
                                    Button.onClick (fun _ -> actions.onStateChange Stop) ]
                                Button.create [
                                    Button.content (Icon(Icons.Next))
                                    Button.classes [ "mediabtn" ]
                                    Button.onClick (fun _ -> actions.onNext())
                                ]
                            | Stop ->
                                Button.create [
                                    Button.content (Icon(Icons.Play))
                                    Button.classes [ "mediabtn" ]
                                    Button.onClick (fun _ -> actions.onStateChange (Play None))
                                ]
                            Button.create [
                                Button.content (Icon(Icons.Shuffle))
                                Button.classes [ "mediabtn" ]
                                Button.onClick (fun _ -> actions.onShuffle())
                            ]
                            match loop.Current with
                            | All ->
                                Button.create [
                                    Button.content (Icon(Icons.Repeat))
                                    Button.classes [ "mediabtn" ]
                                    Button.onClick (fun _ -> actions.onLoopState loop.Current)
                                ]
                            | Single ->
                                Button.create [
                                    Button.content (Icon(Icons.RepeatOne))
                                    Button.classes [ "mediabtn" ]
                                    Button.onClick (fun _ -> actions.onLoopState loop.Current)
                                ]
                            | Off ->
                                Button.create [
                                    Button.content ((Icon(Icons.RepeatOff)))
                                    Button.classes [ "mediabtn" ]
                                    Button.onClick (fun _ -> actions.onLoopState loop.Current)
                                ]
                        ]
                    ]
                ]
            ])

module Shell =
    let private shuffle (org: _ array) =
        let rng = Random.Shared
        let arr = Array.copy org
        let max = (arr.Length - 1)

        let randomSwap (arr: _ []) i =
            let pos = rng.Next(max)
            let tmp = arr.[pos]
            arr.[pos] <- arr.[i]
            arr.[i] <- tmp
            arr

        [| 0 .. max |]
        |> Array.fold randomSwap arr

    let private View (window: Window) =

        let _view (ctx: IComponentContext) : IView =
            let _playlist = ctx.useState [||]
            
            let playerState = ctx.useState ({ song = None; loop = Off; state = Stop }, true)

            let playState = playerState |> State.readMap(fun s -> s.state)
            let loop = playerState |> State.readMap(fun s -> s.loop)
            let song = playerState |> State.readMap(fun s -> s.song)
            let playlist = _playlist |> State.readMap id
            
            let onPrevious() =
                match song.Current with
                | None ->
                    playerState.Set { playerState.Current with  song = playlist.Current |> Array.tryHead }
                | Some song ->
                    match playlist.Current |> Array.tryFindIndex (fun s -> s = song) with
                    | Some index ->
                        if index + 1 > playlist.Current.Length - 1 then
                            playerState.Set { playerState.Current with song = Some playlist.Current[0] }
                        else
                            playerState.Set { playerState.Current with song = Some playlist.Current[index + 1] }
                    | None -> playerState.Set { playerState.Current with song = Some playlist.Current[0] }
               
            let onNext() =
                match song.Current with
                | None ->
                    playerState.Set { playerState.Current with  song = playlist.Current |> Array.tryHead }
                | Some song ->
                    match playlist.Current |> Array.tryFindIndex (fun s -> s = song) with
                    | Some index ->
                        match loop.Current with
                        | Off ->
                            playerState.Set { playerState.Current with song = None }
                        | Single ->
                            playerState.Set { playerState.Current with song = Some playlist.Current[index] }
                        | All ->
                            if index - 1 > 0 then
                                playerState.Set { playerState.Current with song = Some playlist.Current[playlist.Current.Length - 1] }
                            else
                                playerState.Set { playerState.Current with song = Some playlist.Current[index - 1] }
                    | None -> playerState.Set { playerState.Current with song = Some playlist.Current[0] }

            let onLoopState (state: LoopState) =
                let state  = 
                    match state with
                    | Off -> All
                    | All -> Single
                    | Single -> Off
                
                playerState.Set { playerState.Current with loop = state }

            let onShuffle() =
                playlist.Current |> shuffle |> _playlist.Set

            let onPlay (song: Song) =
                if Media.Play song.path then
                    playerState.Set { playerState.Current with song = Some song; state = Play (Some song) }

            let onStop() =
                Media.Player.Value.Stop()
                playerState.Set { playerState.Current with state = Stop; }
            
            let onRequestSong (song: Song) =
                onPlay song

            let onSelectFiles () =
                let dialog = OpenFileDialog()
                let filter = FileDialogFilter()
                filter.Extensions <- ResizeArray(["mp3"; "wav"])
                filter.Name <- "Music"
                dialog.AllowMultiple <- true
                dialog.Directory <- Environment.GetFolderPath(Environment.SpecialFolder.MyMusic)
                dialog.Title <- "Select Your Music Files"
                dialog.Filters.Add(filter)
                task {
                    let! result = dialog.ShowAsync(window)
                    result
                    |> Array.Parallel.map(fun r -> { filename = FileInfo(r).Name; path = r })
                    |> _playlist.Set
                } |> Async.AwaitTask |> Async.Start

            let onSelectDirectory() =
                let dialog = OpenFolderDialog()
                dialog.Directory <- Environment.GetFolderPath(Environment.SpecialFolder.MyMusic)
                dialog.Title <- "Choose where to look up for music"
                task {
                    let! directory = dialog.ShowAsync(window)
                    [|
                        yield! Directory.EnumerateFiles(directory, "*.mp3")
                        yield! Directory.EnumerateFiles(directory, "*.wav")
                    |]
                    |> Array.Parallel.map(fun r -> { filename = FileInfo(r).Name; path = r })
                    |> _playlist.Set
                } |> Async.AwaitTask |> Async.Start
            
            let onStateChange (state: MediaState) =
                match state with
                | Play (Some song) -> onPlay song
                | Play None ->
                    playlist.Current
                    |> Array.tryHead
                    |> Option.map(onPlay)
                    |> ignore
                | Stop -> onStop()
                
            ctx.useEffect(
                handler = (fun _ ->
                    let disposables = [
                        Media.Player.Value.Stopped.Subscribe(fun _ -> onNext())
                        Media.Player.Value.EndReached.Subscribe(fun _ -> onNext())
                    ]
                    { new IDisposable with
                        member _.Dispose() = disposables |> List.iter (fun i -> i.Dispose()) }),
                triggers = [EffectTrigger.AfterInit]
                )

            let playerActions =
                { onNext = onNext;
                  onPrevious = onPrevious
                  onShuffle = onShuffle;
                  onLoopState = onLoopState;
                  onStateChange = onStateChange }
            
            DockPanel.create [
                DockPanel.verticalAlignment VerticalAlignment.Stretch
                DockPanel.horizontalAlignment HorizontalAlignment.Stretch
                DockPanel.lastChildFill false
                DockPanel.children [
                    Menubar.View onSelectFiles onSelectDirectory
                    Playlist.View playlist song onRequestSong
                    Player.View playState loop playerActions
                ]
            ]

        Component _view

    type ShellWindow() as this =
        inherit HostWindow()
        do
            base.Title <- "Music Player in F# :)"
            base.Width <- 800.0
            base.Height <- 600.0
            base.MinWidth <- 526.0
            base.MinHeight <- 526.0
            this.SystemDecorations <- SystemDecorations.Full
            this.Content <- View this
