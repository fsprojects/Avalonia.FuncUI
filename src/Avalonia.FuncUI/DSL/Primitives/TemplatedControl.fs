namespace Avalonia.FuncUI.DSL

[<AutoOpen>]
module TemplatedControl =
    open Avalonia
    open Avalonia.Controls
    open Avalonia.Controls.Primitives
    open Avalonia.Controls.Templates
    open Avalonia.Media
    open Avalonia.Media.Immutable
    open Avalonia.FuncUI.Types
    open Avalonia.FuncUI.Builder
    
    let create (attrs: IAttr<TemplatedControl> list): IView<TemplatedControl> =
        ViewBuilder.Create<TemplatedControl>(attrs)

    type Control with
        static member isTemplateFocusTarget<'t when 't :> Control>(value: bool) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<bool>(TemplatedControl.IsTemplateFocusTargetProperty, value, ValueNone)

    type TemplatedControl with
        static member onTemplateApplied<'t when 't :> TemplatedControl>(func: TemplateAppliedEventArgs -> unit, ?subPatchOptions) : IAttr<'t> =
            AttrBuilder<'t>.CreateSubscription(TemplatedControl.TemplateAppliedEvent, func, ?subPatchOptions = subPatchOptions)

        static member background<'t when 't :> TemplatedControl>(value: IBrush) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<IBrush>(TemplatedControl.BackgroundProperty, value, ValueNone)
            
        static member background<'t when 't :> TemplatedControl>(color: string) : IAttr<'t> =
            Color.Parse(color) |> ImmutableSolidColorBrush |> TemplatedControl.background

        static member background<'t when 't :> TemplatedControl>(color: Color) : IAttr<'t> =
            color |> ImmutableSolidColorBrush |> TemplatedControl.background

        static member borderBrush<'t when 't :> TemplatedControl>(value: IBrush) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<IBrush>(TemplatedControl.BorderBrushProperty, value, ValueNone)
            
        static member borderBrush<'t when 't :> TemplatedControl>(color: string) : IAttr<'t> =
            Color.Parse(color) |> ImmutableSolidColorBrush |> TemplatedControl.borderBrush

        static member borderBrush<'t when 't :> TemplatedControl>(color: Color) : IAttr<'t> =
            color |> ImmutableSolidColorBrush |> TemplatedControl.borderBrush
            
        static member borderThickness<'t when 't :> TemplatedControl>(value: Thickness) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<Thickness>(TemplatedControl.BorderThicknessProperty, value, ValueNone)
            
        static member borderThickness<'t when 't :> TemplatedControl>(value: float) : IAttr<'t> =
            value |> Thickness |> TemplatedControl.borderThickness

        static member borderThickness<'t when 't :> TemplatedControl>(horizontal: float, vertical: float) : IAttr<'t> =
            (horizontal, vertical) |> Thickness |> TemplatedControl.borderThickness
            
        static member borderThickness<'t when 't :> TemplatedControl>(left: float, top: float, right: float, bottom: float) : IAttr<'t> =
            (left, top, right, bottom) |> Thickness |> TemplatedControl.borderThickness
            
        static member fontFamily<'t when 't :> TemplatedControl>(value: FontFamily) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<FontFamily>(TemplatedControl.FontFamilyProperty, value, ValueNone)
            
        static member fontSize<'t when 't :> TemplatedControl>(value: double) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<double>(TemplatedControl.FontSizeProperty, value, ValueNone)
            
        static member fontStyle<'t when 't :> TemplatedControl>(value: FontStyle) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<FontStyle>(TemplatedControl.FontStyleProperty, value, ValueNone)

        static member fontWeight<'t when 't :> TemplatedControl>(value: FontWeight) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<FontWeight>(TemplatedControl.FontWeightProperty, value, ValueNone)

        static member fontStretch<'t when 't :> TemplatedControl>(value: FontStretch) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<FontStretch>(TemplatedControl.FontStretchProperty, value, ValueNone)

        static member foreground<'t when 't :> TemplatedControl>(value: IBrush) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<IBrush>(TemplatedControl.ForegroundProperty, value, ValueNone)
            
        static member foreground<'t when 't :> TemplatedControl>(color: string) : IAttr<'t> =
            Color.Parse(color) |> ImmutableSolidColorBrush |> TemplatedControl.foreground
            
        static member padding<'t when 't :> TemplatedControl>(value: Thickness) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<Thickness>(TemplatedControl.PaddingProperty, value, ValueNone)
            
        static member padding<'t when 't :> TemplatedControl>(value: float) : IAttr<'t> =
            Thickness(value) |> TemplatedControl.padding
            
        static member padding<'t when 't :> TemplatedControl>(horizontal: float, vertical: float) : IAttr<'t> =
            Thickness(horizontal, vertical) |> TemplatedControl.padding
            
        static member padding<'t when 't :> TemplatedControl>(left: float, top: float, right: float, bottom: float) : IAttr<'t> =
            Thickness(left, top, right, bottom) |> TemplatedControl.padding 

        static member cornerRadius<'t when 't :> TemplatedControl>(value: CornerRadius) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<CornerRadius>(TemplatedControl.CornerRadiusProperty, value, ValueNone)
            
        static member cornerRadius<'t when 't :> TemplatedControl>(value: float) : IAttr<'t> =
            CornerRadius(value) |> TemplatedControl.cornerRadius
            
        static member cornerRadius<'t when 't :> TemplatedControl>(horizontal: float, vertical: float) : IAttr<'t> =
            CornerRadius(horizontal, vertical) |> TemplatedControl.cornerRadius
            
        static member cornerRadius<'t when 't :> TemplatedControl>(left: float, top: float, right: float, bottom: float) : IAttr<'t> =
            CornerRadius(left, top, right, bottom) |> TemplatedControl.cornerRadius 

        static member template<'t when 't :> TemplatedControl>(value: IControlTemplate) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<IControlTemplate>(TemplatedControl.TemplateProperty, value, ValueNone)  
