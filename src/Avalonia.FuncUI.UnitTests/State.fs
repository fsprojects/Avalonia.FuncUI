namespace Avalonia.FuncUI.UnitTests.VirtualDom

open System.Collections.Generic
open Avalonia
open Avalonia.FuncUI
open Avalonia.Styling

[<RequireQualifiedAccess>]
module StateTests =
    open Avalonia.FuncUI.VirtualDom
    open Avalonia.FuncUI.DSL
    open Avalonia.FuncUI.Types
    open Avalonia.Controls
    open Xunit
    open Avalonia.Media

    [<Fact>]
    let ``State.unique`` () =
        let calls = new ResizeArray<int>()
        let origin: IWritable<int> = new State<_>(0)

        let _ =
            origin
            |> State.unique
            |> State.subscribe calls.Add

        for i in  [ 1; 1; 2; 3; 3; 1; 6; ] do
            origin.Set i

        Assert.Equal([1; 2; 3; 1; 6;], calls)

        ()