namespace Avalonia.FuncUI.DSL

open Avalonia.FuncUI.Types
open System.Threading

[<AutoOpen>]
module Helpers =
    let generalize (view: IView<'t>) : IView =
        view :> IView