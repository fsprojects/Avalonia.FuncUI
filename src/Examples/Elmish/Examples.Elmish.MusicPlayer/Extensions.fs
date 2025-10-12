namespace Examples.MusicPlayer


[<AutoOpen>]
module Extensions =
    open System
    open Avalonia.Controls
    open Avalonia.Media.Imaging
    open Avalonia.Platform

    type Bitmap with
        static member Create(s: string): Bitmap =
            let uri =
                if s.StartsWith("/") then Uri(s, UriKind.Relative) else Uri(s, UriKind.RelativeOrAbsolute)

            if uri.IsAbsoluteUri && uri.IsFile then
                new Bitmap(uri.LocalPath)
            else
                new Bitmap(AssetLoader.Open(uri))

    type Image with
        static member FromString(s: string): Image =
            let img = Image()
            img.Source <- Bitmap.Create s
            img
