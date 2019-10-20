namespace Avalonia.FuncUI.DSL

[<AutoOpen>]
module Grid =  
    open Avalonia.Controls
    open Avalonia.FuncUI.Types
    
    let create (attrs: IAttr<Grid> list): IView<Grid> =
        View.create<Grid>(attrs)
        
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
        static member dock<'t when 't :> Grid>(dock: Dock) : IAttr<'t> =
            let accessor = Accessor.AvaloniaProperty DockPanel.DockProperty
            let property = Property.createAttached(accessor, dock)
            let attr = Attr.createProperty<'t> property
            attr :> IAttr<'t>
    
    type Grid with
        
        static member showGridLines<'t when 't :> Grid>(value: bool) : IAttr<'t> =
            let accessor = Accessor.AvaloniaProperty Grid.ShowGridLinesProperty
            let property = Property.createDirect(accessor, value)
            let attr = Attr.createProperty<'t> property
            attr :> IAttr<'t>
            
        static member columnDefinitions<'t when 't :> Grid>(value: ColumnDefinitions) : IAttr<'t> =
            let accessor = Accessor.create(
                "ColumnDefinitions",
                ValueSome (fun c -> (c :?> Grid).ColumnDefinitions :> obj),
                ValueSome (fun (c, v) -> (c :?> Grid).ColumnDefinitions <- (v :?> ColumnDefinitions)))
            
            let property = Property.createDirect'(Accessor.InstanceProperty accessor, value, Internals.compareColumnDefinitions)
            let attr = Attr.createProperty<'t> property
            attr :> IAttr<'t>
            
        static member columnDefinitions<'t when 't :> Grid>(value: string) : IAttr<'t> =
            value |> ColumnDefinitions.Parse |> Grid.columnDefinitions 
        
        static member rowDefinitions<'t when 't :> Grid>(value: RowDefinitions) : IAttr<'t> =
            let accessor = Accessor.create(
                "RowDefinitions",
                ValueSome (fun c -> (c :?> Grid).RowDefinitions :> obj),
                ValueSome (fun (c, v) -> (c :?> Grid).RowDefinitions <- (v :?> RowDefinitions)))
            
            let property = Property.createDirect'(Accessor.InstanceProperty accessor, value, Internals.compareRowDefinitions)
            let attr = Attr.createProperty<'t> property
            attr :> IAttr<'t>
            
        static member rowDefinitions<'t when 't :> Grid>(value: string) : IAttr<'t> =
            value |> RowDefinitions.Parse |> Grid.rowDefinitions 