namespace Avalonia.FuncUI

open System
open System.Diagnostics.CodeAnalysis
open Avalonia
open Avalonia.FuncUI
open Avalonia.FuncUI.Types

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