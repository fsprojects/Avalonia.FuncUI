namespace Avalonia.FuncUI

open System
open System.Diagnostics.CodeAnalysis
open Avalonia.Controls
open Avalonia.FuncUI
open Avalonia.FuncUI.Types
open Avalonia.FuncUI.VirtualDom
open Avalonia.Threading

[<AllowNullLiteral>]
[<DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)>]
type Component (render: IComponentContext -> IView) as this =
    inherit ComponentBase ()

    override this.Render ctx =
        render ctx

type Component with

    static member create(key: obj, render: IComponentContext -> IView) : IView<Component> =
        { View.ViewType = typeof<Component>
          View.ViewKey = ValueSome { ViewKey.Key = key; ViewKey.Comparer = null }
          View.Attrs = list.Empty
          View.Outlet = ValueNone
          View.ConstructorArgs = [| render :> obj |] }
        :> IView<Component>