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
        let calls = ResizeArray<int>()
        let origin: IWritable<int> = new State<_>(0)

        let _ =
            origin
            |> State.unique
            |> State.subscribe calls.Add

        for i in  [ 1; 1; 2; 3; 3; 1; 6; ] do
            origin.Set i

        Assert.Equal([1; 2; 3; 1; 6;], calls)

        ()

    type private Account =
        { Name: string
          Balance: int }

    [<Fact>]
    let ``State.sequenceBy`` () =
        let callsToAlice = ResizeArray<Account>()
        let callsToBob = ResizeArray<Account>()

        let accounts: IWritable<Account list> = new State<_>(List.empty)

        accounts.Set [
            { Account.Name = "Alice"; Balance = 100 }
        ]

        let aliceWire =
            accounts
            |> State.sequenceBy (fun a -> a.Name)
            |> List.item 0

        let _ = State.subscribe callsToAlice.Add aliceWire

        accounts.Set [
            yield! accounts.Current
            { Account.Name = "Bob"; Balance = 0 }
        ]

        let _ =
            accounts
            |> State.sequenceBy (fun a -> a.Name)
            |> List.item 1
            |> State.subscribe callsToBob.Add

        (* Alice gets a deposit (via toplevel) *)
        accounts.Set [
            { Account.Name = "Alice"; Balance = 99 }
            { Account.Name = "Bob"; Balance = 0 }
        ]

        Assert.Equal (2, callsToAlice.Count)
        Assert.Equal ({ Account.Name = "Alice"; Balance = 99 }, Seq.last callsToAlice)
        Assert.Equal ({ Account.Name = "Alice"; Balance = 99 }, aliceWire.Current)

        Assert.Equal (1, callsToBob.Count)
        Assert.Equal ({ Account.Name = "Bob"; Balance = 0 }, Seq.last callsToBob)

        (* Alice gets more money (via new sequenced wire) *)
        accounts
        |> State.sequenceBy (fun a -> a.Name)
        |> List.item 0
        |> fun state -> state.Set { state.Current with Balance = 101 }

        Assert.Equal (3, callsToAlice.Count)
        Assert.Equal ({ Account.Name = "Alice"; Balance = 101 }, Seq.last callsToAlice)
        Assert.Equal ({ Account.Name = "Alice"; Balance = 101 }, aliceWire.Current)

        Assert.Equal (2, callsToBob.Count)
        Assert.Equal ({ Account.Name = "Bob"; Balance = 0 }, Seq.last callsToBob)

        (* Alice gets more money (via old wire *)
        aliceWire
        |> fun state -> state.Set { state.Current with Balance = 102 }

        Assert.Equal (4, callsToAlice.Count)
        Assert.Equal ({ Account.Name = "Alice"; Balance = 102 }, Seq.last callsToAlice)
        Assert.Equal ({ Account.Name = "Alice"; Balance = 102 }, aliceWire.Current)

        Assert.Equal (3, callsToBob.Count)
        Assert.Equal ({ Account.Name = "Bob"; Balance = 0 }, Seq.last callsToBob)