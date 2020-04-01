#r "../_lib/Fornax.Core.dll"
#load "layout.fsx"

open Html

let generate' (ctx: SiteContents) (page: string) =
    let control =
        ctx.TryGetValues<Controlloader.Control> ()
        |> Option.defaultValue Seq.empty
        |> Seq.find (fun n -> n.file = page)
    
    Layout.layout ctx control.name [
        article [Class "control box"] [
            header [Class "control-header"] [
                h1 [Class "title"] [!!control.name]
                h4 [Class "subtitle"] [
                    match control.group with 
                    | Some group -> !!group.ToString()
                    | None -> !!"Avalonia Community"
                ]
            ]
            section [Class "control-content content"] [!!control.content]
        ]
    ]

let generate (ctx : SiteContents) (projectRoot: string) (page: string) =
    generate' ctx page
    |> Layout.render ctx