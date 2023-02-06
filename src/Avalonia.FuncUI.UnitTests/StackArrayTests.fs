namespace Avalonia.FuncUI.UnitTests.StackArray

open System.Collections.Generic
open Avalonia
open Avalonia.FuncUI
open Avalonia.Styling
open Microsoft.FSharp.NativeInterop
open Xunit

#nowarn "9"

[<RequireQualifiedAccess>]
module StackArrayTests =
    open Avalonia.FuncUI.DSL

    [<Fact>]
    let ``StackArray`` () =
        let arr = collection {
            1; 2; 3;
        }


        printfn $"%A{List.ofArray arr.Buffer}"
        ()

        //Assert.Equal<int>([1; 2; 3; 4; 5; 6; 7; 8; 9], arr)
