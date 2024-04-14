[<AutoOpen>]
module Avalonia.FuncUI.DSL.__ComponentExtensions

open System
open Avalonia.FuncUI
open Avalonia.FuncUI.Builder
open Avalonia.FuncUI.Types

type Component with

    static member internal renderFunction<'t when 't :> Component>(value: IComponentContext -> IView) : IAttr<'t> =
        AttrBuilder<'t>.CreateProperty<Func<IComponentContext, IView>>(Component.RenderFunctionProperty, value, ValueNone)

    static member create(key: string, render: IComponentContext -> IView) : IView<Component> =
        { View.ViewType = typeof<Component>
          View.ViewKey = ValueSome key
          View.Attrs = List.Empty
          View.Outlet = ValueNone
          View.ConstructorArgs = [| render :> obj |] }
        :> IView<Component>

    static member create(render: IComponentContext -> IView) : IView<Component> =
        { View.ViewType = typeof<Component>
          View.ViewKey = ValueNone
          View.Attrs = [
            Component.renderFunction render
          ]
          View.Outlet = ValueNone
          View.ConstructorArgs = [| render :> obj |] }
        :> IView<Component>