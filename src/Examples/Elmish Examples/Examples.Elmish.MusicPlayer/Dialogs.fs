namespace Examples.MusicPlayer

module Dialogs =
    open Avalonia
    open Avalonia.Platform.Storage
    open Avalonia.Threading
    open System.Collections.Generic

    let showMusicFilesDialog(provider: IStorageProvider, filters: FilePickerFileType list option) =

        let filters =
            match filters with
            | Some filter -> filter
            | None ->
                let patterns = [ "*.mp3"; "*.wav" ]
                let filter = FilePickerFileType("Music", Patterns = patterns)
                [ filter ]

        let options = FilePickerOpenOptions(AllowMultiple = true, Title = "Select Your Music Files", FileTypeFilter = filters)

        async {
            let! musicFolder = provider.TryGetWellKnownFolderAsync Platform.Storage.WellKnownFolder.Music |> Async.AwaitTask
            options.SuggestedStartLocation <- musicFolder

            return!
                Dispatcher.UIThread.InvokeAsync<IReadOnlyList<IStorageFile>>
                    (fun _ -> provider.OpenFilePickerAsync(options)) |> Async.AwaitTask
        }

    let showMusicFolderDialog(provider: IStorageProvider) =
        async {
            let! musicFolder = provider.TryGetWellKnownFolderAsync Platform.Storage.WellKnownFolder.Music |> Async.AwaitTask
            let options = FolderPickerOpenOptions(Title = "Choose where to look up for music", SuggestedStartLocation = musicFolder)
            
            return!
                Dispatcher.UIThread.InvokeAsync<IReadOnlyList<IStorageFolder>>
                    (fun _ -> provider.OpenFolderPickerAsync(options)) |> Async.AwaitTask

        }
