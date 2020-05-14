namespace Avalonia.FuncUI.DSL

open Avalonia.Controls

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

                        member this.GetHashCode (a) =
                            (a.Height, a.MinHeight, a.MaxHeight, a.SharedSizeGroup).GetHashCode()
                }
            
            Enumerable.SequenceEqual(a, b, comparer);

    type Control with
        static member row<'t when 't :> Control>(row: int) : IAttr<'t> =
            AttrBuilder.CreateProperty<'t, int>(Grid.RowProperty, row, ValueNone)
            
        static member rowSpan<'t when 't :> Control>(span: int) : IAttr<'t> =
            AttrBuilder.CreateProperty<'t, int>(Grid.RowSpanProperty, span, ValueNone)
            
        static member column<'t when 't :> Control>(column: int) : IAttr<'t> =
            AttrBuilder.CreateProperty<'t, int>(Grid.ColumnProperty, column, ValueNone)
            
        static member columnSpan<'t when 't :> Control>(span: int) : IAttr<'t> =
            AttrBuilder.CreateProperty<'t, int>(Grid.ColumnSpanProperty, span, ValueNone)
            
        static member isSharedSizeScope<'t when 't :> Control>(value: bool) : IAttr<'t> =
            AttrBuilder.CreateProperty<'t, bool>(Grid.IsSharedSizeScopeProperty, value, ValueNone)
    
    type Grid with
        
        static member showGridLines<'t when 't :> Grid>(value: bool) : IAttr<'t> =
            AttrBuilder.CreateProperty<'t, bool>(Grid.ShowGridLinesProperty, value, ValueNone)

        static member columnDefinitions<'t when 't :> Grid>(value: ColumnDefinitions) : IAttr<'t> =
            let getter : 't -> ColumnDefinitions = fun view -> view.ColumnDefinitions
            let setter : 't * ColumnDefinitions -> unit = fun (view, value) -> view.ColumnDefinitions <- value
            
            AttrBuilder.CreateProperty<'t, ColumnDefinitions>("ColumnDefinitions", value, ValueSome getter, ValueSome setter, ValueSome Internals.compareColumnDefinitions)

        static member columnDefinitions<'t when 't :> Grid>(value: string) : IAttr<'t> =
            value |> ColumnDefinitions.Parse |> Grid.columnDefinitions
            
        static member columnDefinitions<'t when 't :> Grid>(content: IView<ColumnDefinitions>) : IAttr<'t> =
            let getter : 't -> obj = fun view -> view.ColumnDefinitions :> obj
            let setter : 't * obj -> unit = fun (view, value) -> view.ColumnDefinitions <- value :?> ColumnDefinitions
            
            AttrBuilder.CreateContentSingle("ColumnDefinitions", ValueSome getter, ValueSome setter, Some (content :> IView))

        static member rowDefinitions<'t when 't :> Grid>(value: RowDefinitions) : IAttr<'t> =
            let getter : 't -> RowDefinitions = fun view -> view.RowDefinitions
            let setter : 't * RowDefinitions -> unit = fun (view, value) -> view.RowDefinitions <- value
            
            AttrBuilder.CreateProperty<'t, RowDefinitions>("RowDefinitions", value, ValueSome getter, ValueSome setter, ValueSome Internals.compareRowDefinitions)

        static member rowDefinitions<'t when 't :> Grid>(value: string) : IAttr<'t> =
            value |> RowDefinitions.Parse |> Grid.rowDefinitions 