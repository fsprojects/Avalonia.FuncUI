namespace Avalonia.FuncUI.DSL.Primitives


[<AutoOpen>]
module CalendarButton =
    open Avalonia.Controls.Primitives
    open Avalonia.FuncUI.Types
    open Avalonia.FuncUI.Builder
    
    let create (attrs: IAttr<CalendarButton> list): IView<CalendarButton> =
        ViewBuilder.Create<CalendarButton>(attrs)
     
    type CalendarButton with
        end
