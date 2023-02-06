namespace Avalonia.FuncUI.DSL

open System.Collections
open Microsoft.FSharp.NativeInterop

#nowarn "9"

namespace Avalonia.FuncUI

open System
open System.Buffers

[<Struct>]
type CollectionState<'item> =
    { Buffer: 'item array
      Singleton: 'item
      Length: int }

    member this.IsSingleton =
        isNull this.Buffer

[<Struct>]
type RentedArray<'item> =
    { Buffer: 'item array
      Length: int }

    interface IDisposable with
        member this.Dispose () =
            if this.Length > 1 then
                ArrayPool.Shared.Return this.Buffer

type CollectionState<'item> with

    static member initial () =
        { CollectionState.Buffer = ArrayPool.Shared.Rent 100
          CollectionState.Singleton = Unchecked.defaultof<'item>
          CollectionState.Length = 0 }

    static member singleton(item: 'item) =
        { CollectionState.Buffer = null
          CollectionState.Singleton = item
          CollectionState.Length = 1 }

    static member private combineIntoNewArray (state1: CollectionState<'item>, state2: CollectionState<'item>) =
        let newBuffer = ArrayPool.Shared.Rent (state1.Length + state2.Length + 8)

        if state1.IsSingleton then
            newBuffer[0] <- state1.Singleton
        else
            for i in 0 .. state1.Length-1 do
                newBuffer[i] <- state1.Buffer[i]

            ArrayPool.Shared.Return state1.Buffer

        if state2.IsSingleton then
            newBuffer[state1.Length] <- state2.Singleton
        else
            for i in 0 .. state2.Length-1 do
                newBuffer[state1.Length + i] <- state2.Buffer[i]

            ArrayPool.Shared.Return state2.Buffer

        { CollectionState.Buffer = newBuffer
          CollectionState.Singleton = Unchecked.defaultof<'item>
          CollectionState.Length = state1.Length + state2.Length }

    static member combine (state1: CollectionState<'item>, state2: CollectionState<'item>) =
        if state1.IsSingleton then
            CollectionState.combineIntoNewArray (state1, state2)

        elif state1.Length + state2.Length > state1.Buffer.Length then
            if state2.IsSingleton then
                state1.Buffer[state1.Length] <- state2.Singleton
            else
                for i in 0 .. state2.Length-1 do
                    state1.Buffer[state1.Length + i] <- state2.Buffer[i]

            { state1 with Length = state1.Length + state2.Length }

        else
            CollectionState.combineIntoNewArray (state1, state2)



type CollectionBuilder () =
    member inline this.Zero () =
        //printfn "Zero"
        CollectionState.initial()

    member inline this.Yield (item: 'item) : CollectionState<'item> =
        //printfn "Yield %A" item
        CollectionState.singleton item

    member inline this.Combine(x: CollectionState<'item>, y: CollectionState<'item>) : CollectionState<'item> =
        //printfn "Combine"
        CollectionState.combine(x, y)

    member inline this.For (items: seq<'item>, f: 'item -> CollectionState<'item>) : CollectionState<'item> =
        //printfn "For"
        let mutable state = CollectionState.initial()
        for item in items do
            state <- this.Combine(state, f item)
        state

    member inline this.Delay (f: unit -> CollectionState<'item>) : CollectionState<'item> =
        //printfn "Delay"
        f()

    member inline this.Run (state: CollectionState<'item>) =
        //printfn "Run"
        { RentedArray.Buffer = state.Buffer
          RentedArray.Length = state.Length }

[<AutoOpen>]
module CollectionBuilderConst =

    let collection = CollectionBuilder()