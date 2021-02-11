namespace Avalonia.FuncUI.DSL

[<AutoOpen>]
module DataGrid =
    open System.Collections
    open Avalonia.Controls
    open Avalonia.Controls.Primitives
    open Avalonia.Controls.Templates
    open Avalonia.Media
    open Avalonia.Media.Immutable
    open Avalonia.FuncUI.Builder
    open Avalonia.FuncUI.Types

    let create (attrs: IAttr<DataGrid> list): IView<DataGrid> =
        ViewBuilder.Create<DataGrid>(attrs)

    type DataGrid with

        static member canUserReorderColumns<'t when 't :> DataGrid>(value: bool) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<bool>(DataGrid.CanUserReorderColumnsProperty, value, ValueNone)

        static member canUserResizeColumns<'t when 't :> DataGrid>(value: bool) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<bool>(DataGrid.CanUserResizeColumnsProperty, value, ValueNone)

        static member canUserSortColumns<'t when 't :> DataGrid>(value: bool) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<bool>(DataGrid.CanUserSortColumnsProperty, value, ValueNone)

        static member columnHeaderHeight<'t when 't :> DataGrid>(value: float) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<float>(DataGrid.ColumnHeaderHeightProperty, value, ValueNone)

        static member columnWidth<'t when 't :> DataGrid>(value: DataGridLength) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<DataGridLength>(DataGrid.ColumnWidthProperty , value, ValueNone)

        static member columnWidth<'t when 't :> DataGrid>(value: float) : IAttr<'t> =
            DataGridLength(value) |> DataGrid.columnWidth

        static member alternatingRowBackground<'t when 't :> DataGrid>(brush: IBrush) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<IBrush>(DataGrid.AlternatingRowBackgroundProperty, brush, ValueNone)

        static member alternatingRowBackground<'t when 't :> DataGrid>(color: string) : IAttr<'t> =
            color |> Color.Parse |> ImmutableSolidColorBrush |> DataGrid.alternatingRowBackground

        static member frozenColumnCount<'t when 't :> DataGrid>(value: int) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<int>(DataGrid.FrozenColumnCountProperty, value, ValueNone)

        static member gridLinesVisibility<'t when 't :> DataGrid>(value: DataGridGridLinesVisibility) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<DataGridGridLinesVisibility>(DataGrid.GridLinesVisibilityProperty, value, ValueNone)

        static member headersVisibility<'t when 't :> DataGrid>(value: DataGridHeadersVisibility) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<DataGridHeadersVisibility>(DataGrid.HeadersVisibilityProperty, value, ValueNone)

        static member horizontalGridLinesBrush<'t when 't :> DataGrid>(brush: IBrush) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<IBrush>(DataGrid.HorizontalGridLinesBrushProperty, brush, ValueNone)

        static member horizontalScrollBarVisibility<'t when 't :> DataGrid>(visibility: ScrollBarVisibility) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<ScrollBarVisibility>(DataGrid.HorizontalScrollBarVisibilityProperty, visibility, ValueNone)

        static member isReadOnly<'t when 't :> DataGrid>(value: bool) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<bool>(DataGrid.IsReadOnlyProperty, value, ValueNone)

        static member areRowGroupHeadersFrozen<'t when 't :> DataGrid>(value: bool) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<bool>(DataGrid.AreRowGroupHeadersFrozenProperty, value, ValueNone)

        static member isValid<'t when 't :> DataGrid>(value: bool) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<bool>(DataGrid.IsValidProperty, value, ValueNone)

        static member maxColumnWidth<'t when 't :> DataGrid>(value: double) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<double>(DataGrid.MaxColumnWidthProperty, value, ValueNone)

        static member minColumnWidth<'t when 't :> DataGrid>(value: double) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<double>(DataGrid.MinColumnWidthProperty, value, ValueNone)

        static member rowBackground<'t when 't :> DataGrid>(brush: IBrush) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<IBrush>(DataGrid.RowBackgroundProperty, brush, ValueNone)

        static member rowBackground<'t when 't :> DataGrid>(color: string) : IAttr<'t> =
            color |> Color.Parse |> ImmutableSolidColorBrush |> DataGrid.rowBackground

        static member rowHeight<'t when 't :> DataGrid>(value: double) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<double>(DataGrid.RowHeightProperty, value, ValueNone)

        static member rowHeaderWidth<'t when 't :> DataGrid>(value: double) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<double>(DataGrid.RowHeaderWidthProperty, value, ValueNone)

        static member selectionMode<'t when 't :> DataGrid>(value: DataGridSelectionMode) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<DataGridSelectionMode>(DataGrid.SelectionModeProperty, value, ValueNone)

        static member verticalGridLinesBrush<'t when 't :> DataGrid>(brush: IBrush) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<IBrush>(DataGrid.VerticalGridLinesBrushProperty, brush, ValueNone)

        static member verticalGridLinesBrush<'t when 't :> DataGrid>(color: string) : IAttr<'t> =
            color |> Color.Parse |> ImmutableSolidColorBrush |> DataGrid.verticalGridLinesBrush

        static member verticalScrollBarVisibility<'t when 't :> DataGrid>(visibility: ScrollBarVisibility) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<ScrollBarVisibility>(DataGrid.VerticalScrollBarVisibilityProperty, visibility, ValueNone)

        static member dropLocationIndicatorTemplate<'t when 't :> DataGrid>(template: ITemplate<IControl>) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<ITemplate<IControl>>(DataGrid.DropLocationIndicatorTemplateProperty, template, ValueNone)

        static member selectedIndex<'t when 't :> DataGrid>(value: int) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<int>(DataGrid.SelectedIndexProperty, value, ValueNone)

        static member selectedItem<'t when 't :> DataGrid>(value: obj) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<obj>(DataGrid.SelectedItemProperty, value, ValueNone)

        static member clipboardCopyMode<'t when 't :> DataGrid>(value: DataGridClipboardCopyMode) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<DataGridClipboardCopyMode>(DataGrid.ClipboardCopyModeProperty, value, ValueNone)

        static member autoGenerateColumns<'t when 't :> DataGrid>(value: bool) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<bool>(DataGrid.AutoGenerateColumnsProperty, value, ValueNone)

        static member items<'t when 't :> DataGrid>(value: IEnumerable) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<IEnumerable>(DataGrid.ItemsProperty, value, ValueNone)

        static member areRowDetailsFrozen<'t when 't :> DataGrid>(value: bool) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<bool>(DataGrid.AreRowDetailsFrozenProperty, value, ValueNone)

        static member rowDetailsTemplate<'t when 't :> DataGrid>(value: IDataTemplate) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<IDataTemplate>(DataGrid.RowDetailsTemplateProperty, value, ValueNone)

        static member rowDetailsVisibilityMode<'t when 't :> DataGrid>(value: DataGridRowDetailsVisibilityMode) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<DataGridRowDetailsVisibilityMode>(DataGrid.RowDetailsVisibilityModeProperty, value, ValueNone)

        static member onSelectionChanged<'t when 't :> DataGrid>(func: SelectionChangedEventArgs -> unit, ?subPatchOptions) =
            AttrBuilder<'t>.CreateSubscription<SelectionChangedEventArgs>(DataGrid.SelectionChangedEvent, func, ?subPatchOptions = subPatchOptions)