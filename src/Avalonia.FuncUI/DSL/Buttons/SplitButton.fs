namespace Avalonia.FuncUI.DSL

[<AutoOpen>]
module SplitButton =
    open System.Windows.Input
    open Avalonia.Controls
    open Avalonia.Interactivity
    open Avalonia.FuncUI.Builder
    open Avalonia.FuncUI.Types

    let create (attrs: IAttr<SplitButton> list): IView<SplitButton> =
        ViewBuilder.Create<SplitButton>(attrs)

    type SplitButton with

        static member command<'t when 't :> SplitButton>(value: ICommand) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<ICommand>(SplitButton.CommandProperty, value, ValueNone)

        static member commandParameter<'t when 't :> SplitButton>(value: obj) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<obj>(SplitButton.CommandParameterProperty, value, ValueNone)

        static member onClick<'t when 't :> SplitButton>(func: RoutedEventArgs -> unit, ?subPatchOptions) =
            AttrBuilder<'t>.CreateSubscription<RoutedEventArgs>(SplitButton.ClickEvent, func, ?subPatchOptions = subPatchOptions)

        static member flyout<'t when 't :> SplitButton>(value: IView option) : IAttr<'t> =
            AttrBuilder<'t>.CreateContentSingle(SplitButton.FlyoutProperty, value)

        static member flyout<'t when 't :> SplitButton>(value: IView) : IAttr<'t> =
            value
            |> Some
            |> SplitButton.flyout
