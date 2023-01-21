module Avalonia.FuncUI.Elmish.ElmishHook

open Avalonia.FuncUI
open Elmish

type ElmishState<'model, 'msg, 'arg when 'model : equality>(mkProgram : unit -> Program<'arg, 'model, 'msg, unit>, arg: 'arg) =
    let program = mkProgram()

    let mutable state, cmd =
        let model, cmd = Program.init program arg
        (model, fun (_: 'msg) -> ()), cmd

    let setState model dispatch =
        let oldModel = fst state
        state <- model, dispatch
        if not (obj.ReferenceEquals(model, oldModel)) then
            ()
            // sync
    do  
        program
        |> Program.withSetState setState
        |> Program.runWith arg

    member this.Model = fst state
    member this.Dispatch = snd state

let noView = (fun _ _ -> ())

type IComponentContext with
    
    member this.useRealElmish<'arg, 'model, 'msg when 'model : equality> (mkProgram : unit -> Program<'arg, 'model, 'msg, unit>, arg: 'arg) =
        let elmishState = this.useState (ElmishState(mkProgram, arg))
        
        elmishState.Current.Model, (fun msg -> 
            elmishState.Current.Dispatch msg
            elmishState.Set elmishState.Current
        )
