namespace Avalonia.FuncUI.DSL

open System
open Avalonia.Controls.Primitives
open Avalonia.Input

[<AutoOpen>]
module CalendarDayButton =
    open Avalonia.FuncUI.Types
    open Avalonia.FuncUI.Builder

    let create (attrs: IAttr<CalendarDayButton> list): IView<CalendarDayButton> =
        ViewBuilder.Create<CalendarDayButton>(attrs)
    
    type CalendarDayButton with

        /// <summary>
        /// Occurs when the left mouse button is pressed (or when the tip of the
        /// stylus touches the tablet PC) while the mouse pointer is over a
        /// UIElement.
        /// </summary>
        /// 
        /// <remarks>Because of CalendarDayButton is sealed, this binding is only for CalendarDayButton</remarks>
        static member onCalendarDayButtonMouseDown(func: PointerPressedEventArgs -> unit, ?subPatchOptions) : IAttr<CalendarDayButton> =
            let name = nameof Unchecked.defaultof<CalendarDayButton>.CalendarDayButtonMouseDown
            let factory: SubscriptionFactory<PointerPressedEventArgs> =
                fun (control, func, token) ->
                    let control = control :?> CalendarDayButton
                    let handler = EventHandler<PointerPressedEventArgs>(fun s e -> func e)
                    let event = control.PointerPressed

                    event.AddHandler(handler)
                    token.Register(fun () -> event.RemoveHandler(handler)) |> ignore
            
            AttrBuilder<CalendarDayButton>.CreateSubscription(name, factory, func, ?subPatchOptions = subPatchOptions)

        /// <summary>
        /// Occurs when the left mouse button is released (or the tip of the
        /// stylus is removed from the tablet PC) while the mouse (or the
        /// stylus) is over a UIElement (or while a UIElement holds mouse
        /// capture).
        /// </summary>
        /// 
        /// <remarks>Because of CalendarDayButton is sealed, this binding is only for CalendarDayButton</remarks>
        static member onCalendarDayButtonMouseUp(func: PointerReleasedEventArgs -> unit, ?subPatchOptions) : IAttr<CalendarDayButton> =
            let name = nameof Unchecked.defaultof<CalendarDayButton>.CalendarDayButtonMouseUp
            let factory: SubscriptionFactory<PointerReleasedEventArgs> =
                fun (control, func, token) ->
                    let control = control :?> CalendarDayButton
                    let handler = EventHandler<PointerReleasedEventArgs>(fun s e -> func e)
                    let event = control.PointerReleased

                    event.AddHandler(handler)
                    token.Register(fun () -> event.RemoveHandler(handler)) |> ignore
            
            AttrBuilder<CalendarDayButton>.CreateSubscription(name, factory, func, ?subPatchOptions = subPatchOptions)
