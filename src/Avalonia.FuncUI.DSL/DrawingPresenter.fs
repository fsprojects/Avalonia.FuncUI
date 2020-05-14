namespace Avalonia.FuncUI.DSL

[<AutoOpen>]
module DrawingPresenter =
    open Avalonia.Controls
    open Avalonia.Media
    open Avalonia.FuncUI.Types
    open Avalonia.FuncUI.Builder
   
    let create (attrs: IAttr<DrawingPresenter> list): IView<DrawingPresenter> =
        ViewBuilder.Create<DrawingPresenter>(attrs)

    type DrawingPresenter with            

        static member drawing<'t when 't :> DrawingPresenter>(value: Drawing) : IAttr<'t> =
            AttrBuilder.CreateProperty<'t, Drawing>(DrawingPresenter.DrawingProperty, value, ValueNone)

        static member stretch<'t when 't :> DrawingPresenter>(value: Stretch) : IAttr<'t> =
            AttrBuilder.CreateProperty<'t, Stretch>(DrawingPresenter.StretchProperty, value, ValueNone)