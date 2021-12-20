namespace Examples.MusicPlayer


module Dialogs =
    open System
    open Avalonia.Controls

    let getMusicFilesDialog (filters: FileDialogFilter seq option) =
        let dialog = OpenFileDialog()

        let filters =
            match filters with
            | Some filter -> filter
            | None ->
                let filter = FileDialogFilter()
                filter.Extensions <-
                    Collections.Generic.List
                        (seq {
                            "mp3"
                            "wav" })
                filter.Name <- "Music"
                seq { filter }

        dialog.AllowMultiple <- true
        dialog.Directory <- Environment.GetFolderPath(Environment.SpecialFolder.MyMusic)
        dialog.Title <- "Select Your Music Files"
        dialog.Filters <- System.Collections.Generic.List(filters)
        dialog

    let getFolderDialog =
        let dialog = OpenFolderDialog()
        dialog.Directory <- Environment.GetFolderPath(Environment.SpecialFolder.MyMusic)
        dialog.Title <- "Choose where to look up for music"
        dialog
