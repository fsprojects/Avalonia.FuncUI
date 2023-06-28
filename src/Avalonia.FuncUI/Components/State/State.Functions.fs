namespace Avalonia.FuncUI

open System
open System.Runtime.CompilerServices
open Avalonia.FuncUI

[<RequireQualifiedAccess>]
module State =
    (* read only functions are prefixed with 'read' *)

    let keyed (keyPath: 'value -> 'key) (wire: IWritable<list<'value>>) : IWritable<Map<'key, 'value>> =
        let keyedWire = new ValueMap<'value, 'key>(wire, keyPath)
        keyedWire :> _

    let sequenceBy (keyPath: 'value -> 'key) (wire: IWritable<list<'value>>) : list<IWritable<'value>> =
        let keyedWire = new ValueMap<'value, 'key>(wire, keyPath)
        [
            for item in wire.Current do
                let key = keyPath item
                new TraversedValue<'value, 'key>(keyedWire, key) :> IWritable<'value>
        ]

    let tryFindByKey (keyPath: 'value -> 'key) (key: IReadable<'key>) (wire: IWritable<list<'value>>) : IWritable<'value option> =
        let keyedWire: IWritable<Map<'key, 'value>> = new ValueMap<'value, 'key>(wire, keyPath) :> _
        let keyFocusedWire: IWritable<'value option> = new KeyFocusedValue<'value, 'key>(keyedWire, key) :> _
        keyFocusedWire

    let readFilter (filterTap: IReadable<'filter>) (filterFunc: 'value -> 'filter -> bool) (wire: IWritable<list<'value>>) : IReadable<'value list> =
        let keyFocusedWire: IReadable<'value list> = new FilteringValueList<'value, 'filter>(wire, filterTap, filterFunc) :> _
        keyFocusedWire

    let unique (state: IWritable<'value>) : IWritable<'value> =
        let uniqueWire = new UniqueValue<'value>(state)
        uniqueWire :> _

    let subscribe (handler: 'value -> unit) (state: IWritable<'value>) : IDisposable =
        state.Subscribe handler

    let readUnique (state: IReadable<'value>) : IReadable<'value> =
        let uniqueWire = new UniqueValueReadOnly<'value>(state)
        uniqueWire :> _

    let readMap (mapFunc: 'a -> 'b) (value: IReadable<'a>) : IReadable<'b> =
        new ReadValueMapped<'a, 'b>(value, mapFunc) :> _

    let readTryFindByKey (keyPath: 'value -> 'key) (key: IReadable<'key>) (wire: IReadable<list<'value>>) : IReadable<'value option> =
        let keyedWire: IReadable<Map<'key, 'value>> = new ReadValueMap<'value, 'key>(wire, keyPath) :> _
        let keyFocusedWire: IReadable<'value option> = new ReadKeyFocusedValue<'value, 'key>(keyedWire, key) :> _
        keyFocusedWire

[<Extension>]
type __IReadableExtensions =

    [<Extension>]
    static member Map<'a, 'b> (value: IReadable<'a>, mapFunc: 'a -> 'b) : IReadable<'b> =
        State.readMap mapFunc value