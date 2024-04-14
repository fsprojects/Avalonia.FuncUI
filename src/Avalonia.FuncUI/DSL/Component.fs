[<AutoOpen>]
module Avalonia.FuncUI.DSL.__ComponentExtensions

open Avalonia.FuncUI
open Avalonia.FuncUI.Builder
open Avalonia.FuncUI.Types

type Component with

    static member create(key: string, render: IComponentContext -> IView) : IView<ComponentBase> =
        { View.ViewType = typeof<Component>
          View.ViewKey = ValueSome key
          View.Attrs = [
            //Component.renderFunction render
          ]
          View.Outlet = ValueNone
          View.ConstructorArgs = [| render :> obj |] }
        :> IView<ComponentBase>

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