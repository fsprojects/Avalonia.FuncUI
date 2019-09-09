namespace Avalonia.FuncUI.Elmish

open Avalonia.FuncUI.Hosts
open Elmish
open Avalonia.FuncUI.Core.Domain

module Program =

    let withHost (host: IViewHost) (program: Program<'arg, 'model, 'msg, #IView>) =
        let setState state dispatch =
            let view = ((Program.view program) state dispatch)
            host.Update (Some (view :> IView))

        program |> Program.withSetState setState
