namespace Avalonia.FuncUI.DSL

[<AutoOpen>]
module Image =
    open Avalonia.Controls
    open Avalonia.FuncUI.Builder
    open Avalonia.FuncUI.Types
    open Avalonia.Media

    let create (attrs: Attr<Image> list): View<Image> =
        ViewBuilder.Create<Image>(attrs)

    type Image with
        static member source<'t when 't :> Image>(value: IImage) : Attr<'t> =
            // TODO: maybe add custom bitmap comparer OR pass enum that has different options ?
            AttrBuilder<'t>.CreateProperty<IImage>(Image.SourceProperty, value, ValueNone)

        static member stretch<'t when 't :> Image>(value: Stretch) : Attr<'t> =
            AttrBuilder<'t>.CreateProperty<Stretch>(Image.StretchProperty, value, ValueNone)