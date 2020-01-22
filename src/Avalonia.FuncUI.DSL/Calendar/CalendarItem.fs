namespace Avalonia.FuncUI.DSL.Primitives


[<AutoOpen>]
module CalendarItem =
    open Avalonia.Controls.Primitives
    open Avalonia.Media
    open Avalonia.FuncUI.Types
    open Avalonia.FuncUI.Builder
    
    let create (attrs: IAttr<CalendarItem> list): IView<CalendarItem> =
        ViewBuilder.Create<CalendarItem>(attrs)
     
    type CalendarItem with
        end