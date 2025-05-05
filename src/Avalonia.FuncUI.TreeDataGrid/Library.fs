namespace Avalonia.FuncUI.DSL

open Avalonia.Controls
open Avalonia.FuncUI.Types
open Avalonia.FuncUI.Builder
open Avalonia.Controls.Models.TreeDataGrid
open Avalonia.Controls.Primitives
open System

[<AutoOpen>]
module TreeDataGrid =

    let create (attrs: IAttr<TreeDataGrid> list): IView<TreeDataGrid> =
        ViewBuilder.Create<TreeDataGrid>(attrs)

    type TreeDataGrid with

        static member autoDragDropRows<'t when 't :> TreeDataGrid>(autoDragDropRows: bool) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<bool>(TreeDataGrid.AutoDragDropRowsProperty, autoDragDropRows, ValueNone)

        static member canUserResizeColumns<'t when 't :> TreeDataGrid>(canUserResizeColumns: bool) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<bool>(TreeDataGrid.CanUserResizeColumnsProperty, canUserResizeColumns, ValueNone)

        static member canUserSortColumns<'t when 't :> TreeDataGrid>(canUserSortColumns: bool) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<bool>(TreeDataGrid.CanUserSortColumnsProperty, canUserSortColumns, ValueNone)

        static member columns<'t when 't :> TreeDataGrid>(columns: IColumns) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<IColumns>(TreeDataGrid.ColumnsProperty, columns, ValueNone)

        static member elementFactory<'t when 't :> TreeDataGrid>(factory: TreeDataGridElementFactory) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<TreeDataGridElementFactory>(TreeDataGrid.ElementFactoryProperty, factory, ValueNone)

        static member rows<'t when 't :> TreeDataGrid>(rows: IRows) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<IRows>(TreeDataGrid.RowsProperty, rows, ValueNone)

        static member scroll<'t when 't :> TreeDataGrid>(scrollable: IScrollable) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<IScrollable>(TreeDataGrid.ScrollProperty, scrollable, ValueNone)

        static member showColumnHeaders<'t when 't :> TreeDataGrid>(showHeaders: bool) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<bool>(TreeDataGrid.ShowColumnHeadersProperty, showHeaders, ValueNone)

        static member source<'t when 't :> TreeDataGrid>(dataSource: ITreeDataGridSource) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<ITreeDataGridSource>(TreeDataGrid.SourceProperty, dataSource, ValueNone)

        static member rowDragStarted<'t when 't :> TreeDataGrid>(func: TreeDataGridRowDragStartedEventArgs -> unit, ?subPatchOptions) =
            AttrBuilder<'t>.CreateSubscription<TreeDataGridRowDragStartedEventArgs>(TreeDataGrid.RowDragStartedEvent, func, ?subPatchOptions = subPatchOptions)

        static member rowDragOver<'t when 't :> TreeDataGrid>(func: TreeDataGridRowDragEventArgs -> unit, ?subPatchOptions) =
            AttrBuilder<'t>.CreateSubscription<TreeDataGridRowDragEventArgs>(TreeDataGrid.RowDragOverEvent, func, ?subPatchOptions = subPatchOptions)

        static member rowDrop<'t when 't :> TreeDataGrid>(func: TreeDataGridRowDragEventArgs -> unit, ?subPatchOptions) =
            AttrBuilder<'t>.CreateSubscription<TreeDataGridRowDragEventArgs>(TreeDataGrid.RowDropEvent, func, ?subPatchOptions = subPatchOptions)

