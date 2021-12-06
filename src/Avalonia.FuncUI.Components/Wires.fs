namespace Avalonia.FuncUI

open System

type IConnectable =
    inherit IDisposable
    abstract InstanceId: Guid with get

type ITap<'signal> =
    inherit IConnectable
    abstract member CurrentSignal: 'signal with get
    abstract member Subscribe : ('signal -> unit) -> IDisposable

type IWire<'signal> =
    inherit ITap<'signal>
    abstract member Send : 'signal -> unit

type Port<'signal>(init: 'signal) =
    let instanceId = Guid.NewGuid ()
    let onSignal = new Event<'signal>()
    let mutable current = init

    interface IWire<'signal> with
        member this.InstanceId with get () = instanceId
        member this.CurrentSignal with get () = current
        member this.Subscribe (handler: 'signal -> unit) =
            onSignal.Publish.Subscribe handler

        member this.Send (signal: 'signal) : unit =
            current <- signal
            onSignal.Trigger(signal)

        member this.Dispose () =
            ()

type internal KeyedWire<'signal, 'key when 'key : comparison>
  ( wire: IWire<'signal list>,
    keyPath: 'signal -> 'key ) =

    let makeMap items =
        items
        |> Seq.map (fun item -> keyPath item, item)
        |> Map.ofSeq

    let mutable currentSource: 'signal list = wire.CurrentSignal
    let mutable current: Map<'key, 'signal> = makeMap wire.CurrentSignal

    interface IWire<Map<'key, 'signal>> with
        member this.InstanceId with get () = wire.InstanceId
        member this.CurrentSignal with get () = current
        member this.Subscribe (handler: Map<'key, 'signal> -> unit) =
            wire.Subscribe (fun value ->
                (* meh, we need a better strategy here. *)
                if (currentSource :> obj).Equals value then
                    handler current
                else
                    currentSource <- value
                    let current' = makeMap value
                    current <- current'
                    handler current'
            )

        member this.Send (signal: Map<'key, 'signal>) : unit =
            current <- signal

            wire.CurrentSignal
            |> Seq.choose (fun item -> Map.tryFind (keyPath item) signal)
            |> Seq.toList
            |> wire.Send

        member this.Dispose () =
            ()

type internal KeyFocusedWire<'signal, 'key when 'key : comparison>
  ( wire: IWire<Map<'key, 'signal>>,
    keyTap: ITap<'key> ) =

    let onSignal = Event<'signal option>()
    let mutable tapValue = keyTap.CurrentSignal
    let mutable currentSignal = wire.CurrentSignal.TryFind keyTap.CurrentSignal

    let tapSubscription =
        keyTap.Subscribe (fun value ->
            tapValue <- value
            currentSignal <- wire.CurrentSignal.TryFind tapValue
            onSignal.Trigger currentSignal
        )

    let srcSubscription =
        wire.Subscribe (fun value ->
            currentSignal <- value.TryFind tapValue
            onSignal.Trigger currentSignal
        )


    interface IWire<'signal option> with
        member this.InstanceId with get () = wire.InstanceId
        member this.CurrentSignal with get () =
            wire.CurrentSignal.TryFind tapValue

        member this.Subscribe (handler: 'signal option -> unit) =
            onSignal.Publish.Subscribe (fun value ->
                handler currentSignal
            )

        member this.Send (signal: 'signal option) : unit =
            match signal with
            | Some signal ->
                wire.CurrentSignal
                |> Map.add tapValue signal
                |> wire.Send
            | None ->
                wire.CurrentSignal
                |> Map.remove tapValue
                |> wire.Send

        member this.Dispose () =
            tapSubscription.Dispose ()
            srcSubscription.Dispose ()

type internal FilteredTap<'signal, 'filter>
  ( wire: IWire<'signal list>,
    filterTap: ITap<'filter>,
    filterFunc: 'signal -> 'filter -> bool ) =

    let onSignal = Event<'signal list>()
    let mutable tapValue = filterTap.CurrentSignal
    let mutable currentSignal =
        wire.CurrentSignal
        |> List.filter (fun signal -> filterFunc signal tapValue)

    let onChange () =
        let value =
            wire.CurrentSignal
            |> List.filter (fun signal -> filterFunc signal tapValue)

        currentSignal <- value
        onSignal.Trigger value

    let tapSubscription =
        filterTap.Subscribe (fun value ->
            tapValue <- value
            onChange ()
        )

    let srcSubscription =
        wire.Subscribe (fun _value ->
            onChange ()
        )

    interface ITap<'signal list> with
        member this.InstanceId with get () = wire.InstanceId
        member this.CurrentSignal with get () = currentSignal

        member this.Subscribe (handler: 'signal list -> unit) =
            onSignal.Publish.Subscribe (fun value ->
                handler currentSignal
            )

        member this.Dispose () =
            tapSubscription.Dispose ()
            srcSubscription.Dispose ()

type internal TraversedWire<'signal, 'key when 'key : comparison>
  ( wire: IWire<Map<'key, 'signal>>,
    key: 'key ) =

    interface IWire<'signal> with
        member this.InstanceId with get () = wire.InstanceId
        member this.CurrentSignal with get () =
            wire.CurrentSignal.[key]

        member this.Subscribe (handler: 'signal -> unit) =
            wire.Subscribe (fun value ->
                match value.TryFind key with
                | Some value -> handler value
                | None -> ()
            )

        member this.Send (signal: 'signal) : unit =
            wire.CurrentSignal
            |> Map.add key signal
            |> wire.Send

        member this.Dispose () =
            ()


[<RequireQualifiedAccess>]
module Wire =

    let keyed (keyPath: 'signal -> 'key) (wire: IWire<list<'signal>>) : IWire<Map<'key, 'signal>> =
        let keyedWire = new KeyedWire<'signal, 'key>(wire, keyPath)
        keyedWire :> _

    let sequenceBy (keyPath: 'signal -> 'key) (wire: IWire<list<'signal>>) : list<IWire<'signal>> =
        let keyedWire = new KeyedWire<'signal, 'key>(wire, keyPath)
        [
            for signal in wire.CurrentSignal do
                new TraversedWire<'signal, 'key>(keyedWire, keyPath signal) :> IWire<'signal>
        ]

    let tryFindBy (keyPath: 'signal -> 'key) (keyTap: ITap<'key>) (wire: IWire<list<'signal>>) : IWire<'signal option> =
        let keyedWire: IWire<Map<'key, 'signal>> = new KeyedWire<'signal, 'key>(wire, keyPath) :> _
        let keyFocusedWire: IWire<'signal option> = new KeyFocusedWire<'signal, 'key>(keyedWire, keyTap) :> _
        keyFocusedWire

    let filter (filterTap: ITap<'filter>) (filterFunc: 'signal -> 'filter -> bool) (wire: IWire<list<'signal>>) : ITap<'signal list> =
        let keyFocusedWire: ITap<'signal list> = new FilteredTap<'signal, 'filter>(wire, filterTap, filterFunc) :> _
        keyFocusedWire