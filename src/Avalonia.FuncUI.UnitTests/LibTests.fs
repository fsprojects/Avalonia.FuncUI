namespace Avalonia.FuncUI.UnitTests

open Xunit
open System

module LibTests =
    module FuncTests = 
        open Avalonia.FuncUI.Lib

        type Msg = Increment | Decrement

        type State = { count : int }

        [<Fact>]
        let ``Comparing funcs`` () =

            let a = fun (state: State, dispatch: Msg -> unit) -> dispatch Increment
            let b = fun (state: State, dispatch: Msg -> unit) -> dispatch Increment
            let c = fun (state: State, dispatch: Msg -> unit) -> dispatch Decrement
            let d = fun (state: State, dispatch: Msg -> unit) -> a(state, dispatch)

            Assert.True(Func.compare a b)
            Assert.False(Func.compare a c)
            Assert.True(Func.isComparable a)
            Assert.True(Func.isComparable b)
            Assert.False(Func.isComparable d)