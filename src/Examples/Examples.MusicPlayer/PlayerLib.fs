namespace Examples.MusicPlayer

/// Dedicated module to the LibCLVSharp interaciont
/// in this case we're only looking to play music
/// but you should be able to include any video/broadcasting (chromecast)
/// code in here
module PlayerLib =
    open LibVLCSharp.Shared

    let getMediaFromlocal (source: string) =
        use libvlc = new LibVLC()
        new Media(libvlc, source, FromType.FromPath)

    let getEmptyPlayer =
        use libvlc = new LibVLC()
        new MediaPlayer(libvlc)
