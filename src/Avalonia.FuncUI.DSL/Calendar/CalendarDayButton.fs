namespace Avalonia.FuncUI.DSL.Primitives


[<AutoOpen>]
module CalendarDayButton =
    open Avalonia.Controls.Primitives
    open Avalonia.FuncUI.Types
    open Avalonia.FuncUI.Builder
    
    let create (attrs: IAttr<CalendarDayButton> list): IView<CalendarDayButton> =
        ViewBuilder.Create<CalendarDayButton>(attrs)
     
    type CalendarDayButton with
        end
