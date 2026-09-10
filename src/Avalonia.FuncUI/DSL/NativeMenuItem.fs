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

        static member menu<'t when 't :> NativeMenuItem>(value: IView<NativeMenu> option) : IAttr<'t> =
            AttrBuilder<'t>.CreateContentSingle(NativeMenuItem.MenuProperty, value |> Option.map (fun menu -> menu :> IView))

        static member menu<'t when 't :> NativeMenuItem>(value: IView<NativeMenu>) : IAttr<'t> =
            NativeMenuItem.menu(Some value)

        static member icon<'t when 't :> NativeMenuItem>(value: Avalonia.Media.Imaging.Bitmap) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty(NativeMenuItem.IconProperty, value, ValueNone)

        static member gesture<'t when 't :> NativeMenuItem>(value: KeyGesture) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty(NativeMenuItem.GestureProperty, value, ValueNone)

        static member toggleType<'t when 't :> NativeMenuItem>(value: MenuItemToggleType) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty(NativeMenuItem.ToggleTypeProperty, value, ValueNone)

        static member isVisible<'t when 't :> NativeMenuItem>(value: bool) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty(NativeMenuItem.IsVisibleProperty, value, ValueNone)

        static member toolTip<'t when 't :> NativeMenuItem>(value: string) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty(NativeMenuItem.ToolTipProperty, value, ValueNone)

        static member onClick<'t when 't :> NativeMenuItem>(func: System.EventArgs -> unit, ?subPatchOptions: SubPatchOptions) : IAttr<'t> =
            let factory: SubscriptionFactory<System.EventArgs> = fun (target, handler, token) ->
                let subscription = (target :?> NativeMenuItem).Click.Subscribe handler
                token.Register(fun () -> subscription.Dispose()) |> ignore
            let name = nameof Unchecked.defaultof<'t>.Click
            AttrBuilder<'t>.CreateSubscription(name, factory, func, ?subPatchOptions = subPatchOptions)

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