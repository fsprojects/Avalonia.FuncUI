#r "../_lib/Fornax.Core.dll"
#load "layout.fsx"

open Html

let generate' (ctx: SiteContents) (page: string) =
    let guide =
        ctx.TryGetValues<Guideloader.Guide> ()
        |> Option.defaultValue Seq.empty
        |> Seq.find (fun n -> n.file = page)
    
    Layout.layout ctx guide.title [
        article [Class "guide box"] [
            header [Class "guide-header"] [
                h1 [Class "title"] [!!guide.title]
                h4 [Class "subtitle"] [
                    match guide.author with 
                    | Some author -> !!author
                    | None -> !!"Avalonia Community"
                ]
                section [Class "tags"] [
                    for tag in guide.tags do
                        span [Class "tag is-light"][ !!tag ]
                ]
            ]
            section [Class "guide-content content"] [!!guide.content]
        ]
    ]

let generate (ctx : SiteContents) (projectRoot: string) (page: string) =
    generate' ctx page
    |> Layout.render ctx