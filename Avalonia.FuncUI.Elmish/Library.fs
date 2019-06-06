namespace Avalonia.FuncUI.Elmish

open Avalonia.FuncUI.Hosts
open Elmish
open Avalonia.FuncUI.Types

module Program =

    let withHost (host: IViewHost) (program: Program<'arg, 'model, 'msg, View>) =
        let setState state dispatch =
            let view = ((Program.view program) state dispatch)
            host.UpdateView view

        program |> Program.withSetState setState
