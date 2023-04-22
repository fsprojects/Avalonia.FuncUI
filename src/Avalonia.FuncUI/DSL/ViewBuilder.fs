namespace Avalonia.FuncUI.DSL

open System.Diagnostics.CodeAnalysis
open Avalonia.FuncUI.Types

[<AbstractClass; Sealed>]
type ViewBuilder() =

    [<System.Obsolete "Use 'View.createGeneric' instead.">]
    static member Create<[<DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)>]'view>(attrs: IAttr<'view> list) : IView<'view> =
        { View.ViewType = typeof<'view>
          View.ViewKey = ValueNone
          View.Attrs = attrs
          View.ConstructorArgs = null
          View.Outlet = ValueNone }
