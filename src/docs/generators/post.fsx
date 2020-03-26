#r "../_lib/Fornax.Core.dll"
#load "layout.fsx"

open Html


let generate' (ctx : SiteContents) (page: string) =
    let post =
        ctx.TryGetValues<Postloader.Post> ()
        |> Option.defaultValue Seq.empty
        |> Seq.find (fun n -> n.file = page)

    Layout.layout ctx post.title [
        article [Class "post box"] [
            header [Class "post-header"] [
                h1 [Class "title"] [!!post.title]
                h4 [Class "subtitle"] [
                    match post.author with 
                    | Some author -> !!author
                    | None -> !!"Avalonia Community"
                ]
                match post.published with 
                | Some date -> small [] [!! ("Published: " + date.ToLongDateString()) ]
                | None -> !!""
                section [Class "tags"] [
                    for tag in post.tags do
                        span [Class "tag is-light"][ !!tag ]
                ]
            ]
            section [Class "post-content content"] [!!post.content]
        ]
    ]

let generate (ctx : SiteContents) (projectRoot: string) (page: string) =
    generate' ctx page
    |> Layout.render ctx