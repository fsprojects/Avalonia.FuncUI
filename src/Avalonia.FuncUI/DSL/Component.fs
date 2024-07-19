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