namespace Avalonia.FuncUI.Builders

module Command =
    open System.Windows.Input

    (* canExecuteChanged is not implemented correctly, only to satisfy the interface *)
    type private FunCommand =   
        val private canExecuteChanged: Event<System.EventHandler, System.EventArgs>
        val private execute: obj -> unit
        val private canExecute: obj -> bool
        
        new (execute) =
            {
                canExecute = fun param -> true
                execute = execute
                canExecuteChanged = new Event<_, _>()
            }

        new (execute, canExecute) =
            {
                canExecute = fun param -> canExecute
                execute = execute
                canExecuteChanged = new Event<_, _>()
            }

        new (execute, canExecute) =
            {
                canExecute = canExecute
                execute = execute
                canExecuteChanged = new Event<_, _>()
            }


        interface ICommand with

            [<CLIEvent>]
            member this.CanExecuteChanged = this.canExecuteChanged.Publish
                
            member this.Execute (param: obj) =
                this.execute param

            member this.CanExecute (param: obj) =
                this.canExecute param

    let from (execute: obj -> unit) : ICommand =
        FunCommand(execute) :> ICommand

