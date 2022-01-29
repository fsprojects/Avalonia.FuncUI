namespace Avalonia.FuncUI

open System

type internal ValueMap<'value, 'key when 'key : comparison>
  ( value: IWritable<'value list>,
    keyPath: 'value -> 'key ) =

    let makeMap items =
        items
        |> Seq.map (fun item -> keyPath item, item)
        |> Map.ofSeq

    let mutable currentSource: 'value list = value.Current
    let mutable current: Map<'key, 'value> = makeMap value.Current

    interface IWritable<Map<'key, 'value>> with
        member this.InstanceId with get () = value.InstanceId
        member this.Current with get () = current
        member this.Subscribe (handler: Map<'key, 'value> -> unit) =
            value.Subscribe (fun value ->
                (* meh, we need a better strategy here. *)
                if (currentSource :> obj).Equals value then
                    handler current
                else
                    currentSource <- value
                    let current' = makeMap value
                    current <- current'
                    handler current'
            )

        member this.Set (signal: Map<'key, 'value>) : unit =
            current <- signal

            value.Current
            |> Seq.choose (fun item -> Map.tryFind (keyPath item) signal)
            |> Seq.toList
            |> value.Set

        member this.Dispose () =
            ()

type internal KeyFocusedValue<'value, 'key when 'key : comparison>
  ( wire: IWritable<Map<'key, 'value>>,
    key: IReadable<'key> ) =

    let onChange = Event<'value option>()
    let mutable currentKey = key.Current
    let mutable currentValue = wire.Current.TryFind key.Current

    let keySubscription =
        key.Subscribe (fun value ->
            currentKey <- value
            currentValue <- wire.Current.TryFind currentKey
            onChange.Trigger currentValue
        )

    let srcSubscription =
        wire.Subscribe (fun value ->
            currentValue <- value.TryFind currentKey
            onChange.Trigger currentValue
        )


    interface IWritable<'value option> with
        member this.InstanceId with get () = wire.InstanceId
        member this.Current with get () =
            wire.Current.TryFind currentKey

        member this.Subscribe (handler: 'value option -> unit) =
            onChange.Publish.Subscribe (fun value ->
                handler currentValue
            )

        member this.Set (signal: 'value option) : unit =
            match signal with
            | Some signal ->
                wire.Current
                |> Map.add currentKey signal
                |> wire.Set
            | None ->
                wire.Current
                |> Map.remove currentKey
                |> wire.Set

        member this.Dispose () =
            keySubscription.Dispose ()
            srcSubscription.Dispose ()

type internal FilteringValueList<'value, 'filter>
  ( value: IWritable<'value list>,
    filterValue: IReadable<'filter>,
    filterFunc: 'value -> 'filter -> bool ) =

    let onChange = Event<'value list>()

    let mutable currentSignal =
        value.Current
        |> List.filter (fun signal -> filterFunc signal filterValue.Current)

    let onKeyOrValueChange () =
        let value =
            value.Current
            |> List.filter (fun signal -> filterFunc signal filterValue.Current)

        currentSignal <- value
        onChange.Trigger value

    let tapSubscription =
        filterValue.Subscribe (fun _value ->
            onKeyOrValueChange ()
        )

    let srcSubscription =
        value.Subscribe (fun _value ->
            onKeyOrValueChange ()
        )

    interface IReadable<'value list> with
        member this.InstanceId with get () = value.InstanceId
        member this.Current with get () = currentSignal

        member this.Subscribe (handler: 'value list -> unit) =
            onChange.Publish.Subscribe (fun value ->
                handler currentSignal
            )

        member this.Dispose () =
            tapSubscription.Dispose ()
            srcSubscription.Dispose ()

type internal TraversedValue<'value, 'key when 'key : comparison>
  ( wire: IWritable<Map<'key, 'value>>,
    key: 'key ) =

    interface IWritable<'value> with
        member this.InstanceId with get () = wire.InstanceId
        member this.Current with get () =
            wire.Current.[key]

        member this.Subscribe (handler: 'value -> unit) =
            wire.Subscribe (fun value ->
                match value.TryFind key with
                | Some value -> handler value
                | None -> ()
            )

        member this.Set (signal: 'value) : unit =
            wire.Current
            |> Map.add key signal
            |> wire.Set

        member this.Dispose () =
            ()

