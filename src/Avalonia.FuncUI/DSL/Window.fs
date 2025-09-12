namespace Avalonia.FuncUI.DSL

[<AutoOpen>]
module Window =
    open Avalonia.Controls
    open Avalonia.FuncUI.Types
    open Avalonia.FuncUI.Builder

    let create (attrs: IAttr<Window> list) : IView<Window> =
        ViewBuilder.Create<Window>(attrs)

    type Window with
        static member child(value: IView) : IAttr<Window> =
            AttrBuilder<Window>.CreateContentSingle(Window.ContentProperty, Some value)

        static member title<'t when 't :> Window>(value: string) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<string>(Window.TitleProperty, value, ValueNone)

        static member canResize<'t when 't :> Window>(value: bool) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<bool>(Window.CanResizeProperty, value, ValueNone)
            
        static member sizeToContent<'t when 't :> Window>(value: SizeToContent) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<SizeToContent>(Window.SizeToContentProperty, value, ValueNone)

        static member windowState<'t when 't :> Window>(value: WindowState) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<WindowState>(Window.WindowStateProperty, value, ValueNone)
