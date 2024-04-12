namespace Avalonia.FuncUI

open System
open System.ComponentModel
open System.Diagnostics.CodeAnalysis
open Avalonia
open Avalonia.Controls
open Avalonia.FuncUI
open Avalonia.FuncUI.Types
open Avalonia.FuncUI.VirtualDom
open Avalonia.Threading

[<AllowNullLiteral>]
[<DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)>]
type Component (render: IComponentContext -> IView) as this =
    inherit ComponentBase ()

    static let RenderFunctionProperty =
        AvaloniaProperty.RegisterDirect<Component, IComponentContext -> IView>(
            name = "RenderFunction",
            getter = Func<Component, IComponentContext -> IView>(_.RenderFunction),
            setter = (fun this value -> this.RenderFunction <- value)
        )

    static do
        let _ = RenderFunctionProperty.Changed.AddClassHandler<Component, IComponentContext -> IView>(fun this e ->
            this.ForceRender()

        )
        ()

    let mutable _renderFunction: IComponentContext -> IView = render

    member this.RenderFunction
        with get() = _renderFunction
        and set(value) =
            let didChange = this.SetAndRaise(RenderFunctionProperty, ref _renderFunction, value)
            _renderFunction <- value
            ()

    override this.Render ctx =
        _renderFunction ctx