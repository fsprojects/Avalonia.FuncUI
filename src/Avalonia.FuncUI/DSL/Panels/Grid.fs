namespace Avalonia.FuncUI.DSL

[<AutoOpen>]
module ColumnDefinition =  
    open Avalonia.Controls
    
    [<RequireQualifiedAccess>]
    type ColumnSize =
    /// The column is auto-sized to fit it's contents
    | Auto
    /// The column is sized in device independent pixels
    | Pixel of float
    /// The column is sized as a weighted proportion of available space
    | Star of float

    type ColumnDefinition with

        /// <summary>
        /// Define the properties of a Grid column
        /// </summary>
        /// <param name="columnSize">Declare if the column size should be automatic, proportional or specified in pixels</param>
        /// <param name="minWidth">Optional minimum column width in pixels</param>
        /// <param name="maxWidth">Optional maximum column width in pixels</param>
        static member create(columnSize: ColumnSize , ?minWidth: float, ?maxWidth: float) =
            let columnDefinition = ColumnDefinition()
            match columnSize with
            | ColumnSize.Auto -> columnDefinition.Width <- GridLength(0.0, GridUnitType.Auto)
            | ColumnSize.Pixel width -> columnDefinition.Width <- GridLength(width, GridUnitType.Pixel)
            | ColumnSize.Star proportionalWidth -> columnDefinition.Width <- GridLength(proportionalWidth, GridUnitType.Star)
            minWidth |> Option.iter (fun minW -> columnDefinition.MinWidth <- minW)
            maxWidth |> Option.iter (fun maxW -> columnDefinition.MaxWidth <- maxW)
            columnDefinition

[<AutoOpen>]
module RowDefinition =  
    open Avalonia.Controls

    [<RequireQualifiedAccess>]
    type RowSize =
    /// The row is auto-sized to fit it's contents
    | Auto
    /// The row is sized in device independent pixels
    | Pixel of float
    /// The row is sized as a weighted proportion of available space
    | Star of float

    type RowDefinition with

        /// <summary>
        /// Define the properties of a Grid row
        /// </summary>
        /// <param name="rowSize">Declare if row size should be automatic, proportional or specified in pixels</param>
        /// <param name="minHeight">Optional minimum row height in pixels</param>
        /// <param name="maxHeight">Optional maximum row height in pixels</param>
        static member create(rowSize: RowSize, ?minHeight: float, ?maxHeight: float) =
            let rowDefinition = RowDefinition()
            match rowSize with
            | RowSize.Auto -> rowDefinition.Height <- GridLength(0.0, GridUnitType.Auto)
            | RowSize.Pixel height -> rowDefinition.Height <- GridLength(height, GridUnitType.Pixel)
            | RowSize.Star proportionalHeight -> rowDefinition.Height <- GridLength(proportionalHeight, GridUnitType.Star)
            minHeight |> Option.iter (fun minH -> rowDefinition.MinHeight <- minH)
            maxHeight |> Option.iter (fun maxH -> rowDefinition.MaxHeight <- maxH)
            rowDefinition

[<AutoOpen>]
module Grid =  
    open Avalonia.Controls
    open Avalonia.FuncUI.Types
    open Avalonia.FuncUI.Builder
    
    let create (attrs: IAttr<Grid> list): IView<Grid> =
        ViewBuilder.Create<Grid>(attrs)
        
    module private Internals =
        open System.Collections.Generic
        open System.Linq
        
        let compareColumnDefinitions (a: obj, b: obj): bool =
            let a = a :?> ColumnDefinitions
            let b = b :?> ColumnDefinitions
                
            let comparer =
                {
                    new IEqualityComparer<ColumnDefinition> with
                        member this.Equals (a, b) : bool =
                            a.Width = b.Width &&
                            a.MinWidth = b.MinWidth &&
                            a.MaxWidth = b.MaxWidth &&
                            a.SharedSizeGroup = b.SharedSizeGroup
                            
                        member this.GetHashCode (a) =
                            (a.Width, a.MinWidth, a.MaxWidth, a.SharedSizeGroup).GetHashCode()
                }
            
            Enumerable.SequenceEqual(a, b, comparer);
            
        let compareRowDefinitions (a: obj, b: obj): bool =
            let a = a :?> RowDefinitions
            let b = b :?> RowDefinitions
                
            let comparer =
                {
                    new IEqualityComparer<RowDefinition> with
                        member this.Equals (a, b) : bool =
                            a.Height = b.Height &&
                            a.MinHeight = b.MinHeight &&
                            a.MaxHeight = b.MaxHeight &&
                            a.SharedSizeGroup = b.SharedSizeGroup

                        member this.GetHashCode a =
                            (a.Height, a.MinHeight, a.MaxHeight, a.SharedSizeGroup).GetHashCode()
                }
            
            Enumerable.SequenceEqual(a, b, comparer);

    type Control with
        /// An attached property specifying the row of parent Grid. The applied control should be a child of a Grid for this property to take effect.
        static member row<'t when 't :> Control>(row: int) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<int>(Grid.RowProperty, row, ValueNone)
            
        /// An attached property specifying the row span of parent Grid. The applied control should be a child of a Grid for this property to take effect.
        static member rowSpan<'t when 't :> Control>(span: int) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<int>(Grid.RowSpanProperty, span, ValueNone)
            
        /// An attached property specifying the column of parent Grid. The applied control should be a child of a Grid for this property to take effect.
        static member column<'t when 't :> Control>(column: int) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<int>(Grid.ColumnProperty, column, ValueNone)
            
        /// An attached property specifying the column span of parent Grid. The applied control should be a child of a Grid for this property to take effect.
        static member columnSpan<'t when 't :> Control>(span: int) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<int>(Grid.ColumnSpanProperty, span, ValueNone)
            
        /// An attached property marking the scoping element for shared size for the parent Grid. The applied control should be a child of a Grid for this property to take effect.
        static member isSharedSizeScope<'t when 't :> Control>(value: bool) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<bool>(Grid.IsSharedSizeScopeProperty, value, ValueNone)
    
    type Grid with
        
        static member showGridLines<'t when 't :> Grid>(value: bool) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<bool>(Grid.ShowGridLinesProperty, value, ValueNone)

        static member columnDefinitions<'t when 't :> Grid>(value: ColumnDefinitions) : IAttr<'t> =
            let getter : 't -> ColumnDefinitions = fun view -> view.ColumnDefinitions
            let setter : 't * ColumnDefinitions -> unit = fun (view, value) -> view.ColumnDefinitions <- value
            
            AttrBuilder<'t>.CreateProperty<_>(
                "ColumnDefinitions",
                value,
                ValueSome getter,
                ValueSome setter,
                ValueSome Internals.compareColumnDefinitions,
                (fun () -> ColumnDefinitions())
            )

        static member columnDefinitions<'t when 't :> Grid>(value: string) : IAttr<'t> =
            value |> ColumnDefinitions.Parse |> Grid.columnDefinitions 

        static member rowDefinitions<'t when 't :> Grid>(value: RowDefinitions) : IAttr<'t> =
            let getter : 't -> RowDefinitions = fun view -> view.RowDefinitions
            let setter : 't * RowDefinitions -> unit = fun (view, value) -> view.RowDefinitions <- value
            
            AttrBuilder<'t>.CreateProperty<_>(
                "RowDefinitions",
                value,
                ValueSome getter,
                ValueSome setter,
                ValueSome Internals.compareRowDefinitions,
                (fun () -> RowDefinitions())
            )

        static member rowDefinitions<'t when 't :> Grid>(value: string) : IAttr<'t> =
            value |> RowDefinitions.Parse |> Grid.rowDefinitions 

        /// <summary>
        /// Add a list of column definitions to the grid
        /// </summary>
        /// <param name="columns">List of ColumnDefinition defining how the column widths should be divided</param>
        static member columnDefinitions (columns: ColumnDefinition list) =
            let colDefs = ColumnDefinitions()
            columns
            |> List.iter (fun column -> colDefs.Add(column))
            Grid.columnDefinitions colDefs

        /// <summary>
        /// Add a list of row definitions to the grid
        /// </summary>
        /// <param name="rows">List of RowDefinition defining how the row heights should be divided</param>
        static member rowDefinitions (rows: RowDefinition list) =
            let rowDefs = RowDefinitions()
            rows
            |> List.iter (fun row -> rowDefs.Add(row))
            Grid.rowDefinitions rowDefs
