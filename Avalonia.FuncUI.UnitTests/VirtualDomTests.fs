namespace Avalonia.FuncUI.UnitTests

open Xunit
open System

module FuncCompare = 

    type Msg = Increment | Decrement

    type State = { count : int }

    [<Fact>]
    let ``Comparing funcs`` () =

        let getIL (func: 'a -> 'b) =
            let t = func.GetType()
            let m = t.GetMethod("Invoke")
            let b = m.GetMethodBody()
            b.GetILAsByteArray()

        let isComparable(func: 'a -> 'b) =
            let funcType = func.GetType()

            let hasFields = funcType.GetFields()
          
            if funcType.IsGenericType || not (Seq.isEmpty hasFields) then
                false
            else
                true

        let compare (funcA: 'a -> 'b) (funcB: 'c -> 'd) : bool=
            if not (isComparable funcA) then
                raise (Exception("function 'funcA' is generic or has outer dependencies"))

            if not (isComparable funcB) then
                raise (Exception("function 'funcB' is generic or has outer dependencies"))

            let bytesA = getIL funcA
            let bytesB = getIL funcB
            let spanA = ReadOnlySpan(bytesA)
            let spanB = ReadOnlySpan(bytesB)
            spanA.SequenceEqual(spanB)



        let a = fun (state: State, dispatch: Msg -> unit) -> dispatch Increment
        let b = fun (state: State, dispatch: Msg -> unit) -> dispatch Increment
        let c = fun (state: State, dispatch: Msg -> unit) -> a(state, dispatch)

        Assert.True(compare a b)
        Assert.False(isComparable c)