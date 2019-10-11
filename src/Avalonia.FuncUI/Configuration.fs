namespace Avalonia.FuncUI
open System.Collections.Generic
open System.Linq
open Avalonia.Controls

module Configuration =
    open Avalonia.FuncUI.VirtualDom
    
    let defaultConfiguration =
        let provided = new ProvidedEquality()
       
        provided.ProvideFor (typeof<RowDefinitions>, (fun (a, b) -> 
            let a = a :?> RowDefinitions
            let b = b :?> RowDefinitions
                
            let comparer =
                {
                    new IEqualityComparer<RowDefinition> with
                        member this.Equals (a, b) : bool =
                            a.Height = b.Height &&
                            a.MinHeight = b.MinHeight &&
                            a.MaxHeight = b.MaxHeight
                            
                        member this.GetHashCode (a) =
                            (a.Height, a.MinHeight, a.MaxHeight).GetHashCode()
                }
            
            Enumerable.SequenceEqual(a, b, comparer);
        ))
        
        provided.ProvideFor (typeof<ColumnDefinitions>, (fun (a, b) -> 
            let a = a :?> ColumnDefinitions
            let b = b :?> ColumnDefinitions
                
            let comparer =
                {
                    new IEqualityComparer<ColumnDefinition> with
                        member this.Equals (a, b) : bool =
                            a.Width = b.Width &&
                            a.MinWidth = b.MinWidth &&
                            a.MaxWidth = b.MaxWidth
                            
                        member this.GetHashCode (a) =
                            (a.Width, a.MinWidth, a.MaxWidth).GetHashCode()
                }
            
            Enumerable.SequenceEqual(a, b, comparer);
        ))
        
        { providedEquality = provided }