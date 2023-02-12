namespace Avalonia.FuncUI

open System
open Avalonia.FuncUI


/// Used to create a dependency graph of state values for debugging / visualization.
[<Struct; RequireQualifiedAccess>]
type InstanceType =
    | Source
    | Adapter of sources: Map<string, IAnyReadable>

    member this.Sources with get () : InstanceSourceInfo list =
        match this with
        | Source -> List.empty
        | Adapter sources ->
            [
                for src in sources do
                    { InstanceSourceInfo.Name = src.Key
                      InstanceSourceInfo.Source = src.Value }
            ]

    static member Create (sources: (string * IAnyReadable) list) : InstanceType =
        sources
        |> Map.ofList
        |> InstanceType.Adapter

    static member Create (src: IAnyReadable) : InstanceType =
        Map.empty
        |> Map.add "src" src
        |> InstanceType.Adapter

and InstanceSourceInfo =
    { Name: string
      Source: IAnyReadable }

/// <summary>
/// <para>
/// Non generic interface for FuncUI state values.
/// </para>
/// <para>
/// Used as the lowest common denominator for all state values that implement <c>IReadable&lt;'t&gt;</c>.
/// </para>
/// </summary>
and IAnyReadable =
    inherit IDisposable
    abstract InstanceId: Guid with get
    abstract InstanceType: InstanceType with get
    abstract ValueType: Type with get
    abstract member SubscribeAny : (obj -> unit) -> IDisposable

/// <summary>
/// Readable state value that can be subscribed to.
/// </summary>
type IReadable<'value> =
    inherit IAnyReadable
    abstract member Current: 'value with get
    abstract member Subscribe : ('value -> unit) -> IDisposable

/// <summary>
/// Readable and writable state value that can be subscribed to.
/// </summary>
type IWritable<'value> =
    inherit IReadable<'value>
    abstract member Set : 'value -> unit

/// <summary>
/// State value that supports reading and writing.
/// </summary>
type State<'value>(init: 'value) =
    let instanceId = Guid.NewGuid ()
    let onChange = new Event<'value>()
    let mutable current = init

    interface IWritable<'value> with
        member this.InstanceId with get () = instanceId
        member this.InstanceType with get () = InstanceType.Source
        member this.ValueType with get () = typeof<'value>
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

/// <summary>
/// Read only / Constant state value.
/// </summary>
type ReadOnlyState<'value>(init: 'value) =
    let instanceId = Guid.NewGuid ()

    interface IReadable<'value> with
        member this.InstanceId with get () = instanceId
        member this.InstanceType with get () = InstanceType.Source
        member this.ValueType = typeof<'value>
        member this.Current with get () = init
        member this.Subscribe (_handler: 'value -> unit) =
            (* This is a constant value and therefore does never change. *)
            null
        member this.SubscribeAny (_handler: obj -> unit) : IDisposable =
            (* This is a constant value and therefore does never change. *)
            null
        member this.Dispose () =
            ()

[<AutoOpen>]
module StateExtensions =

    type IAnyReadable with

        /// <summary>
        /// Gets the current value of a non generic state value.
        /// <remarks>
        /// This uses reflection, use <c>IReadable&lt;'t&gt;.Current</c> instead when possible.
        /// </remarks>
        /// </summary>
        member this.CurrentAny with get () : obj =
            let readableTypeName = typedefof<IReadable<_>>
            let readableTypeGeneric = readableTypeName.MakeGenericType([| this.ValueType |])
            let currentGeneric = readableTypeGeneric.GetProperty("Current")
            currentGeneric.GetMethod.Invoke(this, Array.empty)


        member this.ObservableAny with get () : IObservable<obj> =
            { new IObservable<obj> with
                member _.Subscribe(observer: IObserver<obj>) =
                    this.SubscribeAny(observer.OnNext)
            }

    type IReadable<'value> with

        member this.Observable with get () : IObservable<'value> =
            { new IObservable<'value> with
                member _.Subscribe(observer: IObserver<'value>) =
                    this.Subscribe(observer.OnNext)
            }

        member this.ImmediateObservable with get () : IObservable<'value> =
            { new IObservable<'value> with
                member _.Subscribe(observer: IObserver<'value>) =
                    observer.OnNext this.Current
                    this.Subscribe(observer.OnNext)
            }

    type IWritable<'value> with

        member this.Observer with get () : IObserver<'value> =
            { new IObserver<'value> with
                member _.OnNext(value: 'value) =
                    this.Set value

                member _.OnCompleted () =
                    ()

                member _.OnError (error: Exception) =
                    ()
            }