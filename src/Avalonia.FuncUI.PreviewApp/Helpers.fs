namespace Avalonia.FuncUI.PreviewApp

open System
open Avalonia
open Avalonia.Controls
open Avalonia.Controls.ApplicationLifetimes
open Avalonia.Threading

module Helpers =

    let openDllPicker () =
        async {
            let filter = FileDialogFilter()
            filter.Name <- "Flink Document"
            filter.Extensions.Add "flink"

            let dialog = OpenFileDialog();
            dialog.Directory <- Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
            dialog.AllowMultiple <- false
            dialog.Filters.Add filter

            let window = (Application.Current.ApplicationLifetime :?> IClassicDesktopStyleApplicationLifetime).MainWindow

            let pathTask = task {
                return! Dispatcher.UIThread.InvokeAsync<string[]>(fun () -> task { return! dialog.ShowAsync(window) })
            }

            let! path = pathTask |> Async.AwaitTask

            match path with
            | null -> return None
            | _ ->
                match path.Length > 0 with
                | true -> return Some path.[0]
                | false -> return None
        }

