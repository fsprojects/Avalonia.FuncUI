namespace Examples.MusicPlayer


module Types =
    open System

    type SongRecord =
        { id: Guid
          name: string
          path: string
          createdAt: DateTime }

    type LoopState =
        | Off
        | All
        | Single
