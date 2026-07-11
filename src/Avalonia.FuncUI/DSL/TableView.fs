namespace Avalonia.FuncUI.DSL

[<AutoOpen>]
module TableView =
    open Avalonia.Controls
    open Avalonia.FuncUI.Types
    open Avalonia.FuncUI.Builder
    open Avalonia.FuncUI
    
    let create (attrs: IAttr<TableView> list): IView<TableView> =
        ViewBuilder.Create<TableView>(attrs)
     
    type TableView with

        static member canUserResizeColumns<'t when 't :> TableView>(canUserResizeColumns : bool) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<bool>(TableView.CanUserResizeColumnsProperty, canUserResizeColumns, ValueNone)

        static member columns(columns: Types.IView<TableViewColumn> list) : IAttr<'t> =
            let getter = fun (tableView: #TableView) -> tableView.Columns :> obj
            let tableColumnViews = columns |> List.map (fun x -> x :> IView)
            AttrBuilder<'t>.CreateContentMultiple("Columns", ValueSome getter, ValueNone, tableColumnViews)

[<AutoOpen>]
module TableViewColumn =
    open System
    open Avalonia.Controls
    open Avalonia.FuncUI.Types
    open Avalonia.FuncUI.Builder
    open Avalonia.Data
    open Avalonia.Controls.Templates
    open Avalonia.Styling
    open Avalonia.Layout
    
    let create (attrs: IAttr<TableViewColumn> list): IView<TableViewColumn> =
        ViewBuilder.Create<TableViewColumn>(attrs)
     
    type TableViewColumn  with

        static member actualWidth<'t when 't :> TableViewColumn>(actualWidth : double) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<double>(TableViewColumn.ActualWidthProperty, actualWidth, ValueNone)

        static member binding<'t when 't :> TableViewColumn>(binding : BindingBase | null) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<BindingBase | null>(TableViewColumn.BindingProperty, binding, ValueNone)

        static member canUserEffectivelyResize<'t when 't :> TableViewColumn>(canUserEffectivelyResize : bool) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<bool>(TableViewColumn.CanUserEffectivelyResizeProperty, canUserEffectivelyResize, ValueNone)

        static member canUserResize<'t when 't :> TableViewColumn>(canUserResize : Nullable<bool>) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<Nullable<bool>>(TableViewColumn.CanUserResizeProperty, canUserResize, ValueNone)

        static member cellTemplate<'t when 't :> TableViewColumn>(cellTemplate : IDataTemplate | null) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<IDataTemplate | null>(TableViewColumn.CellTemplateProperty, cellTemplate, ValueNone)

        static member cellTheme<'t when 't :> TableViewColumn>(cellTheme : ControlTheme | null) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<ControlTheme | null>(TableViewColumn.CellThemeProperty, cellTheme, ValueNone)

        static member header<'t when 't :> TableViewColumn>(header : obj | null) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<obj | null>(TableViewColumn.HeaderProperty, header, ValueNone)

        static member headerTemplate<'t when 't :> TableViewColumn>(headerTemplate : IDataTemplate | null) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<IDataTemplate | null>(TableViewColumn.HeaderTemplateProperty, headerTemplate, ValueNone)

        static member headerTheme<'t when 't :> TableViewColumn>(headerTheme : ControlTheme | null) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<ControlTheme | null>(TableViewColumn.HeaderThemeProperty, headerTheme, ValueNone)

        static member horizontalContentAlignment<'t when 't :> TableViewColumn>(horizontalContentAlignment : HorizontalAlignment) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<HorizontalAlignment>(TableViewColumn.HorizontalContentAlignmentProperty, horizontalContentAlignment, ValueNone)

        static member tableView<'t when 't :> TableViewColumn>(tableView : TableView | null) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<TableView | null>(TableViewColumn.TableViewProperty, tableView, ValueNone)
        
        static member width<'t when 't :> TableViewColumn>(width : GridLength) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<GridLength>(TableViewColumn.WidthProperty, width, ValueNone)

[<AutoOpen>]
module TableViewCell =
    open Avalonia.Controls
    open Avalonia.FuncUI.Types
    open Avalonia.FuncUI.Builder
    
    let create (attrs: IAttr<TableViewCell > list): IView<TableViewCell > =
        ViewBuilder.Create<TableViewCell>(attrs)

    type TableViewCell  with

        static member column<'t when 't :> TableViewCell>(column: TableViewColumn) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<TableViewColumn>(TableViewCell.ColumnProperty, column, ValueNone)

[<AutoOpen>]
module TableViewColumnHeader =
    open Avalonia.Controls
    open Avalonia.FuncUI.Types
    open Avalonia.FuncUI.Builder
    
    let create (attrs: IAttr<TableViewColumnHeader > list): IView<TableViewColumnHeader > =
        ViewBuilder.Create<TableViewColumnHeader>(attrs)
     
    type TableViewColumnHeader  with

        static member column<'t when 't :> TableViewColumnHeader>(column: TableViewColumn) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<TableViewColumn>(TableViewColumnHeader.ColumnProperty, column, ValueNone)

[<AutoOpen>]
module TableViewRow =
    open Avalonia.Controls
    open Avalonia.FuncUI.Types
    let create (attrs: IAttr<TableViewRow > list): IView<TableViewRow > =
        ViewBuilder.Create<TableViewRow >(attrs)