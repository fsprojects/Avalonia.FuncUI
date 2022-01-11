namespace Examples.Component.MusicPlayer

[<RequireQualifiedAccess>]
module Media =
    open LibVLCSharp.Shared

    let private libVlc = lazy (new LibVLC())

    let Player = lazy (new MediaPlayer(libVlc.Value))
    
    let Play (file: string) =
        use media = new Media(libVlc.Value, file, FromType.FromPath)
        Player.Value.Play(media)
