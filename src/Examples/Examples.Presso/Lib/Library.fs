namespace Examples.Presso

open Avalonia

[<AutoOpen>]
module Extensions =
    open System
    open Avalonia.Media.Imaging
    open Avalonia.Platform
    
    type Bitmap with
        static member Create (s: string) : Bitmap =
            let uri =
                if s.StartsWith("/")
                then Uri(s, UriKind.Relative)
                else Uri(s, UriKind.RelativeOrAbsolute);

            if uri.IsAbsoluteUri && uri.IsFile then
                new Bitmap(uri.LocalPath)
            else
                let assets = AvaloniaLocator.Current.GetService<IAssetLoader>();
                new Bitmap(assets.Open(uri));
