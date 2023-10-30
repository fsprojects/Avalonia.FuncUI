namespace Avalonia.FuncUI.DSL

open Avalonia.Controls.Documents

[<AutoOpen>]
module TextElement =  
    open Avalonia.Media.Immutable
    open Avalonia.FuncUI.Types
    open Avalonia.FuncUI.Builder
    open Avalonia.Media
    
    let create (attrs: IAttr<TextElement> list): IView<TextElement> =
        ViewBuilder.Create<TextElement>(attrs)
        
    type TextElement with
        static member background<'t when 't :> TextElement>(value: IBrush) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<IBrush>(TextElement.BackgroundProperty, value, ValueNone)
            
        static member background<'t when 't :> TextElement>(color: string) : IAttr<'t> =
            Color.Parse(color) |> ImmutableSolidColorBrush |> TextElement.background

        static member background<'t when 't :> TextElement>(color: Color) : IAttr<'t> =
            color |> ImmutableSolidColorBrush |> TextElement.background
            
        static member fontFamily<'t when 't :> TextElement>(value: FontFamily) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<FontFamily>(TextElement.FontFamilyProperty, value, ValueNone)
            
        static member fontSize<'t when 't :> TextElement>(value: double) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<double>(TextElement.FontSizeProperty, value, ValueNone)
            
        static member fontStyle<'t when 't :> TextElement>(value: FontStyle) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<FontStyle>(TextElement.FontStyleProperty, value, ValueNone)
            
        static member fontStretch<'t when 't :> TextElement>(value: FontStretch) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<FontStretch>(TextElement.FontStretchProperty, value, ValueNone)

        static member fontWeight<'t when 't :> TextElement>(value: FontWeight) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<FontWeight>(TextElement.FontWeightProperty, value, ValueNone)
            
        static member foreground<'t when 't :> TextElement>(value: IBrush) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<IBrush>(TextElement.ForegroundProperty, value, ValueNone)
            
        static member foreground<'t when 't :> TextElement>(color: string) : IAttr<'t> =
            Color.Parse(color) |> ImmutableSolidColorBrush |> TextElement.foreground

        static member foreground<'t when 't :> TextElement>(color: Color) : IAttr<'t> =
            color |> ImmutableSolidColorBrush |> TextElement.foreground
