namespace Avalonia.FuncUI

open System
open Avalonia.FuncUI

[<RequireQualifiedAccess>]
module State =

    let keyed (keyPath: 'signal -> 'key) (wire: IWritable<list<'signal>>) : IWritable<Map<'key, 'signal>> =
        let keyedWire = new ValueMap<'signal, 'key>(wire, keyPath)
        keyedWire :> _

    let sequenceBy (keyPath: 'signal -> 'key) (wire: IWritable<list<'signal>>) : list<IWritable<'signal>> =
        let keyedWire = new ValueMap<'signal, 'key>(wire, keyPath)
        [
            for signal in wire.Current do
                new TraversedValue<'signal, 'key>(keyedWire, keyPath signal) :> IWritable<'signal>
        ]

    let tryFindByKey (keyPath: 'signal -> 'key) (key: IReadable<'key>) (wire: IWritable<list<'signal>>) : IWritable<'signal option> =
        let keyedWire: IWritable<Map<'key, 'signal>> = new ValueMap<'signal, 'key>(wire, keyPath) :> _
        let keyFocusedWire: IWritable<'signal option> = new KeyFocusedValue<'signal, 'key>(keyedWire, key) :> _
        keyFocusedWire

    let filter (filterTap: IReadable<'filter>) (filterFunc: 'signal -> 'filter -> bool) (wire: IWritable<list<'signal>>) : IReadable<'signal list> =
        let keyFocusedWire: IReadable<'signal list> = new FilteringValueList<'signal, 'filter>(wire, filterTap, filterFunc) :> _
        keyFocusedWire

    let unique (state: IWritable<'value>) : IWritable<'value> =
        let uniqueWire = new UniqueValue<'value>(state)
        uniqueWire :> _

    let subscribe (handler: 'value -> unit) (state: IWritable<'value>) : IDisposable =
        state.Subscribe handler

    (* read only functions prefixed with 'read' *)

    let readUnique (state: IReadable<'value>) : IReadable<'value> =
        let uniqueWire = new UniqueValueReadOnly<'value>(state)
        uniqueWire :> _

    let readMap (mapFunc: 'a -> 'b) (value: IReadable<'a>) : IReadable<'b> =
        new ValueMapped<'a, 'b>(value, mapFunc) :> _

    let readTryFindByKey (keyPath: 'signal -> 'key) (key: IReadable<'key>) (wire: IReadable<list<'signal>>) : IReadable<'signal option> =
        let keyedWire: IReadable<Map<'key, 'signal>> = new ReadValueMap<'signal, 'key>(wire, keyPath) :> _
        let keyFocusedWire: IReadable<'signal option> = new ReadKeyFocusedValue<'signal, 'key>(keyedWire, key) :> _
        keyFocusedWire