namespace Avalonia.FuncUI.DSL

open Avalonia.FuncUI.VirtualDom
open Avalonia.FuncUI.VirtualDom.Delta
open Avalonia.Media

[<AutoOpen>]
module Run =  
    open Avalonia.Controls.Documents
    open Avalonia.FuncUI.Builder
    open Avalonia.FuncUI.Types

    let create (attrs: IAttr<Run> list): Inline =
        let run = Run()
        
        attrs
        |> List.choose (fun attr ->
            match attr.Property with
            | Some prop -> PropertyDelta.From prop |> Some
            | None -> None
        )
        |> List.iter (Patcher.patchProperty run)
        
        run
    
    type Run with
        static member text<'t when 't :> Run>(value: string) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<string>(Run.TextProperty, value, ValueNone)
            
        static member background<'t when 't :> Run>(value: IBrush) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<IBrush>(Run.BackgroundProperty, value, ValueNone)