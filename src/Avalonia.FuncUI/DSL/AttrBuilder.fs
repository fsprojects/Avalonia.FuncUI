namespace Avalonia.FuncUI.DSL

open System.Diagnostics.CodeAnalysis
open Avalonia.FuncUI.Types

[<AbstractClass; Sealed>]
type ViewBuilder() =

    static member Create<[<DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)>]'view>(attrs: IAttr<'view> list) : IView<'view> =
        { View.ViewType = typeof<'view>
          View.ViewKey = ValueNone
          View.Attrs = attrs
          View.ConstructorArgs = null
          View.Outlet = ValueNone }
