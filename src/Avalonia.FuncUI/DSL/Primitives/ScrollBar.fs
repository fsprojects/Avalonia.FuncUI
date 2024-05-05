namespace Avalonia.FuncUI.DSL

[<AutoOpen>]
module ScrollBar =
    open System
    open Avalonia.Layout
    open Avalonia.Controls.Primitives
    open Avalonia.FuncUI.Types
    open Avalonia.FuncUI.Builder

    let create (attrs: IAttr<ScrollBar> list): IView<ScrollBar> =
        ViewBuilder.Create<ScrollBar>(attrs)

    type ScrollBar with
        static member onScroll<'t when 't :> ScrollBar>(func: ScrollEventArgs -> unit, ?subPatchOptions) : IAttr<'t> =
            let name = nameof Unchecked.defaultof<'t>.Scroll
            let factory: SubscriptionFactory<ScrollEventArgs> =
                fun (control, func, token) ->
                    let content = control :?> 't
                    let handler = EventHandler<ScrollEventArgs>(fun sender args -> func args)
                    let event = content.Scroll

                    event.AddHandler(handler)
                    token.Register(fun () -> event.RemoveHandler(handler)) |> ignore

            AttrBuilder<'t>.CreateSubscription<ScrollEventArgs>(name, factory, func, ?subPatchOptions = subPatchOptions)

        /// <summary>
        /// Gets a value that indicates whether the scrollbar can hide itself when user is not interacting with it.
        /// </summary>
        static member allowAutoHide<'t when 't :> ScrollBar>(value: bool) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<bool>(ScrollBar.AllowAutoHideProperty, value, ValueNone)

        /// <summary>
        /// Gets a value that determines how long will be the hide delay after user stops interacting with the scrollbar.
        /// </summary>
        static member hideDelay<'t when 't :> ScrollBar>(value: TimeSpan) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<TimeSpan>(ScrollBar.HideDelayProperty, value, ValueNone)

        /// <summary>
        /// Gets a value that indicates whether the scrollbar is expanded.
        /// </summary>
        static member isExpanded<'t when 't :> ScrollBar>(value: bool) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<bool>(ScrollBar.IsExpandedProperty, value, ValueNone)

        /// <summary>
        /// Sets the amount of the scrollable content that is currently visible.
        /// </summary>
        static member viewportSize<'t when 't :> ScrollBar>(value: double) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<double>(ScrollBar.ViewportSizeProperty, value, ValueNone)

        /// <summary>
        /// Sets a value that indicates whether the scrollbar should hide itself when it is not needed.
        /// </summary>
        static member visibility<'t when 't :> ScrollBar>(visibility: ScrollBarVisibility) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<ScrollBarVisibility>(ScrollBar.VisibilityProperty, visibility, ValueNone)

        /// <summary>
        /// Sets the orientation of the scrollbar.
        /// </summary>
        static member orientation<'t when 't :> ScrollBar>(orientation: Orientation) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<Orientation>(ScrollBar.OrientationProperty, orientation, ValueNone)

        /// <summary>
        /// Gets a value that determines how long will be the show delay when user starts interacting with the scrollbar.
        /// </summary>
        static member showDelay<'t when 't :> ScrollBar>(value: TimeSpan) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<TimeSpan>(ScrollBar.ShowDelayProperty, value, ValueNone)
