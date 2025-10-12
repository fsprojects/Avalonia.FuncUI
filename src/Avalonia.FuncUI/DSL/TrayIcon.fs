namespace Avalonia.FuncUI.DSL
open Avalonia.Input
open Avalonia.Interactivity
open System.Windows.Input

[<AutoOpen>]
module TrayIcon =
    open Avalonia.Controls
    open Avalonia.FuncUI.Builder
    open Avalonia.FuncUI.Types
     
    let create (attrs: IAttr<TrayIcon> list): IView<TrayIcon> =
        ViewBuilder.Create<TrayIcon>(attrs)
     
    type TrayIcon with
        static member command<'t when 't :> TrayIcon>(command: ICommand) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<ICommand>(TrayIcon.CommandProperty, command, ValueNone)

        static member commandParameter<'t when 't :> TrayIcon>(parameter: obj) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<obj>(TrayIcon.CommandParameterProperty, parameter, ValueNone)        

        static member isVisible<'t when 't :> TrayIcon>(isVisible: bool) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<bool>(TrayIcon.IsVisibleProperty, isVisible, ValueNone)

        static member toolTipText<'t when 't :> TrayIcon>(toolTipText: string) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<string>(TrayIcon.ToolTipTextProperty, toolTipText, ValueNone)

        static member menu<'t when 't :> TrayIcon>(menu: IView option) : IAttr<'t> =
            AttrBuilder<'t>.CreateContentSingle(TrayIcon.MenuProperty, menu)

        static member menu<'t when 't :> TrayIcon>(value: IView) : IAttr<'t> =
            value
            |> Some
            |> TrayIcon.menu
