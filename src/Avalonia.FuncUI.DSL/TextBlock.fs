namespace Avalonia.FuncUI.DSL
open Avalonia.Media.Immutable

[<AutoOpen>]
module TextBlock =  
    open Avalonia.Controls
    open Avalonia.Media    
    open Avalonia.FuncUI.Builder
    open Avalonia.FuncUI.Types

    let create (attrs: IAttr<TextBlock> list): IView<TextBlock> =
        ViewBuilder.Create<TextBlock>(attrs)
    
    type TextBlock with
            
        static member text<'t when 't :> TextBlock>(value: string) : IAttr<'t> =
            AttrBuilder.CreateProperty<'t, string>(TextBlock.TextProperty, value, ValueNone)
            
        static member background<'t when 't :> TextBlock>(value: IBrush) : IAttr<'t> =
            AttrBuilder.CreateProperty<'t, IBrush>(TextBlock.BackgroundProperty, value, ValueNone)
            
        static member background<'t when 't :> TextBlock>(color: string) : IAttr<'t> =
            color |> Color.Parse |> ImmutableSolidColorBrush |> TextBlock.background
        
        static member fontFamily<'t when 't :> TextBlock>(value: FontFamily) : IAttr<'t> =
            AttrBuilder.CreateProperty<'t, FontFamily>(TextBlock.FontFamilyProperty, value, ValueNone)
            
        static member fontSize<'t when 't :> TextBlock>(value: double) : IAttr<'t> =
            AttrBuilder.CreateProperty<'t, double>(TextBlock.FontSizeProperty, value, ValueNone)
            
        static member fontStyle<'t when 't :> TextBlock>(value: FontStyle) : IAttr<'t> =
            AttrBuilder.CreateProperty<'t, FontStyle>(TextBlock.FontStyleProperty, value, ValueNone)
            
        static member fontWeight<'t when 't :> TextBlock>(value: FontWeight) : IAttr<'t> =
            AttrBuilder.CreateProperty<'t, FontWeight>(TextBlock.FontWeightProperty, value, ValueNone)
            
        static member foreground<'t when 't :> TextBlock>(value: IBrush) : IAttr<'t> =
            AttrBuilder.CreateProperty<'t, IBrush>(TextBlock.ForegroundProperty, value, ValueNone)
            
        static member foreground<'t when 't :> TextBlock>(color: string) : IAttr<'t> =
            color |> Color.Parse |> ImmutableSolidColorBrush |> TextBlock.foreground
            
        static member textAlignment<'t when 't :> TextBlock>(alignment: TextAlignment) : IAttr<'t> =
            AttrBuilder.CreateProperty<'t, TextAlignment>(TextBlock.TextAlignmentProperty, alignment, ValueNone)
            
        static member textWrapping<'t when 't :> TextBlock>(value: TextWrapping) : IAttr<'t> =
            AttrBuilder.CreateProperty<'t, TextWrapping>(TextBlock.TextWrappingProperty, value, ValueNone)