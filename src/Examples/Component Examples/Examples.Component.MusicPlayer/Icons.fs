namespace Examples.Component.MusicPlayer

/// Material Design Icons (mdi) offer you a XAML based canvas code which
/// you can take and add the specific icons you need :) to this module
/// these icons were taken from https://materialdesignicons.com/

module Icons =
    open Avalonia.Controls
    open Avalonia.Controls.Shapes
    open Avalonia.FuncUI.DSL
    open Avalonia.FuncUI
    open Avalonia.FuncUI.Types
    open Avalonia.Layout
    open Avalonia.FuncUI.Components.Hosts
    open Avalonia.FuncUI.DSL

    [<Literal>]
    let private shuffle = "M17,3L22.25,7.5L17,12L22.25,16.5L17,21V18H14.26L11.44,15.18L13.56,13.06L15.5,15H17V12L17,9H15.5L6.5,18H2V15H5.26L14.26,6H17V3M2,6H6.5L9.32,8.82L7.2,10.94L5.26,9H2V6Z"
    [<Literal>]
    let private repeat = "M17,17H7V14L3,18L7,22V19H19V13H17M7,7H17V10L21,6L17,2V5H5V11H7V7Z"
    [<Literal>]
    let private repeatOne = "M13,15V9H12L10,10V11H11.5V15M17,17H7V14L3,18L7,22V19H19V13H17M7,7H17V10L21,6L17,2V5H5V11H7V7Z"
    [<Literal>]
    let private repeatOff = "M2,5.27L3.28,4L20,20.72L18.73,22L15.73,19H7V22L3,18L7,14V17H13.73L7,10.27V11H5V8.27L2,5.27M17,13H19V17.18L17,15.18V13M17,5V2L21,6L17,10V7H8.82L6.82,5H17Z"
    [<Literal>]
    let private stop = "M18,18H6V6H18V18Z"
    [<Literal>]
    let private play = "M8,5.14V19.14L19,12.14L8,5.14Z"
    [<Literal>]
    let private pause = "M14,19H18V5H14M6,19H10V5H6V19Z"
    [<Literal>]
    let private previous = "M6,18V6H8V18H6M9.5,12L18,6V18L9.5,12Z"
    [<Literal>]
    let private next = "M16,18H18V6H16M6,18L14.5,12L6,6V18Z"
    [<Literal>]
    let private folderMultiple = "M22,4H14L12,2H6A2,2 0 0,0 4,4V16A2,2 0 0,0 6,18H22A2,2 0 0,0 24,16V6A2,2 0 0,0 22,4M2,6H0V11H0V20A2,2 0 0,0 2,22H20V20H2V6Z"
    [<Literal>]
    let private folderMusic = "M10 4L12 6H20C21.1 6 22 6.89 22 8V18C22 19.1 21.1 20 20 20H4C2.89 20 2 19.1 2 18L2 6C2 4.89 2.89 4 4 4H10M19 9H15.5V13.06L15 13C13.9 13 13 13.9 13 15C13 16.11 13.9 17 15 17C16.11 17 17 16.11 17 15V11H19V9Z"

    type Icon = 
        | Shuffle
        | Repeat
        | RepeatOne
        | RepeatOff
        | Stop
        | Play
        | Pause
        | Previous
        | Next
        | FolderMultiple
        | FolderMusic
        
        member this.AsData : string = 
            match this with 
            | Shuffle -> shuffle
            | Repeat -> repeat
            | RepeatOne -> repeatOne
            | RepeatOff -> repeatOff
            | Stop -> stop
            | Play -> play
            | Pause -> pause
            | Previous -> previous
            | Next -> next
            | FolderMultiple -> folderMultiple
            | FolderMusic -> folderMusic

    type Icons() =
        static member Icon (icon: Icon, ?fill: string) =
            let fill = defaultArg fill "black"
            Component.create("mp-icon", fun _ ->
                Canvas.create [
                    Canvas.width 24.0
                    Canvas.height 24.0
                    Canvas.children [
                        Path.create [
                            Path.fill fill
                            Path.data icon.AsData
                        ]
                    ]
                ])
            
