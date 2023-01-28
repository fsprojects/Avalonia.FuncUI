namespace Avalonia.FuncUI.DSL

[<AutoOpen>]
module RepeatButton =
    open Avalonia.Controls
    open Avalonia.FuncUI.Types
    open Avalonia.FuncUI.Builder

    let create (attrs: Attr<RepeatButton> list): IView<RepeatButton> =
        ViewBuilder.Create<RepeatButton>(attrs)

    type RepeatButton with

        /// <summary>
        /// Sets the amount of time, in milliseconds, of repeating clicks.
        /// </summary>
        static member interval<'t when 't :> RepeatButton>(value: int) : Attr<'t> =
            AttrBuilder<'t>.CreateProperty<int>(RepeatButton.IntervalProperty, value, ValueNone)

        /// <summary>
        /// Sets the amount of time, in milliseconds, to wait before repeating begins.
        /// </summary>
        static member delay<'t when 't :> RepeatButton>(value: int) : Attr<'t> =
            AttrBuilder<'t>.CreateProperty<int>(RepeatButton.DelayProperty, value, ValueNone)