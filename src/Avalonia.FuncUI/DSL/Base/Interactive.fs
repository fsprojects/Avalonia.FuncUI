namespace Avalonia.FuncUI.DSL

open System

open Avalonia
open Avalonia.Interactivity
open Avalonia.Controls
open Avalonia.Layout
open Avalonia.VisualTree
open Avalonia.Reactive

[<AutoOpen>]
module Interactive =
    open Avalonia.FuncUI
    open Avalonia.FuncUI.Types
    open Avalonia.FuncUI.Builder
    open Avalonia.Data
    let create (attrs: IAttr<Interactive> list): IView<Interactive> =
        ViewBuilder.Create<Interactive>(attrs)

    type Interactive with
        
        /// <summary>
        /// Create a Routed Event subscription as attached event.
        /// </summary>
        /// 
        /// <param name="routedEvent">The routed event to subscribe to.</param>
        /// <param name="func">The function to call when the event is raised. Arguments are current control(sender) and routed event args.</param>
        /// <param name="subPatchOptions">The subscription patch options.</param>
        static member attachedEvent<'t when 't :> Interactive>(routedEvent:RoutedEvent, func, ?subPatchOptions) : IAttr<'t> =
            let factory: SubscriptionFactory<'t * RoutedEventArgs> =
                fun (control, func, token) ->
                    let control = control :?> 't
                    let handler = EventHandler<RoutedEventArgs>(fun s e -> func (s :?> 't, e))
                    
                    control.AddHandler(routedEvent, handler, routedEvent.RoutingStrategies)
                    token.Register(fun () -> control.RemoveHandler(routedEvent, handler)) |> ignore

            let name = $"{nameof<'t>}.{routedEvent.Name}"
            AttrBuilder<'t>.CreateSubscription<'t * RoutedEventArgs>(name, factory, func, ?subPatchOptions = subPatchOptions)

        /// <summary>
        /// Create a Routed Event subscription as attached event.
        /// </summary>
        /// 
        /// <param name="routedEvent">The routed event to subscribe to.</param>
        /// <param name="func">The function to call when the event is raised. Arguments are current control(sender) and routed event args.</param>
        /// <param name="subPatchOptions">The subscription patch options.</param>
        static member attachedEvent<'t , 'arg when 't :> Interactive and 'arg :> RoutedEventArgs>(routedEvent: RoutedEvent<'arg>, func, ?subPatchOptions) : IAttr<'t> =
            let factory: SubscriptionFactory<'t * 'arg> =
                fun (control, func, token) ->
                    let control = control :?> 't                    
                    let handler = EventHandler<'arg>(fun s e -> func (s :?> 't, e))

                    control.AddHandler<'arg>(routedEvent, handler, routedEvent.RoutingStrategies)
                    token.Register(fun () -> control.RemoveHandler(routedEvent, handler)) |> ignore

            let name = $"{nameof<'t>}.{routedEvent.Name}"
            AttrBuilder<'t>.CreateSubscription<'t * 'arg>(name, factory, func, ?subPatchOptions = subPatchOptions)