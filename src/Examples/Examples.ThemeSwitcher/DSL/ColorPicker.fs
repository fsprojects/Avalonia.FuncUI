namespace Examples.ThemeSwitcher.DSL

open Avalonia.Controls
open Avalonia.FuncUI.Types

module ColorPicker =
    open Avalonia.FuncUI.DSL

    let create (attrs: IAttr<ColorPicker> list) : IView<ColorPicker> = ViewBuilder.Create<ColorPicker> (attrs)

[<AutoOpen>]
module ColorPickerDSL =
    open Avalonia.FuncUI.Builder
    open Avalonia.Controls.Templates
    open Avalonia.Layout

    type ColorPicker with

        /// <summary>
        /// Defines the `ColorPicker.Content` property.
        /// </summary>
        static member content<'t when 't :> ColorPicker> (value:obj)=
            AttrBuilder<'t>.CreateProperty<obj>(ColorPicker.ContentProperty, value, ValueNone)

        /// <summary>
        /// Defines the `ColorPicker.Content` property.
        /// </summary>
        static member content<'t when 't :> ColorPicker> (view:IView)=
            AttrBuilder<'t>.CreateContentSingle(ColorPicker.ContentProperty,Some view)

        /// <summary>
        /// Defines the `ColorPicker.ContentTemplate` property.
        /// </summary>
        static member contentTemplate<'t when 't :> ColorPicker> (value:IDataTemplate)=
            AttrBuilder<'t>.CreateProperty<IDataTemplate>(ColorPicker.ContentTemplateProperty, value, ValueNone)

        /// <summary>
        /// Defines the `ColorPicker.HorizontalContentAlignment` property.
        /// </summary>
        static member horizontalContentAlignment<'t when 't :> ColorPicker> (value:HorizontalAlignment)=
            AttrBuilder<'t>.CreateProperty<HorizontalAlignment>(ColorPicker.HorizontalContentAlignmentProperty, value, ValueNone)

        /// <summary>
        /// Defines the `ColorPicker.VerticalContentAlignment` property.
        /// </summary>
        static member verticalContentAlignment<'t when 't :> ColorPicker> (value:VerticalAlignment)=
            AttrBuilder<'t>.CreateProperty<VerticalAlignment>(ColorPicker.VerticalContentAlignmentProperty, value, ValueNone)