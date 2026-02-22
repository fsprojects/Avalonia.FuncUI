namespace Avalonia.FuncUI.DSL

[<AutoOpen>]
module Image =
    open Avalonia.Controls
    open Avalonia.FuncUI.Builder
    open Avalonia.FuncUI.Types
    open Avalonia.Media
    open Avalonia.Media.Imaging

    let create (attrs: IAttr<Image> list): IView<Image> =
        ViewBuilder.Create<Image>(attrs)
    
    type Image with
        static member source<'t when 't :> Image>(value: IImage) : IAttr<'t> =
            // TODO: maybe add custom bitmap comparer OR pass enum that has different options ?
            AttrBuilder<'t>.CreateProperty<IImage>(Image.SourceProperty, value, ValueNone)
            
        static member stretch<'t when 't :> Image>(value: Stretch) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<Stretch>(Image.StretchProperty, value, ValueNone)
        
        static member stretchDirection<'t when 't :> Image>(value: StretchDirection) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<StretchDirection>(Image.StretchDirectionProperty, value, ValueNone)

        static member blendMode<'t when 't :> Image>(value: BitmapBlendingMode) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<BitmapBlendingMode>(Image.BlendModeProperty, value, ValueNone)
