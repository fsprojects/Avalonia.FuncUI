namespace Avalonia.FuncUI.SmallSampleApp

open Avalonia.FuncUI.Builders

module Views =

    [<RequireQualifiedAccess>]
    module Counter =
        open Avalonia.FuncUI.Core
        open Avalonia.Controls
        open Avalonia.Media
        
        type State = {
            current : int
        }

        let view (state: State) : IViewElement =
            button {
                background (SolidColorBrush(Colors.Green))
                contentView (textblock { text "Test Text" })
                fontSize (if true then 12.0 else 24.0)
            }

        let init = {
            current = 0
        }
        
