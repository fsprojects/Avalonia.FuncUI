namespace Avalonia.FuncUI.DSL

open Avalonia.Controls.Primitives

[<AutoOpen>]
module HeaderedItemsControl =  
    open Avalonia
    open Avalonia.FuncUI.Types
    open Avalonia.FuncUI.Builder
            
    type HeaderedItemsControl with
    
        /// <summary>
        /// Gets or sets the content of the control's header.
        /// </summary>
        static member header<'t when 't :> HeaderedItemsControl>(value: obj) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<obj>(HeaderedItemsControl.HeaderProperty, value, ValueNone)