namespace Avalonia.FuncUI.DSL

open Avalonia
open Avalonia.Controls.Documents
open Avalonia.Controls.Templates
open Avalonia.Data
open Avalonia.Media
open Avalonia.Media.Immutable
open Avalonia.Styling
open Avalonia.Controls
open Avalonia.FuncUI.Types
open Avalonia.FuncUI.Builder

[<AutoOpen>]
module DataGrid =

    let create (attrs: IAttr<DataGrid> list): IView<DataGrid> =
        ViewBuilder.Create<DataGrid>(attrs)

    type DataGrid with

        static member isReadOnly<'t when 't :> DataGrid>(readOnly: bool) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<bool>(DataGrid.IsReadOnlyProperty, readOnly, ValueNone)

        static member autoGeneratedColumns<'t when 't :> DataGrid>(autoGenerate: bool) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<bool>(DataGrid.AutoGenerateColumnsProperty, autoGenerate, ValueNone)

        static member items (items: #System.Collections.IEnumerable) : IAttr<DataGrid> =
            AttrBuilder<DataGrid>.CreateProperty(DataGrid.ItemsSourceProperty, items, ValueNone)

        static member canUserReorderColumns<'t when 't :> DataGrid>(value: bool) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<bool>(DataGrid.CanUserReorderColumnsProperty, value, ValueNone)

        static member canUserResizeColumns<'t when 't :> DataGrid>(value: bool) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<bool>(DataGrid.CanUserResizeColumnsProperty, value, ValueNone)

        static member canUserSortColumns<'t when 't :> DataGrid>(value: bool) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<bool>(DataGrid.CanUserSortColumnsProperty, value, ValueNone)

        static member columns (columns: IView list) : IAttr<'t> =
            AttrBuilder<'t>.CreateContentMultiple(
                name = "Columns",
                getter = ValueSome (fun (dataGrid: #DataGrid) -> dataGrid.Columns :> obj),
                setter = ValueNone,
                multipleContent = columns
            )

[<AutoOpen>]
module DataGridColumn =

    let create (attrs: IAttr<DataGridColumn> list): IView<DataGridColumn> =
        ViewBuilder.Create<DataGridColumn>(attrs)

    type DataGridColumn with

        static member header<'t when 't :> DataGridColumn>(text: string) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<string>(DataGridColumn.HeaderProperty, text, ValueNone)

        static member header<'t when 't :> DataGridColumn>(view: IView option) : IAttr<'t> =
            AttrBuilder<'t>.CreateContentSingle(DataGridColumn.HeaderProperty, view)

        static member header<'t when 't :> DataGridColumn>(view: IView) : IAttr<'t> =
            DataGridColumn.header (Some view)

        static member isVisible<'t when 't :> DataGridColumn>(value: bool) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<bool>(DataGridColumn.IsVisibleProperty, value, ValueNone)

        static member cellTheme<'t when 't :> DataGridColumn>(value: ControlTheme) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<ControlTheme>(DataGridColumn.CellThemeProperty, value, ValueNone)

        static member width<'t when 't :> DataGridColumn>(value: DataGridLength) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<DataGridLength>(
                name = "width",
                value = value,
                getter = ValueSome (fun column -> column.Width),
                setter = ValueSome (fun (column, value) -> column.Width <- value),
                comparer = ValueNone
            )

[<AutoOpen>]
module DataGridBoundColumn =

    type DataGridBoundColumn with

        static member binding<'t when 't :> DataGridBoundColumn>(binding: IBinding) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<IBinding>(
                name = "Binding",
                value = binding,
                getter = ValueSome (fun column -> column.Binding),
                setter = ValueSome (fun (column, value) -> column.Binding <- value),
                comparer = ValueNone
            )


[<AutoOpen>]
module DataGridTextColumn =

    let create (attrs: IAttr<DataGridTextColumn> list): IView<DataGridTextColumn> =
        ViewBuilder.Create<DataGridTextColumn>(attrs)

    type DataGridTextColumn with

        static member fontFamily<'t when 't :> DataGridTextColumn>(value: FontFamily) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<FontFamily>(DataGridTextColumn.FontFamilyProperty, value, ValueNone)

        static member fontSize<'t when 't :> DataGridTextColumn>(value: double) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<double>(DataGridTextColumn.FontSizeProperty, value, ValueNone)

        static member fontStyle<'t when 't :> DataGridTextColumn>(value: FontStyle) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<FontStyle>(DataGridTextColumn.FontStyleProperty, value, ValueNone)

        static member fontWeight<'t when 't :> DataGridTextColumn>(value: FontWeight) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<FontWeight>(DataGridTextColumn.FontWeightProperty, value, ValueNone)

        static member foreground<'t when 't :> DataGridTextColumn>(value: IBrush) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<IBrush>(DataGridTextColumn.ForegroundProperty, value, ValueNone)

        static member foreground<'t when 't :> DataGridTextColumn>(color: string) : IAttr<'t> =
            color |> Color.Parse |> ImmutableSolidColorBrush |> DataGridTextColumn.foreground

        static member foreground<'t when 't :> DataGridTextColumn>(color: Color) : IAttr<'t> =
            color |> ImmutableSolidColorBrush |> DataGridTextColumn.foreground


[<AutoOpen>]
module DataGridCheckBoxColumn =

    let create (attrs: IAttr<DataGridCheckBoxColumn> list): IView<DataGridCheckBoxColumn> =
        ViewBuilder.Create<DataGridCheckBoxColumn>(attrs)

[<AutoOpen>]
module DataGridTemplateColumn =

    let create (attrs: IAttr<DataGridTemplateColumn> list): IView<DataGridTemplateColumn> =
        ViewBuilder.Create<DataGridTemplateColumn>(attrs)

    type DataGridTemplateColumn with

        static member cellTemplate<'t when 't :> DataGridTemplateColumn>(template: IDataTemplate) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<IDataTemplate>(DataGridTemplateColumn.CellTemplateProperty, template, ValueNone)

        static member cellEditingTemplate<'t when 't :> DataGridTemplateColumn>(template: IDataTemplate) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<IDataTemplate>(DataGridTemplateColumn.CellEditingTemplateProperty, template, ValueNone)