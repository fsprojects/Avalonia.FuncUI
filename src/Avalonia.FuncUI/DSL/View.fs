namespace Avalonia.FuncUI.DSL

open System.Diagnostics.CodeAnalysis
open Avalonia
open Avalonia.FuncUI.Types

[<AbstractClass; Sealed>]
type View () =

    /// <summary>
    /// Creates a new IView&lt;'t&gt; with the given attributes.
    /// <example>
    /// <code>
    /// View.createGeneric&lt;Button&gt; [
    ///     Button.content "Click me"
    ///     ..
    /// ]
    /// </code>
    /// </example>
    /// </summary>
    static member createGeneric<[<DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)>] 'view when 'view :> AvaloniaObject>
      ( attrs: IAttr<'view> list ) =

        { View.ViewType = typeof<'view>
          View.ViewKey = ValueNone
          View.Attrs = attrs
          View.ConstructorArgs = null
          View.Outlet = ValueNone }
        :> IView<'view>

    /// <summary>
    /// Creates a new IView&lt;'view&gt; with the given attributes and key.
    /// <example>
    /// <code>
    /// View.createWithKey "key" Button.create [
    ///     Button.content "Click me"
    ///     ..
    /// ]
    /// </code>
    /// </example>
    /// </summary>
    static member createWithKey<[<DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)>] 'view when 'view :> AvaloniaObject>
      ( key: string )
      ( createView: IAttr<'view> list -> IView<'view>)
      ( attrs: IAttr<'view> list ) =

        let view = createView(attrs)

        { View.ViewType = typeof<'view>
          View.ViewKey = ValueSome key
          View.Attrs = view.Attrs
          View.ConstructorArgs = view.ConstructorArgs
          View.Outlet = ValueNone }
        :> IView<'view>

    /// <summary>
    /// Creates a new IView&lt;'view&gt; with the given attributes and outlet.
    /// <example>
    /// <code>
    ///
    /// let mutable button: Button = null
    ///
    /// View.createWithOutlet (fun btnRef -&gt; button &lt;- btnRef) Button.create [
    ///     Button.content "Click me"
    ///     ..
    /// ]
    /// </code>
    /// </example>
    /// <remarks>Consider using an init attribute instead.</remarks>
    /// </summary>
    static member createWithOutlet<[<DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)>] 'view when 'view :> AvaloniaObject>
      ( outlet: 'view -> unit )
      ( createView: IAttr<'view> list -> IView<'view> )
      ( attrs: IAttr<'view> list ) =

        let view = createView(attrs)

        { View.ViewType = typeof<'view>
          View.ViewKey = view.ViewKey
          View.Attrs = view.Attrs
          View.ConstructorArgs = view.ConstructorArgs
          View.Outlet = ValueSome (fun control -> outlet (control :?> 'view)) }
        :> IView<'view>

    /// <summary>
    /// Creates a new IView&lt;'view&gt; with the given attributes and key.
    /// <example>
    /// <code>
    /// [
    ///     Button.content "Click me"
    ///     ..
    /// ]
    /// |&gt; Button.create
    /// |&gt; View.withKey "key"
    /// </code>
    /// </example>
    /// </summary>
    static member withKey<[<DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)>] 'view when 'view :> AvaloniaObject>
      ( key: string )
      ( view: IView<'view> ) : IView<'view> =

        { View.ViewType = view.ViewType
          View.ViewKey = ValueSome key
          View.Attrs = view.Attrs
          View.ConstructorArgs = view.ConstructorArgs
          View.Outlet = view.Outlet }

     /// <summary>
    /// Creates a new IView&lt;'view&gt; with the given attributes and outlet.
    /// <example>
    /// <code>
    ///
    /// let mutable button: Button = null
    ///
    /// [
    ///     Button.content "Click me"
    ///     ..
    /// ]
    /// |&gt; Button.create
    /// |&gt; View.withOutlet (fun btnRef -&gt; button &lt;- btnRef)
    /// </code>
    /// </example>
    /// <remarks>Consider using an init attribute instead.</remarks>
    /// </summary>
    static member withOutlet<[<DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)>] 'view when 'view :> AvaloniaObject>
      (outlet: 'view -> unit)
      (view: IView<'view>) : IView<'view> =

        { View.ViewType = view.ViewType
          View.ViewKey = view.ViewKey
          View.Attrs = view.Attrs
          View.ConstructorArgs = view.ConstructorArgs
          View.Outlet = ValueSome (fun control -> outlet (control :?> 'view)) }

    /// <summary>
    /// Creates a new IView&lt;'view&gt; with constructor arguments.
    /// <example>
    /// <code>
    /// [
    ///     ...
    /// ]
    /// |&gt; MyView.create
    /// |&gt; View.withConstructorArgs [| arg1 :&gt; obj; arg1 :&gt; obj; |]
    /// </code>
    /// </example>
    /// </summary>
    static member withConstructorArgs<[<DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)>] 'view when 'view :> AvaloniaObject>
      ( constructorArgs: obj array )
      ( view: IView<'view> ) : IView<'view> =

        { View.ViewType = view.ViewType
          View.ViewKey = view.ViewKey
          View.Attrs = view.Attrs
          View.ConstructorArgs = constructorArgs
          View.Outlet = view.Outlet }