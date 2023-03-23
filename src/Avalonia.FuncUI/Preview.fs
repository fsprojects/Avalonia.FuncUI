namespace Avalonia.FuncUI

open System
open System.Diagnostics
open Avalonia

type PreviewAttribute () =
    inherit System.Attribute ()

    member this.Args = Array.empty


[<RequireQualifiedAccess>]
module Previewer =

    open System
    open System.Runtime.InteropServices


    [<DllImport("libc", SetLastError=true)>]
    extern int posix_spawn(int& pid, string path, IntPtr file_actions, IntPtr attrp, string[] argv, string[] envp);


    let startDetachedProcess (executable: string) (arguments: string) =
        let argv = [| executable; "-c \"echo 123\"" |]
        let envp = null
        let mutable pid = 0

        match posix_spawn(&pid, executable, IntPtr.Zero, IntPtr.Zero, argv, envp) with
        | 0 ->
            printfn $"Process started with PID: {pid}"
        | error ->
            printfn $"Process failed to start. (error: %i{error})"


    let launch () =
        let executable = "/bin/zsh"
        let arguments = "-c 'funcui-preview'"
        startDetachedProcess executable arguments

        ()