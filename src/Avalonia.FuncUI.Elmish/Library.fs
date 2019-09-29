namespace Avalonia.FuncUI.Elmish

open Elmish
open Avalonia.FuncUI.Hosts
open Avalonia.FuncUI.Types

module Program =

    let withHost (host: IViewHost) (program: Program<'arg, 'model, 'msg, #IView>) =
        let setState state dispatch =
            let view = ((Program.view program) state dispatch)
            host.Update (Some (view :> IView))

        program |> Program.withSetState setState
