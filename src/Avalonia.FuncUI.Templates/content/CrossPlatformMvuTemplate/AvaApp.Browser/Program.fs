open System.Runtime.Versioning
open Avalonia
open Avalonia.Browser
open AvaApp

// Translated from C# source generator for
// [JSImport("globalThis.window.open")]
// internal static partial void Open([JSMarshalAs<JSType.String>] string url);
#nowarn "9"
open FSharp.NativeInterop
open type System.MemoryExtensions
open System.Runtime.InteropServices.JavaScript
[<System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Interop.JavaScript.JSImportGenerator", "7.0.8.17405")>]
type OpenUrl =
    [<System.ThreadStatic; DefaultValue>]
    static val mutable private __signature_Open_589601652 : JSFunctionBinding
    static member Open(url: string) =
        if isNull OpenUrl.__signature_Open_589601652 then 
            OpenUrl.__signature_Open_589601652 <- JSFunctionBinding.BindJSFunction("globalThis.window.open", null, [|JSMarshalerType.Discard; JSMarshalerType.String|].AsSpan())
        let __arguments_buffer = System.Span<JSMarshalerArgument>(NativePtr.stackalloc<JSMarshalerArgument> 3 |> NativePtr.toVoidPtr, 3)
        let __arg_exception = &__arguments_buffer[0]
        __arg_exception.Initialize()
        let __arg_return = &__arguments_buffer[1]
        __arg_return.Initialize()
        // Setup - Perform required setup.
        let __url_native__js_arg = &__arguments_buffer[2]
        __url_native__js_arg.ToJS url
        JSFunctionBinding.InvokeJS(OpenUrl.__signature_Open_589601652, __arguments_buffer)

module Program =
    [<assembly: SupportedOSPlatform("browser")>]
    do ()

    [<CompiledName "BuildAvaloniaApp">] 
    let buildAvaloniaApp () = 
        AppBuilder
            .Configure<App>()

    [<EntryPoint>]
    let main argv =
        About.urlOpen <- OpenUrl.Open
        buildAvaloniaApp()
            .StartBrowserAppAsync("out")
        |> ignore
        0