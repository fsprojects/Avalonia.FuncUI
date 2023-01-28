namespace Avalonia.FuncUI.DSL

[<AutoOpen>]
module Panel =
    open Avalonia.Controls
    open Avalonia.FuncUI.Types
    open Avalonia.FuncUI.Builder
    open Avalonia.Media.Immutable
    open Avalonia.Media

    type Panel with

        static member children<'t when 't :> Panel>(value: IView list) : Attr<'t> =
            let getter : ('t -> obj) = (fun control -> control.Children :> obj)

            AttrBuilder<'t>.CreateContentMultiple("Children", ValueSome getter, ValueNone, value)

        static member background<'t when 't :> Panel>(value: IBrush) : Attr<'t> =
            AttrBuilder<'t>.CreateProperty<IBrush>(Panel.BackgroundProperty, value, ValueNone)

        static member background<'t when 't :> Panel>(color: string) : Attr<'t> =
            color |> Color.Parse |> ImmutableSolidColorBrush |> Panel.background