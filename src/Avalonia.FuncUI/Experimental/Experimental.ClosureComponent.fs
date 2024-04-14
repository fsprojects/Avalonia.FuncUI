namespace Avalonia.FuncUI.Experimental

open System
open System.Diagnostics.CodeAnalysis
open Avalonia
open Avalonia.FuncUI
open Avalonia.FuncUI.Types
open Avalonia.FuncUI.Builder

/// Component that works well with a render function that captures state.
[<AllowNullLiteral>]
[<DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)>]
type ClosureComponent (render: IComponentContext -> IView) as this =
    inherit ComponentBase ()

    static let _RenderFunctionProperty =
        AvaloniaProperty.RegisterDirect<ClosureComponent, IComponentContext -> IView>(
            name = "RenderFunction",
            getter = Func<ClosureComponent, IComponentContext -> IView>(_.RenderFunction),
            setter = (fun this value -> this.RenderFunction <- value)
        )

    static do
        let _ = _RenderFunctionProperty.Changed.AddClassHandler<ClosureComponent, IComponentContext -> IView>(fun this e ->
            let capturesState = RenderFunctionAnalysis.capturesState(e.NewValue.Value)

            if capturesState then
                this.ForceRender()
        )
        ()

    let mutable _renderFunction = render

    static member RenderFunctionProperty = _RenderFunctionProperty

    member this.RenderFunction
        with get() = _renderFunction
        and set(value) =
            let oldValue = _renderFunction
            _renderFunction <- value
            let _ = this.RaisePropertyChanged(ClosureComponent.RenderFunctionProperty, oldValue, value)
            ()

    override this.Render ctx =
        _renderFunction ctx



[<AutoOpen>]
module __ClosureComponentExtensions =

    type ClosureComponent with

        static member internal renderFunction<'t when 't :> ClosureComponent>(value: IComponentContext -> IView) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<IComponentContext -> IView>(ClosureComponent.RenderFunctionProperty, value, ValueNone)

        static member create(key: string, render: IComponentContext -> IView) : IView<ClosureComponent> =
            let view: View<ClosureComponent> =
              { View.ViewType = typeof<ClosureComponent>
                View.ViewKey = ValueSome key
                View.Attrs = [
                  ClosureComponent.renderFunction render
                ]
                View.Outlet = ValueNone
                View.ConstructorArgs = [| render :> obj |] }

            view :> IView<ClosureComponent>

        static member create(render: IComponentContext -> IView) : IView<ClosureComponent> =
            let view: View<ClosureComponent> =
              { View.ViewType = typeof<ClosureComponent>
                View.ViewKey = ValueNone
                View.Attrs = [
                  ClosureComponent.renderFunction render
                ]
                View.Outlet = ValueNone
                View.ConstructorArgs = [| render :> obj |] }

            view :> IView<ClosureComponent>