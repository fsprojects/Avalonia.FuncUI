namespace Avalonia.FuncUI

open System

type IConnectable =
    inherit IDisposable
    abstract InstanceId: Guid with get

type IReadable<'value> =
    inherit IConnectable
    abstract member Current: 'value with get
    abstract member Subscribe : ('value -> unit) -> IDisposable

type IWritable<'value> =
    inherit IReadable<'value>
    abstract member Set : 'value -> unit

type Value<'value>(init: 'value) =
    let instanceId = Guid.NewGuid ()
    let onChange = new Event<'value>()
    let mutable current = init

    interface IWritable<'value> with
        member this.InstanceId with get () = instanceId
        member this.Current with get () = current
        member this.Subscribe (handler: 'value -> unit) =
            onChange.Publish.Subscribe handler

        member this.Set (signal: 'value) : unit =
            current <- signal
            onChange.Trigger(signal)

        member this.Dispose () =
            ()

type ReadOnlyValue<'value>(init: 'value) =
    let instanceId = Guid.NewGuid ()

    interface IReadable<'value> with
        member this.InstanceId with get () = instanceId
        member this.Current with get () = init
        member this.Subscribe (_handler: 'value -> unit) =
            (* This is a constant value and therefore does never change. *)
            null

        member this.Dispose () =
            ()