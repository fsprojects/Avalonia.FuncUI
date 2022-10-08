namespace Avalonia.FuncUI.DSL

[<AutoOpen>]
module RichTextBlock =  
    open Avalonia.Controls
    open Avalonia.Controls.Documents
    open Avalonia.FuncUI.Builder
    open Avalonia.FuncUI.Types

    let create (attrs: IAttr<RichTextBlock> list): IView<RichTextBlock> =
        ViewBuilder.Create<RichTextBlock>(attrs)
    
    type RichTextBlock with
        static member inlines<'t when 't :> RichTextBlock>(value: InlineCollection) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<InlineCollection>(RichTextBlock.InlinesProperty, value, ValueNone)
            
        static member inlines<'t when 't :> RichTextBlock>(values: Inline list) : IAttr<'t> =
            let inlineCollection =
                let collection = InlineCollection()
                values |> List.iter collection.Add
                collection
            
            inlineCollection |> RichTextBlock.inlines