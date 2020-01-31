namespace Avalonia.FuncUI.DSL

[<AutoOpen>]
module LazyView =  
    open Avalonia.FuncUI.Types
    open Avalonia.FuncUI.Components
    open Avalonia.FuncUI.Builder

    let create<'state, 'args>(attrs: IAttr<LazyView<'state, 'args>> list): IView<LazyView<'state, 'args>> =
        ViewBuilder.Create<LazyView<'state, 'args>>(attrs)
    
    type LazyView<'state, 'args> with
            
        static member args(value: 'args) : IAttr<LazyView<'state, 'args>> =
            AttrBuilder<LazyView<'state, 'args>>.CreateProperty<'args>(LazyView<'state, 'args>.ArgsProperty, value, ValueNone)
            
        static member state(value: 'state) : IAttr<LazyView<'state, 'args>> =
            AttrBuilder<LazyView<'state, 'args>>.CreateProperty<'state>(LazyView<'state, 'args>.StateProperty, value, ValueNone)
            
        static member viewFunc(value: ('state -> 'args -> IView) voption) : IAttr<LazyView<'state, 'args>> =
            AttrBuilder<LazyView<'state, 'args>>.CreateProperty<_>(LazyView<'state, 'args>.ViewFuncProperty, value, ValueNone)
            
        static member viewFunc(value: 'state -> 'args -> IView) : IAttr<LazyView<'state, 'args>> =
            value |> ValueSome |> LazyView<'state, 'args>.viewFunc