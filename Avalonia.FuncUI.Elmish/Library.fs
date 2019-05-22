namespace Avalonia.FuncUI.Elmish

open Avalonia.FuncUI.Hosts
open Elmish
open Avalonia.FuncUI.Core

module Program =

    let withHost (host: IViewHost) (program: Program<'arg, 'model, 'msg, ViewElement>) =
        let setState state dispatch =
            let view = ((Program.view program) state dispatch)
            host.View view

        program |> Program.withSetState setState
