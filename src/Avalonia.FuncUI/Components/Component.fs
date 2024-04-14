namespace Avalonia.FuncUI

open System
open System.ComponentModel
open System.Diagnostics.CodeAnalysis
open Avalonia
open Avalonia.Controls
open Avalonia.FuncUI
open Avalonia.FuncUI.Library
open Avalonia.FuncUI.Types
open Avalonia.FuncUI.VirtualDom
open Avalonia.Threading

[<AllowNullLiteral>]
[<DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)>]
type Component (render: IComponentContext -> IView) as this =
    inherit ComponentBase ()

    static let _RenderFunctionProperty =
        AvaloniaProperty.RegisterDirect<Component, IComponentContext -> IView>(
            name = "RenderFunction",
            getter = Func<Component, IComponentContext -> IView>(_.RenderFunction),
            setter = (fun this value -> this.RenderFunction <- value)
        )

    static do
        let _ = _RenderFunctionProperty.Changed.AddClassHandler<Component, IComponentContext -> IView>(fun this e ->
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
            let _ = this.RaisePropertyChanged(Component.RenderFunctionProperty, oldValue, value)
            ()

    override this.Render ctx =
        _renderFunction ctx