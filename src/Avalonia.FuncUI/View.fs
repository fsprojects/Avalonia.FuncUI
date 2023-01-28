namespace Avalonia.FuncUI.DSL

open Avalonia.FuncUI.Types

[<AbstractClass; Sealed>]
type View () =

    static member createWithKey (key: string) (createView: Attr<'view> list -> View<'view>) (attrs: Attr<'view> list) =
        let view = createView(attrs)

        { View.ViewType = typeof<'view>
          View.ViewKey = ValueSome key
          View.Attrs = view.Attrs
          View.ConstructorArgs = view.ConstructorArgs
          View.Outlet = ValueNone }

    static member createWithOutlet (outlet: 'view -> unit) (createView: Attr<'view> list -> View<'view>) (attrs: Attr<'view> list) =
        let view = createView(attrs)

        { View.ViewType = typeof<'view>
          View.ViewKey = view.ViewKey
          View.Attrs = view.Attrs
          View.ConstructorArgs = view.ConstructorArgs
          View.Outlet = ValueSome (fun control -> outlet (control :?> 'view)) }

    static member withKey (key: string) (view: View<'view>) : View<'view> =
        { View.ViewType = view.ViewType
          View.ViewKey = ValueSome key
          View.Attrs = view.Attrs
          View.ConstructorArgs = view.ConstructorArgs
          View.Outlet = view.Outlet }

    static member withOutlet (outlet: 'view -> unit) (view: View<'view>) : View<'view> =
        { View.ViewType = view.ViewType
          View.ViewKey = view.ViewKey
          View.Attrs = view.Attrs
          View.ConstructorArgs = view.ConstructorArgs
          View.Outlet = ValueSome (fun control -> outlet (control :?> 'view)) }

    static member withConstructorArgs (constructorArgs: obj array) (view: View<'view>) : View<'view> =
        { View.ViewType = view.ViewType
          View.ViewKey = view.ViewKey
          View.Attrs = view.Attrs
          View.ConstructorArgs = constructorArgs
          View.Outlet = view.Outlet }