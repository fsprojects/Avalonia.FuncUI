namespace Avalonia.FuncUI.DSL


[<AutoOpen>]
module SelectableTextBlock =  
    open Avalonia.Controls
    open Avalonia.Controls.Documents
    open Avalonia.FuncUI.Builder
    open Avalonia.FuncUI.Types

    let create (attrs: IAttr<SelectableTextBlock> list): IView<SelectableTextBlock> =
        ViewBuilder.Create<SelectableTextBlock>(attrs)
    
    type SelectableTextBlock with
        static member inlines<'t when 't :> SelectableTextBlock>(value: InlineCollection) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<InlineCollection>(SelectableTextBlock.InlinesProperty, value, ValueNone)
            
        static member inlines<'t when 't :> SelectableTextBlock>(values: IView list (* TODO: Change to IView<Inline> *)) : IAttr<'t> =
            let getter : ('t -> obj) = (fun control -> control.Inlines :> obj)
            AttrBuilder<'t>.CreateContentMultiple("Inlines", ValueSome getter, ValueNone, values)