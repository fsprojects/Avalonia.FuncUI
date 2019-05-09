namespace Avalonia.FuncUI.Core

open Avalonia.Controls

type IAttr<'view> =
    abstract member Apply : 'view -> unit

type Attr<'view, 'value> =
    {
        value: 'value;
        apply: 'view * 'value -> unit
    }
    interface IAttr<'view> with
        member this.Apply (view : 'view) =
            this.apply (view, this.value)

module Attr =
    let create<'view, 'value when 'view :> IControl>(value: 'value, apply: 'view * 'value -> unit) : IAttr<'view> =
        { Attr.value = value; Attr.apply = apply; } :> IAttr<'view>
            
type IViewElement = 
    abstract member Create : unit -> IControl
    abstract member Update : IControl -> unit

type ViewElement<'view when 'view :> IControl> =
    {
        create: unit -> 'view
        update: 'view * IAttr<'view> list -> unit
        attrs: IAttr<'view> list
    }
    interface IViewElement with
        member this.Update view : unit =
            this.update(view :?> 'view, this.attrs)

        member this.Create () : IControl =
            let control = this.create()
            this.update(control, this.attrs)
            control :> IControl

module ViewElement =
    let create<'view when 'view :> IControl> create update attrs =
        { ViewElement.create = create; ViewElement.update = update; attrs = attrs; }
