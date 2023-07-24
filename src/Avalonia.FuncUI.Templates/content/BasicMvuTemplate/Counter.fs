
namespace BasicMvuTemplate

module Counter =
    open Avalonia.Controls
    open Avalonia.FuncUI
    open Avalonia.FuncUI.DSL
    open Avalonia.Layout
    
    type State = { count : int }
    let init() = { count = 0 }
d
    type Msg = Increment | Decrement | Reset
s
    let update (msg: Msg) (state: State) : State =
        match msg with
        | Increment -> { state with count = state.count + 1 }
        | Decrement -> { state with count = state.count - 1 }
        | Reset -> init()

    let buttonStyles: Types.IAttr<Button> list =  [
        Button.width 64
        Button.fontSize 16.0
        Button.horizontalAlignment HorizontalAlignment.Center
        Button.horizontalContentAlignment HorizontalAlignment.Center
        Button.margin 2.
    ]
    
    let view (state: State) (dispatch) =
        DockPanel.create [
            DockPanel.children [
                Button.create [
                    Button.dock Dock.Bottom
                    Button.onClick (fun _ -> dispatch Reset)
                    Button.content "reset"
                    yield! buttonStyles
                ]                
                Button.create [
                    Button.dock Dock.Bottom
                    Button.onClick (fun _ -> dispatch Decrement)
                    Button.content "-"
                    Button.horizontalAlignment HorizontalAlignment.Center
                    yield! buttonStyles
                ]
                Button.create [
                    Button.dock Dock.Bottom
                    Button.horizontalAlignment HorizontalAlignment.Center
                    Button.onClick (fun _ -> dispatch Increment)
                    Button.content "+"
                    yield! buttonStyles
                ]
                TextBlock.create [
                    TextBlock.dock Dock.Top
                    TextBlock.fontSize 48.0
                    TextBlock.verticalAlignment VerticalAlignment.Center
                    TextBlock.horizontalAlignment HorizontalAlignment.Center
                    TextBlock.text (string state.count)
                ]
            ]
        ]