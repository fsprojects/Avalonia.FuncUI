#r "paket:
nuget Fake.DotNet.Cli
nuget Fake.IO.FileSystem
nuget Fake.Core.Target //"
#load ".fake/build.fsx/intellisense.fsx"

open Fake.Core
open Fake.DotNet
open Fake.IO
open Fake.IO.FileSystemOperators
open Fake.IO.Globbing.Operators
open Fake.Core.TargetOperators

Target.initEnvironment()
let docsOutputPath = Path.getFullName "../../docs"

Target.create "Clean" (fun _ ->
    let exitCode = Shell.Exec("fornax", "clean")
    Trace.tracefn "Build ExitCode: %i" exitCode)

Target.create "Build" (fun _ ->
    let exitCode = Shell.Exec("fornax", "build")
    Trace.tracefn "Build ExitCode: %i" exitCode)

Target.create "CleanDocs" (fun _ ->
    printfn "%s" docsOutputPath
    Shell.rm_rf docsOutputPath)

Target.create "CopyDocs" (fun _ ->
    Trace.tracefn "Creating Output Directory: %s" docsOutputPath
    Shell.mkdir docsOutputPath
    let src = Path.getFullName "./_public"
    let copied = 
        match Path.isDirectory src with
        | true ->
            Shell.copyRecursive src docsOutputPath false
        | false ->
            Trace.tracefn "No _public directory detected aborting..."
            List.empty

    Trace.tracefn "Publishing docs to %s" docsOutputPath

    for path in copied do
        Trace.tracefn "%s" path
)
"Clean" ==> "CleanDocs" ==> "Build" ==> "CopyDocs"

Target.create "All" ignore

"Clean" ==> "Build" ==> "All"

Target.runOrDefaultWithArguments "All"
