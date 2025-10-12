namespace Examples.MusicPlayer

module Songs =
    open System
    open System.IO
    open Types
    open Avalonia.Platform.Storage

    let populateSongs (paths: IStorageFile seq): Types.SongRecord array =
        paths
        |> Seq.map (fun storageFile ->
            { id = Guid.NewGuid()
              name = storageFile.Name
              path = storageFile.Path.LocalPath
              createdAt = DateTime.Now })
        |> Array.ofSeq

    let populateFromDirectory (path: string): Types.SongRecord array =
        match String.IsNullOrEmpty path with
        | true -> Array.empty
        | false ->
            let dirinfo = DirectoryInfo path
            dirinfo.GetFiles()
            |> Array.filter (fun info -> info.Extension = ".mp3" || info.Extension = ".wav")
            |> Array.Parallel.map (fun info -> info.Name, info.FullName)
            |> Array.Parallel.map (fun (name, path) ->
                { id = Guid.NewGuid()
                  name = name
                  path = path
                  createdAt = DateTime.Now })

    let populateFromFolders(folders: IStorageFolder seq) =
        folders
        |> Seq.map _.Path.LocalPath
        |> Seq.map populateFromDirectory
        |> Array.concat