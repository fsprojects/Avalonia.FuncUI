namespace Avalonia.FuncUI

open System

/// Used to create a dependency graph of state values for debugging / visualization.
[<Struct; RequireQualifiedAccess>]
type InstanceType =
    | Source
    | Adapter of sources: Map<string, IAnyReadable>

    static member Create (sources: (string * IAnyReadable) list) : InstanceType =
        sources
        |> Map.ofList
        |> InstanceType.Adapter

    static member Create (src: IAnyReadable) : InstanceType =
        Map.empty
        |> Map.add "src" src
        |> InstanceType.Adapter

and IAnyReadable =
    inherit IDisposable
    abstract InstanceId: Guid with get
    abstract InstanceType: InstanceType with get
    abstract member SubscribeAny : (obj -> unit) -> IDisposable

type IReadable<'value> =
    inherit IAnyReadable
    abstract member Current: 'value with get
    abstract member Subscribe : ('value -> unit) -> IDisposable

type IWritable<'value> =
    inherit IReadable<'value>
    abstract member Set : 'value -> unit

type State<'value>(init: 'value) =
    let instanceId = Guid.NewGuid ()
    let onChange = new Event<'value>()
    let mutable current = init

    interface IWritable<'value> with
        member this.InstanceId with get () = instanceId
        member this.InstanceType with get () = InstanceType.Source
        member this.Current with get () = current
        member this.Subscribe (handler: 'value -> unit) =
            onChange.Publish.Subscribe handler
        member this.SubscribeAny (handler: obj -> unit) : IDisposable =
            onChange.Publish.Subscribe handler
        member this.Set (signal: 'value) : unit =
            current <- signal
            onChange.Trigger(signal)
        member this.Dispose () =
            ()

type ReadOnlyState<'value>(init: 'value) =
    let instanceId = Guid.NewGuid ()

    interface IReadable<'value> with
        member this.InstanceId with get () = instanceId
        member this.InstanceType with get () = InstanceType.Source
        member this.Current with get () = init
        member this.Subscribe (_handler: 'value -> unit) =
            (* This is a constant value and therefore does never change. *)
            null
        member this.SubscribeAny (handler: obj -> unit) : IDisposable =
            (* This is a constant value and therefore does never change. *)
            null
        member this.Dispose () =
            ()
