namespace Avalonia.FuncUI.Elmish

open Avalonia.FuncUI.Hosts
open Elmish
open Avalonia.FuncUI.Core
open Avalonia.FuncUI.Core.Model

module Program =

    let withHost (host: IViewHost) (program: Program<'arg, 'model, 'msg, ViewElement>) =
        let setState state dispatch =
            let view = ((Program.view program) state dispatch)
            host.UpdateView view

        program |> Program.withSetState setState
