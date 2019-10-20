namespace Avalonia.FuncUI.DSL
open Avalonia.FuncUI.Components

[<AutoOpen>]
module LazyView =  
    open Avalonia.Controls
    open Avalonia.FuncUI.Types
    open Avalonia.FuncUI.Components
    open Avalonia.Media

    let create<'state, 'args>(attrs: IAttr<LazyView<'state, 'args>> list): IView<LazyView<'state, 'args>> =
        View.create<LazyView<'state, 'args>>(attrs)
    
    type LazyView<'state, 'args> with
            
        static member args(value: 'args) : IAttr<LazyView<'state, 'args>> =
            let accessor = Accessor.AvaloniaProperty LazyView<'state, 'args>.ArgsProperty
            let property = Property.createDirect(accessor, value)
            let attr = Attr.createProperty<_> property
            attr :> IAttr<_>
            
        static member state(value: 'state) : IAttr<LazyView<'state, 'args>> =
            let accessor = Accessor.AvaloniaProperty LazyView<'state, 'args>.StateProperty
            let property = Property.createDirect(accessor, value)
            let attr = Attr.createProperty<_> property
            attr :> IAttr<_>
            
        static member viewFunc(value: ('state -> 'args -> IView) option) : IAttr<LazyView<'state, 'args>> =
            let accessor = Accessor.AvaloniaProperty LazyView<'state, 'args>.ViewFuncProperty
            let property = Property.createDirect(accessor, value)
            let attr = Attr.createProperty<_> property
            attr :> IAttr<_>
            
        static member viewFunc(value: 'state -> 'args -> IView) : IAttr<LazyView<'state, 'args>> =
            value |> Some |> LazyView<'state, 'args>.viewFunc