namespace Avalonia.FuncUI.DSL
open Avalonia.Input
open Avalonia.Interactivity
open System.Windows.Input

[<AutoOpen>]
module NativeMenuItem =
    open Avalonia.Controls
    open Avalonia.FuncUI.Types
    open Avalonia.FuncUI.Builder

    let create (attrs: IAttr<NativeMenuItem> list): IView<NativeMenuItem> =
        ViewBuilder.Create<NativeMenuItem>(attrs)

    type NativeMenuItem with

        static member command<'t when 't :> NativeMenuItem>(command: ICommand) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<ICommand>(NativeMenuItem.CommandProperty, command, ValueNone)

        static member commandParameter<'t when 't :> NativeMenuItem>(parameter: obj) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<obj>(NativeMenuItem.CommandParameterProperty, parameter, ValueNone)

        static member header<'t when 't :> NativeMenuItem>(header: string) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<string>(NativeMenuItem.HeaderProperty, header, ValueNone)

        static member isChecked<'t when 't :> NativeMenuItem>(value: bool) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<bool>(NativeMenuItem.IsCheckedProperty, value, ValueNone)

        static member isEnabled<'t when 't :> NativeMenuItem>(value: bool) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<bool>(NativeMenuItem.IsEnabledProperty, value, ValueNone)