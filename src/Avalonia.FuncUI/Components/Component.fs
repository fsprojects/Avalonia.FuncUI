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

    static let _RenderFunctionProperty =
        AvaloniaProperty.RegisterDirect<Component, Func<IComponentContext, IView>>(
            name = "RenderFunction",
            getter = Func<Component, Func<IComponentContext, IView>>(_.RenderFunction),
            setter = (fun this value -> this.RenderFunction <- value)
        )

    static do
        let _ = _RenderFunctionProperty.Changed.AddClassHandler<Component, Func<IComponentContext, IView>>(fun this e ->
            this.ForceRender()

        )
        ()
    let mutable _renderFunction: Func<IComponentContext, IView> = render

    static member RenderFunctionProperty = _RenderFunctionProperty

    member this.RenderFunction
        with get() = _renderFunction
        and set(value) =
            let didChange = this.SetAndRaise(Component.RenderFunctionProperty, &_renderFunction, value)
            ()

    override this.Render ctx =
        _renderFunction.Invoke ctx