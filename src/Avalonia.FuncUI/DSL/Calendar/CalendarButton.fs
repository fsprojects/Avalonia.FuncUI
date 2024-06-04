namespace Avalonia.FuncUI.DSL

open System
open Avalonia.Controls.Primitives
open Avalonia.Input

[<AutoOpen>]
module CalendarButton =
    open Avalonia.FuncUI.Types
    open Avalonia.FuncUI.Builder

    let create (attrs: IAttr<CalendarButton> list): IView<CalendarButton> =
        ViewBuilder.Create<CalendarButton>(attrs)
    
    type CalendarButton with
        
        /// <summary>
        /// Occurs when the left mouse button is pressed (or when the tip of the
        /// stylus touches the tablet PC) while the mouse pointer is over a
        /// UIElement.
        /// </summary>
        /// 
        /// <remarks>Because of CalendarButton is sealed, this binding is only for CalendarButton</remarks>
        static member onCalendarLeftMouseButtonDown(func: PointerPressedEventArgs -> unit, ?subPatchOptions) : IAttr<CalendarButton> =
            let name = nameof Unchecked.defaultof<CalendarButton>.CalendarLeftMouseButtonDown
            let factory: SubscriptionFactory<PointerPressedEventArgs> =
                fun (control, func, token) ->
                    let control = control :?> CalendarButton
                    let handler = EventHandler<PointerPressedEventArgs>(fun s e -> func e)
                    let event = control.CalendarLeftMouseButtonDown

                    event.AddHandler(handler)
                    token.Register(fun () -> event.RemoveHandler(handler)) |> ignore
            
            AttrBuilder<CalendarButton>.CreateSubscription(name, factory, func, ?subPatchOptions = subPatchOptions)

        /// <summary>
        /// Occurs when the left mouse button is released (or the tip of the
        /// stylus is removed from the tablet PC) while the mouse (or the
        /// stylus) is over a UIElement (or while a UIElement holds mouse
        /// capture).
        /// </summary>
        /// 
        /// <remarks>Because of CalendarButton is sealed, this binding is only for CalendarButton</remarks>
        static member onCalendarLeftMouseButtonUp(func: PointerReleasedEventArgs -> unit, ?subPatchOptions) : IAttr<CalendarButton> =
            let name = nameof Unchecked.defaultof<CalendarButton>.CalendarLeftMouseButtonUp
            let factory: SubscriptionFactory<PointerReleasedEventArgs> =
                fun (control, func, token) ->
                    let control = control :?> CalendarButton
                    let handler = EventHandler<PointerReleasedEventArgs>(fun s e -> func e)
                    let event = control.CalendarLeftMouseButtonUp

                    event.AddHandler(handler)
                    token.Register(fun () -> event.RemoveHandler(handler)) |> ignore
            
            AttrBuilder<CalendarButton>.CreateSubscription(name, factory, func, ?subPatchOptions = subPatchOptions)
