namespace Avalonia.FuncUI.DSL

[<AutoOpen>]
module RemoteWidget =
    open Avalonia.Controls.Remote
    open Avalonia.Remote.Protocol

    open Avalonia.FuncUI.Types
    open Avalonia.FuncUI.Builder

    let create (connection:IAvaloniaRemoteTransportConnection) (attrs: IAttr<RemoteWidget> list): IView<RemoteWidget> =
        ViewBuilder.Create<RemoteWidget>(attrs)
        |> View.withConstructorArgs [|connection|]

    type RemoteWidget with
        static member mode<'t when 't :> RemoteWidget>(value: RemoteWidget.SizingMode) : IAttr<'t> =
            let name = nameof Unchecked.defaultof<'t>.Mode
            let getter: 't -> RemoteWidget.SizingMode = fun x -> x.Mode
            let setter: 't * RemoteWidget.SizingMode -> unit = fun (x, v) -> x.Mode <- v

            AttrBuilder<'t>.CreateProperty(name, value, ValueSome getter, ValueSome setter, ValueNone)
